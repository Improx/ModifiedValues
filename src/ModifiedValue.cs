namespace ModifiedValues;

public class ModifiedValue
{
	public event EventHandler<EventArgs> ? BecameDirty;
	public bool IsDirty { get; private set; }

	public void SetDirty()
	{
		IsDirty = true;
		BecameDirty?.Invoke(this, EventArgs.Empty);
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