using System.Diagnostics;

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
	protected List<Modifier> _modifiers = new List<Modifier>();
	public IReadOnlyList<Modifier> Modifiers => _modifiers.AsReadOnly();

	public void SetDirty()
	{
		IsDirty = true;
		BecameDirty?.Invoke(this, EventArgs.Empty);
	}

	public void RemoveModifier(Modifier mod)
	{
		_modifiers.Remove(mod);
		SetDirty();
	}

	public void RemoveAll()
	{
		_modifiers.Clear();
		SetDirty();
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

	public Modifier<T> Modify(Func<T, T> operation, int priority = 0, int layer = 0, int order = 0, bool compound = false)
	{
		Modifier<T> mod = new Modifier<T>(this, operation, priority, layer, order, compound);
		_modifiers.Add(mod);
		SetDirty();
		return mod;
	}

	private void Update()
	{
		T currentValue = BaseValueGetter();
		//TODO: Loop through all layers in order
		//TODO: Go through all modifiers in the layer and find the highest Priority
		//TODO: Apply all modifiers that have that highest Priority, in defined Order
		_value = currentValue;
	}

	public static implicit operator T(ModifiedValue<T> m) => m.Value;
	public static explicit operator ModifiedValue<T>(T baseValue) => new ModifiedValue<T>(baseValue);

	public override string? ToString()
	{
		return (Value is null) ? null : Value.ToString();
	}

}