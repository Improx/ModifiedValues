namespace ModifiedValues;

public class ModifiedFloat : ModifiedValue<float>
{

	public ModifiedFloat(Func<float> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

	public ModifiedFloat(float baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

	public static implicit operator ModifiedFloat(float baseValue) => new ModifiedFloat(baseValue);

	public Modifier<float> Set(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => amount, priority, layer, -1000);
	}

	public Modifier<float> Add(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => prevValue + amount, priority, layer, 1000);
	}

	public Modifier<float> AddFraction(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => prevValue + amount * prevValue, priority, layer, 2000, false);
	}

	public Modifier<float> Mul(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => prevValue * amount, priority, layer, 3000, true);
	}

	public Modifier<float> MinCap(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => Math.Max(prevValue, amount), priority, layer, int.MaxValue);
	}

	public Modifier<float> MinCapFinal(float amount)
	{
		return Modify((prevValue) => Math.Max(prevValue, amount), int.MaxValue, int.MaxValue, int.MaxValue);
	}

	public Modifier<float> MaxCap(float amount, int priority = 0, int layer = 0)
	{
		return Modify((prevValue) => Math.Min(prevValue, amount), priority, layer, int.MaxValue);
	}

	public Modifier<float> MaxCapFinal(float amount)
	{
		return Modify((prevValue) => Math.Min(prevValue, amount), int.MaxValue, int.MaxValue, int.MaxValue);
	}

}