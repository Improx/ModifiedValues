using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedUint : ModifiedValue<uint>
	{

		public ModifiedUint(Func<uint> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedUint(Func<uint> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedUint(uint baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedUint(uint baseValue) => new ModifiedUint(baseValue);

		public static Modifier<uint> TemplateAdd(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<uint> Add(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAdd(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<uint> TemplateAddDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => latestValue + amountDynamic, priority, layer, order);
		}

		public Modifier<uint> AddDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAddDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<uint> TemplateAddMultiple(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<uint>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<uint> AddMultiple(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultiple(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<uint> TemplateAddMultipleDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<uint>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amountDynamic * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<uint> AddMultipleDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<uint> TemplateAddMultipleBase(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<uint>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<uint> AddMultipleBase(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBase(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<uint> TemplateAddMultipleBaseDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<uint>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amountDynamic * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<uint> AddMultipleBaseDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBaseDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<uint> TemplateMul(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<uint> Mul(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMul(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<uint> TemplateMulDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => latestValue * amountDynamic, priority, layer, order);
		}

		public Modifier<uint> MulDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMulDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<uint> TemplateMinCap(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<uint> MinCap(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<uint> MinCapFinal(uint amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<uint> TemplateMinCapDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => Math.Max(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<uint> MinCapDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<uint> MinCapFinalDynamic(ModifiedValue<uint> amountDynamic)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<uint> TemplateMaxCap(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<uint> MaxCap(uint amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<uint> MaxCapFinal(uint amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<uint> TemplateMaxCapDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<uint>.NewFromLatest((latestValue) => Math.Min(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<uint> MaxCapDynamic(ModifiedValue<uint> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<uint> MaxCapFinalDynamic(ModifiedValue<uint> amountDynamic)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

	}
}