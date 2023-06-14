using System.Diagnostics;

namespace ModifiedValues;

public abstract class ModifiedValue
{
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

	private T _value;
	public T Value
	{
		get
		{
			if (IsDirty)
			{
				Update();
			}
			return _value;
		}
	}
	public T DirtyValue => _value;

	public ModifiedValue(Func<T> baseValueGetter)
	{
		Debug.Assert(baseValueGetter is not null);
		_baseValueGetter = baseValueGetter;
		_value = BaseValueGetter();
	}

	public ModifiedValue(T baseValue)
	{
		Debug.Assert(baseValue is not null);
		_baseValueGetter = () => baseValue;
		_value = BaseValueGetter();
	}

	public Modifier<T> Modify(Func<T, T> operation, int priority = 0, int layer = 0, int order = 0)
	{
		Modifier<T> mod = new Modifier<T>(this, operation, priority, layer, order);
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