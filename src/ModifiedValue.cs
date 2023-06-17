namespace ModifiedValues;

public abstract class ModifiedValue
{
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
	public bool UpdateEveryTime = false;
	public bool IsDirty { get; private set; }
	public event EventHandler<EventArgs> ? BecameDirty;
	protected HashSet<Modifier> _modifiers = new HashSet<Modifier>();
	public IReadOnlyList<Modifier> Modifiers => _modifiers.ToList().AsReadOnly();
	public IReadOnlyList<Modifier> ActiveModifiers => _modifiers.Where(m => m.Active).ToList().AsReadOnly();

	public void SetDirty()
	{
		IsDirty = true;
		BecameDirty?.Invoke(this, EventArgs.Empty);
	}

	private void ModifierChangedEventHandler(object sender, EventArgs e)
	{
		SetDirty();
	}

	private void RemovingModifierEventHandler(object sender, EventArgs e)
	{
		RemoveModifier((Modifier) sender);
	}

	/// <summary>
	/// Returns true if modifier was applied.
	/// Returns false if this modifier object was already applied (duplicates not allowed)
	/// </summary>
	/// <param name="mod"></param>
	/// <returns></returns>
	public bool ApplyModifier(Modifier mod)
	{
		if (_modifiers.Contains(mod))
		{
			return false;
		}
		_modifiers.Add(mod);
		mod.Changed += ModifierChangedEventHandler;
		mod.RemovingFromAll += RemovingModifierEventHandler;
		SetDirty();
		return true;
	}

	/// <summary>
	/// Returns true if the Modifier was found and removed.
	/// </summary>
	/// <param name="mod"></param>
	/// <returns></returns>
	public bool RemoveModifier(Modifier mod)
	{
		bool removed = _modifiers.Remove(mod);
		if (removed)
		{
			mod.Changed -= ModifierChangedEventHandler;
			mod.RemovingFromAll -= RemovingModifierEventHandler;
			SetDirty();
		}
		return removed;
	}

	/// <summary>
	/// Returns true if had at least one modifier that was removed.
	/// </summary>
	/// <returns></returns>
	public bool RemoveAll()
	{
		return RemoveWhere(m => true);
	}

	/// <summary>
	/// Returns true if removed at least one modifier.
	/// </summary>
	/// <param name="predicate"></param>
	/// <returns></returns>
	public bool RemoveWhere(Func<Modifier, bool> predicate)
	{
		bool removedAtLeastOne = false;
		foreach (Modifier mod in _modifiers.Reverse())
		{
			//Iterating in reverse so that can keep iterating collection
			//while mods are being removed from it.
			if (predicate(mod))
			{
				if (RemoveModifier(mod))
				{
					removedAtLeastOne = true;
				}
			}
		}
		return removedAtLeastOne;
	}

}

public class ModifiedValue<T> : ModifiedValue
{
	private Func<T> _baseValueGetter;
	public Func<T> BaseValueGetter
	{
		get
		{
			return _baseValueGetter;
		}
		set
		{
			_baseValueGetter = value;
			SetDirty();
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
			BaseValueGetter = () => value;
		}
	}
	private T _prevBaseValue;
	private T _value;
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
				Update();
			}
			return _value;
		}
	}
	public T DirtyValue => _value;

	public ModifiedValue(Func<T> baseValueGetter, bool updateEveryTime = false)
	{
		_baseValueGetter = baseValueGetter;
		_value = BaseValueGetter();
		_prevBaseValue = _value;
		UpdateEveryTime = updateEveryTime;
	}

	public ModifiedValue(T baseValue, bool updateEveryTime = false)
	{
		_baseValueGetter = () => baseValue;
		_value = BaseValueGetter();
		_prevBaseValue = _value;
		UpdateEveryTime = updateEveryTime;
	}

	public Modifier<T> Modify(Func<T, T> operationCompound, int priority = 0, int layer = 0, int order = 0)
	{
		Modifier<T> mod = new Modifier<T>(operationCompound, priority, layer, order);
		ApplyModifier(mod);
		return mod;
	}

	public Modifier<T> Modify(Func<T, T, T> operationNonCompound, int priority = 0, int layer = 0, int order = 0)
	{
		Modifier<T> mod = new Modifier<T>(operationNonCompound, priority, layer, order);
		ApplyModifier(mod);
		return mod;
	}

	private void Update()
	{
		T currentValue = BaseValueGetter();
		var activeMods = ActiveModifiers;
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
		_value = currentValue;
	}

	public static implicit operator T(ModifiedValue<T> m) => m.Value;
	public static implicit operator ModifiedValue<T>(T baseValue) => new ModifiedValue<T>(baseValue);

	public override string? ToString()
	{
		return (Value is null) ? null : Value.ToString();
	}

}