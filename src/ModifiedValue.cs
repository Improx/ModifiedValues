public class ModifiedValue<T>
{
	public Func<T> BaseValueGetter;
	public T Value => BaseValueGetter();

	public ModifiedValue(Func<T> baseValueGetter)
	{
		BaseValueGetter = baseValueGetter;
	}

	public ModifiedValue(T baseValue)
	{
		BaseValueGetter = () => baseValue;
	}

}