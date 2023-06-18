namespace ModifiedValues;

public class ModifiedNint : ModifiedValue<nint>
{

	public ModifiedNint(Func<nint> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

	public ModifiedNint(nint baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

	public static implicit operator ModifiedNint(nint baseValue) => new ModifiedNint(baseValue);

	public static Modifier<nint> TemplateSet(nint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
	{
		return new Modifier<nint>((prevValue) => amount, priority, layer, order);
	}

	public Modifier<nint> Set(nint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateSet(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nint> TemplateAdd(nint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
	{
		return new Modifier<nint>((prevValue) => prevValue + amount, priority, layer, order);
	}

	public Modifier<nint> Add(nint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateAdd(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nint> TemplateAddMultiple(nint amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
	{
		return new Modifier<nint>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
	}

	/// <summary>
	/// Adds this multiple to value that was at the beginning of this layer.
	/// Stacks additively.
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="priority"></param>
	/// <param name="layer"></param>
	/// <returns></returns>
	public Modifier<nint> AddMultiple(nint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateAddMultiple(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nint> TemplateMul(nint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
	{
		return new Modifier<nint>((prevValue) => prevValue * amount, priority, layer, order);
	}

	public Modifier<nint> Mul(nint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateMul(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public static Modifier<nint> TemplateMinCap(nint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
	{
		return new Modifier<nint>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
	}

	public Modifier<nint> MinCap(nint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateMinCap(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public Modifier<nint> MinCapFinal(nint amount)
	{
		var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
		Attach(mod);
		return mod;
	}

	public static Modifier<nint> TemplateMaxCap(nint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
	{
		return new Modifier<nint>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
	}

	public Modifier<nint> MaxCap(nint amount, int priority = 0, int layer = 0)
	{
		var mod = TemplateMaxCap(amount, priority, layer);
		Attach(mod);
		return mod;
	}

	public Modifier<nint> MaxCapFinal(nint amount)
	{
		var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
		Attach(mod);
		return mod;
	}

}