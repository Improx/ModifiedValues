namespace ModifiedValues;

public class ModifiedFloat : ModifiedValue<float>
{

	public ModifiedFloat(Func<float> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

	public ModifiedFloat(float baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

	public static implicit operator ModifiedFloat(float baseValue) => new ModifiedFloat(baseValue);

	public Modifier<float> Set(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => amount, priority, layer, DefaultOrders.Set);
	}

	public static Modifier<float> TemplateAdd(float amount, int priority = 0, int layer = 0)
	{
		return new Modifier<float>((prevValue) => prevValue + amount, priority, layer, DefaultOrders.Add);
	}

	public Modifier<float> Add(float amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateAdd(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	/// <summary>
	/// Adds this fraction to value that was at the beginning of this layer.
	/// Stacks additively.
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="priority"></param>
	/// <param name="layer"></param>
	/// <returns></returns>
	public Modifier<float> AddFraction(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, DefaultOrders.AddFraction);
	}

	public Modifier<float> Mul(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => prevValue * amount, priority, layer, DefaultOrders.Mul);
	}

	public Modifier<float> MinCap(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => Math.Max(prevValue, amount), priority, layer, DefaultOrders.Cap);
	}

	public Modifier<float> MinCapFinal(float amount)
	{
		return Modify((prevValue) => Math.Max(prevValue, amount), int.MaxValue, int.MaxValue, DefaultOrders.Cap);
	}

	public Modifier<float> MaxCap(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => Math.Min(prevValue, amount), priority, layer, DefaultOrders.Cap);
	}

	public Modifier<float> MaxCapFinal(float amount)
	{
		return Modify((prevValue) => Math.Min(prevValue, amount), int.MaxValue, int.MaxValue, DefaultOrders.Cap);
	}

}