using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModifiedValues
{
	public abstract class ModifiedValue
	{
		public bool Init { get; protected set; } = false;

		/// <summary>
		/// Every time we check what Value is, we recalculate the value no matter
		/// whether IsDirty is true or not.
		/// This may be useful if Modifier operations are not pure functions and have
		/// external dependencies that may change (not recommended). If you use, non-pure
		/// Modifier operations, we recommend you call SetDirty() every time an external
		/// dependency has changed. If you don't want to keep track of all the external
		/// dependencies, you can just set UpdateEveryTime = true.
		/// The downside is lower performance.
		/// </summary>
		[HideInInspector] public bool UpdateEveryTime = false;
		public bool IsDirty { get; private set; } = true;
		public event EventHandler<EventArgs> BecameDirty;
		protected HashSet<Modifier> _modifiers = new HashSet<Modifier>();
		public IReadOnlyList<Modifier> Modifiers => _modifiers.ToList().AsReadOnly();

		public IReadOnlyList<Modifier> ActiveModifiers => _modifiers.Where(m => m.Active).ToList().AsReadOnly();
		public IReadOnlyList<Modifier> InactiveModifiers => _modifiers.Where(m => !m.Active).ToList().AsReadOnly();

		public void SetDirty()
		{
			IsDirty = true;
			BecameDirty?.Invoke(this, EventArgs.Empty);
		}

		private void ModifierChangedEventHandler(object sender, EventArgs e)
		{
			SetDirty();
		}

		private void DetachingModifierEventHandler(object sender, EventArgs e)
		{
			Detach((Modifier) sender);
		}

		private void ProbingAttachedModValuesEventHandler(object sender, Modifier.ProbingAttachedModValuesEventArgs e)
		{
			e.ModValues.Add(this);
		}

		/// <summary>
		/// Returns true if modifier was attached.
		/// Returns false if this modifier object was already attached
		/// (duplicates not allowed)
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public bool Attach(Modifier mod)
		{
			if (_modifiers.Contains(mod))
			{
				return false;
			}
			_modifiers.Add(mod);
			mod.Changed += ModifierChangedEventHandler;
			mod.DetachingFromAll += DetachingModifierEventHandler;
			mod.ProbingAttachedModValues += ProbingAttachedModValuesEventHandler;
			SetDirty();
			return true;
		}

		/// <summary>
		/// Returns true if the Modifier was found and detached.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public bool Detach(Modifier mod)
		{
			bool detached = _modifiers.Remove(mod);
			if (detached)
			{
				mod.Changed -= ModifierChangedEventHandler;
				mod.DetachingFromAll -= DetachingModifierEventHandler;
				mod.ProbingAttachedModValues -= ProbingAttachedModValuesEventHandler;
				SetDirty();
			}
			return detached;
		}

		/// <summary>
		/// Returns true if had at least one modifier that was detached.
		/// </summary>
		/// <returns></returns>
		public bool DetachAll()
		{
			return DetachWhere(m => true);
		}

		/// <summary>
		/// Returns true if detached at least one modifier.
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public bool DetachWhere(Func<Modifier, bool> condition)
		{
			bool detachedAtLeastOne = false;
			foreach (Modifier mod in _modifiers.Reverse())
			{
				//Iterating in reverse so that can keep iterating collection
				//while mods are being removed from it.
				if (condition(mod))
				{
					if (Detach(mod))
					{
						detachedAtLeastOne = true;
					}
				}
			}
			return detachedAtLeastOne;
		}

	}

	public class ModifiedValue<T> : ModifiedValue
	{
		[SerializeField] private T _savedBaseValue;
		[SerializeField][HideInInspector] private bool _usingSavedBaseValue = true;
		private Func<T> _baseValueGetter = () => default(T);
		public Func<T> BaseValueGetter
		{
			get
			{
				return _baseValueGetter;
			}
			set
			{
				_baseValueGetter = value;
				_usingSavedBaseValue = false;
			}
		}

		public T BaseValue
		{
			get
			{
				return BaseValueGetter();
			}
			set
			{
				_savedBaseValue = value;
				_usingSavedBaseValue = true;
				_baseValueGetter = () => _savedBaseValue;
			}
		}

		[SerializeField][HideInInspector] private T _prevBaseValue;
		[SerializeField][HideInInspector] private T _value;
		public T Value
		{
			get
			{
				if (!EqualityComparer<T>.Default.Equals(BaseValue, _prevBaseValue))
				{
					//Base value has changed since last time we checked for Value
					_prevBaseValue = BaseValue;
					SetDirty();
				}
				if (IsDirty || UpdateEveryTime)
				{
					UpdateValue();
				}
				return _value;
			}
		}
		public T DirtyValue => _value;

		public ModifiedValue(Func<T> baseValueGetter, bool updateEveryTime = false)
		{
			_baseValueGetter = baseValueGetter;
			_usingSavedBaseValue = false;
			_value = BaseValueGetter();
			_prevBaseValue = _value;
			UpdateEveryTime = updateEveryTime;
			Init = true;
		}

		public ModifiedValue(T baseValue, bool updateEveryTime = false)
		{
			_savedBaseValue = baseValue;
			_usingSavedBaseValue = true;
			_baseValueGetter = () => _savedBaseValue;
			_value = BaseValueGetter();
			_prevBaseValue = _value;
			UpdateEveryTime = updateEveryTime;
			Init = true;
		}

		/// <summary>
		/// A shorthand for creating a Modifier with the same parameters
		/// and attaching it to this ModifiedValue.
		/// </summary>
		/// <param name="operationCompound"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public Modifier<T> Modify(Func<T, T> operationCompound, int priority = 0, int layer = 0, int order = 0)
		{
			Modifier<T> mod = new Modifier<T>(operationCompound, priority, layer, order);
			Attach(mod);
			return mod;
		}

		/// <summary>
		/// A shorthand for creating a Modifier with the same parameters
		/// and attaching it to this ModifiedValue.
		/// </summary>
		/// <param name="operationNonCompound"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		public Modifier<T> Modify(Func<T, T, T> operationNonCompound, int priority = 0, int layer = 0, int order = 0)
		{
			Modifier<T> mod = new Modifier<T>(operationNonCompound, priority, layer, order);
			Attach(mod);
			return mod;
		}

		private T CalculateValue(IReadOnlyList<Modifier> activeModifiers)
		{
			T currentValue = BaseValueGetter();
			var activeMods = activeModifiers;
			var layers = activeMods.Select(m => m.Layer).Distinct().OrderBy(layer => layer);
			foreach (int layer in layers)
			{
				var modsInLayer = activeMods.Where(m => m.Layer == layer);
				int highestPrio = modsInLayer.Max(m => m.Priority);
				//Keep only Modifiers with highest prio and arrange them in Order:
				modsInLayer = modsInLayer.Where(m => m.Priority == highestPrio).OrderBy(m => m.Order);
				T valueAtLayerBeginning = currentValue;
				foreach (var mod in modsInLayer)
				{
					Modifier<T> typedMod = (Modifier<T>) mod;
					if (typedMod.Compound)
					{
						currentValue = typedMod.OperationCompound(currentValue);
					}
					else
					{
						currentValue = typedMod.OperationNonCompound(currentValue, valueAtLayerBeginning);
					}
				}
			}
			return currentValue;
		}

		private void UpdateValue()
		{
			_value = CalculateValue(ActiveModifiers);
		}

		#region Preview methods

		/// <summary>
		/// Previews the final value if this collection of modifiers were attached
		/// and another were detached.
		/// Just like in regular final value calculation, modifier will not have effect if it is not Active
		/// and a duplicate modifier object would not be attached if it already exists.
		/// </summary>
		/// <param name="attachModifiers"></param>
		/// <param name="detachModifiers"></param>
		/// <returns></returns>
		public T PreviewValue(IEnumerable<Modifier> attachModifiers, IEnumerable<Modifier> detachModifiers)
		{
			return CalculateValue(ActiveModifiers.Union(attachModifiers.Where(m => m.Active)).Except(detachModifiers).ToList());
		}

		/// <summary>
		/// Previews the final value if this collection of modifiers were attached.
		/// Just like in regular final value calculation, modifier will not have effect if it is not Active
		/// and a duplicate modifier object would not be attached if it already exists.
		/// </summary>
		/// <param name="attachModifiers"></param>
		/// <returns></returns>
		public T PreviewValue(IEnumerable<Modifier> attachModifiers)
		{
			return PreviewValue(attachModifiers, new List<Modifier>());
		}

		/// <summary>
		/// Previews the final value if this collection of existing modifiers were detached.
		/// </summary>
		/// <param name="detachModifiers"></param>
		/// <returns></returns>
		public T PreviewValueDetach(IEnumerable<Modifier> detachModifiers)
		{
			return PreviewValue(new List<Modifier>(), detachModifiers);
		}

		/// <summary>
		/// Previews the final value if this modifier group were attached.
		/// Just like in regular final value calculation, modifier will not have effect if it is not Active
		/// and a duplicate modifier object would not be attached if it already exists.
		/// </summary>
		/// <param name="attachModifierGroup"></param>
		/// <returns></returns>
		public T PreviewValue(ModifierGroup attachModifierGroup)
		{
			return PreviewValue(attachModifierGroup.Modifiers);
		}

		/// <summary>
		/// Previews the final value if this modifier group were attached
		/// and another were detached.
		/// Just like in regular final value calculation, modifier will not have effect if it is not Active
		/// and a duplicate modifier object would not be attached if it already exists.
		/// </summary>
		/// <param name="attachModifierGroup"></param>
		/// <param name="detachModifierGroup"></param>
		/// <returns></returns>
		public T PreviewValue(ModifierGroup attachModifierGroup, ModifierGroup detachModifierGroup)
		{
			return PreviewValue(attachModifierGroup.Modifiers, detachModifierGroup.Modifiers);
		}

		/// <summary>
		/// Previews the final value if this modifier group were detached.
		/// </summary>
		/// <param name="detachModifierGroup"></param>
		/// <returns></returns>
		public T PreviewValueDetach(ModifierGroup detachModifierGroup)
		{
			return PreviewValueDetach(detachModifierGroup.Modifiers);
		}

		/// <summary>
		/// Previews the final value if this modifier were attached.
		/// Just like in regular final value calculation, modifier will not have effect if it is not Active
		/// and a duplicate modifier object would not be attached if it already exists.
		/// </summary>
		/// <param name="attachModifier"></param>
		/// <returns></returns>
		public T PreviewValue(Modifier attachModifier)
		{
			return PreviewValue(new List<Modifier>() { attachModifier });
		}

		/// <summary>
		/// Previews the final value if this existing modifier were detached.
		/// </summary>
		/// <param name="detachModifier"></param>
		/// <returns></returns>
		public T PreviewValueDetach(Modifier detachModifier)
		{
			return PreviewValueDetach(new List<Modifier>() { detachModifier });
		}

		/// <summary>
		/// Previews the final value if this modifier were attached
		/// and another existing modifier detached.
		/// Just like in regular final value calculation, modifier will not have effect if it is not Active
		/// and a duplicate modifier object would not be attached if it already exists.
		/// </summary>
		/// <param name="attachModifier"></param>
		/// <param name="detachModifier"></param>
		/// <returns></returns>
		public T PreviewValue(Modifier attachModifier, Modifier detachModifier)
		{
			return PreviewValue(new List<Modifier>() { attachModifier }, new List<Modifier>() { detachModifier });
		}

		#endregion

		public static implicit operator T(ModifiedValue<T> m) => m.Value;
		public static implicit operator ModifiedValue<T>(T baseValue) => new ModifiedValue<T>(baseValue);

		public override string ToString()
		{
			return (Value is null) ? null : Value.ToString();
		}

		public void UseSavedBaseValue()
		{
			BaseValue = default(T);
		}

	}
}