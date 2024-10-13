using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedLong : ModifiedValue<long>
	{

		public ModifiedLong(Func<long> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedLong(Func<long> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedLong(long baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedLong(long baseValue) => new ModifiedLong(baseValue);

		public static Modifier<long> TemplateAdd(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<long>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<long> Add(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAdd(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateAddDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<long>.NewFromLatest((latestValue) => latestValue + amountDynamic, priority, layer, order);
		}

		public Modifier<long> AddDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAddDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<long> TemplateAddMultiple(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<long>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<long> AddMultiple(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultiple(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateAddMultipleDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<long>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amountDynamic * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<long> AddMultipleDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<long> TemplateAddMultipleBase(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<long>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<long> AddMultipleBase(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBase(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateAddMultipleBaseDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<long>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amountDynamic * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<long> AddMultipleBaseDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBaseDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<long> TemplateMul(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<long>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<long> Mul(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMul(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateMulDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<long>.NewFromLatest((latestValue) => latestValue * amountDynamic, priority, layer, order);
		}

		public Modifier<long> MulDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMulDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<long> TemplateMinCap(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<long>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<long> MinCap(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<long> MinCapFinal(long amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateMinCapDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<long>.NewFromLatest((latestValue) => Math.Max(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<long> MinCapDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<long> MinCapFinalDynamic(ModifiedValue<long> amountDynamic)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<long> TemplateMaxCap(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<long>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<long> MaxCap(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<long> MaxCapFinal(long amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateMaxCapDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<long>.NewFromLatest((latestValue) => Math.Min(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<long> MaxCapDynamic(ModifiedValue<long> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<long> MaxCapFinalDynamic(ModifiedValue<long> amountDynamic)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

	}
}