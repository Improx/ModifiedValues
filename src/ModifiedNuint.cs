namespace ModifiedValues;

public class ModifiedNnuint : ModifiedValue<nuint>
{

	public ModifiedNnuint(Func<nuint> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

	public ModifiedNnuint(nuint baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

	public static implicit operator ModifiedNnuint(nuint baseValue) => new ModifiedNnuint(baseValue);

	public static Modifier<nuint> TemplateSet(nuint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
	{
		return new Modifier<nuint>((prevValue) => amount, priority, layer, order);
	}

	public Modifier<nuint> Set(nuint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateSet(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nuint> TemplateAdd(nuint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
	{
		return new Modifier<nuint>((prevValue) => prevValue + amount, priority, layer, order);
	}

	public Modifier<nuint> Add(nuint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateAdd(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nuint> TemplateAddMultiple(nuint amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
	{
		return new Modifier<nuint>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
	}

	/// <summary>
	/// Adds this multiple to value that was at the beginning of this layer.
	/// Stacks additively.
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="priority"></param>
	/// <param name="layer"></param>
	/// <returns></returns>
	public Modifier<nuint> AddMultiple(nuint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateAddMultiple(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nuint> TemplateMul(nuint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
	{
		return new Modifier<nuint>((prevValue) => prevValue * amount, priority, layer, order);
	}

	public Modifier<nuint> Mul(nuint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateMul(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nuint> TemplateMinCap(nuint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
	{
		return new Modifier<nuint>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
	}

	public Modifier<nuint> MinCap(nuint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateMinCap(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public Modifier<nuint> MinCapFinal(nuint amount)
	{
		var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
		Attach(mod);
		return mod;
	}

	public static Modifier<nuint> TemplateMaxCap(nuint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
	{
		return new Modifier<nuint>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
	}

	public Modifier<nuint> MaxCap(nuint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateMaxCap(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public Modifier<nuint> MaxCapFinal(nuint amount)
	{
		var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
		Attach(mod);
		return mod;
	}

}