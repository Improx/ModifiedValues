namespace ModifiedValues;

using System.Collections.Generic;

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
	public Func<T> BaseValueGetter;

	private T _dirtyValue;
	public T Value
	{
		get
		{
			if (IsDirty)
			{
				Update();
			}
			return _dirtyValue;
		}
	}

	public ModifiedValue(Func<T> baseValueGetter)
	{
		BaseValueGetter = baseValueGetter;
		_dirtyValue = BaseValueGetter();
	}

	public ModifiedValue(T baseValue)
	{
		BaseValueGetter = () => baseValue;
		_dirtyValue = BaseValueGetter();
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
		//TODO: Loop through all Modifiers in order defined by their Order
		//TODO: Take into account Modifiers' Priorities
	}

	public static implicit operator T(ModifiedValue<T> m) => m.Value;
	public static explicit operator ModifiedValue<T>(T baseValue) => new ModifiedValue<T>(baseValue);

	public override string? ToString()
	{
		return (Value is null) ? null : Value.ToString();
	}

}