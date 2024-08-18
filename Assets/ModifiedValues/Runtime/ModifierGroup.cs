using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ModifiedValues

{
	/// <summary>
	/// A handy collection of Modifiers, where you can Attach and Detach
	/// all of them at once.
	/// An example use case is a buff that has multiple Modifiers.
	/// Note: if a Modifier is in a ModifierGroup, it does not automatically
	/// mean that it is Attached to a ModifiedValue, or Active.
	/// </summary>
	public class ModifierGroup
	{
		protected HashSet<Modifier> _modifiers = new HashSet<Modifier>();
		public IReadOnlyList<Modifier> Modifiers => _modifiers.ToList().AsReadOnly();
		public IReadOnlyList<Modifier> ActiveModifiers => _modifiers.Where(m => m.Active).ToList().AsReadOnly();
		public IReadOnlyList<Modifier> InactiveModifiers => _modifiers.Where(m => !m.Active).ToList().AsReadOnly();
		public bool IsEmpty => _modifiers.Count == 0;

		public static ModifierGroup operator +(ModifierGroup group, Modifier mod)
		{
			if (!group._modifiers.Contains(mod))
			{
				//Duplicates not allowed
				group._modifiers.Add(mod);
			}
			return group;
		}

		public static ModifierGroup operator -(ModifierGroup group, Modifier mod)
		{
			//If modifier is not in group, will do nothing
			group._modifiers.Remove(mod);
			return group;
		}

		/// <summary>
		/// Do something for each Modifier inside this group.
		/// </summary>
		/// <param name="action"></param>
		public void ForEach(Action<Modifier> action)
		{
			_modifiers.ToList().ForEach(action);
		}

		/// <summary>
		/// Enables foreach-statements for ParamModGroup
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _modifiers.GetEnumerator();
		}

		/// <summary>
		/// Sets all contained Modifiers as Active.
		/// </summary>
		public void SetActive()
		{
			foreach (var mod in _modifiers)
			{
				mod.Active = true;
			}
		}

		/// <summary>
		/// Sets all contained Modifiers as not Active.
		/// </summary>
		public void SetInactive()
		{
			foreach (var mod in _modifiers)
			{
				mod.Active = false;
			}
		}

		/// <summary>
		/// Attach each contained Modifier to <paramref name="modvalue"/>.
		/// </summary>
		/// <param name="modValue"></param>
		public void Attach(ModifiedValue modValue)
		{
			foreach (var mod in _modifiers)
			{
				modValue.Attach(mod);
			}
		}

		/// <summary>
		/// Detach each contained Modifier from <paramref name="modvalue"/>.
		/// </summary>
		/// <param name="modValue"></param>
		public void Detach(ModifiedValue modValue)
		{
			foreach (var mod in _modifiers)
			{
				modValue.Detach(mod);
			}
		}

		/// <summary>
		/// Detach each contained Modifier from all its ModifiedValues.
		/// </summary>
		public void DetachFromAll()
		{
			foreach (var mod in _modifiers)
			{
				mod.DetachFromAll();
			}
		}

		/// <summary>
		/// Clears all Modifiers from this collection, but don't Detach them
		/// </summary>
		public void Clear()
		{
			_modifiers.Clear();
		}

		/// <summary>
		/// Clears all Modifiers from this collection
		/// and detaches them from all ModifiedValues
		/// </summary>
		public void ClearAndDetach()
		{
			foreach (var mod in _modifiers)
			{
				mod.DetachFromAll();
			}
			Clear();
		}

		/// <summary>
		/// For each Modifier in this collection that matches the condition,
		/// detach it from all its ModifiedValues and remove it from this collection.
		/// </summary>
		public void RemoveAndDetachWhere(Func<Modifier, bool> condition)
		{
			foreach (var mod in _modifiers.Reverse())
			{
				if (condition(mod))
				{
					mod.DetachFromAll();
					_modifiers.Remove(mod);
				}
			}
		}

		/// <summary>
		/// For each Modifier in this collection that matches the condition,
		/// detach it from all its ModifiedValues.
		/// </summary>
		public void DetachWhere(Func<Modifier, bool> condition)
		{
			foreach (var mod in _modifiers.Reverse())
			{
				if (condition(mod))
				{
					mod.DetachFromAll();
				}
			}
		}

	}
}