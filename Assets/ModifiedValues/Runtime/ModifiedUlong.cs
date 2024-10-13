using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedUlong : ModifiedValue<ulong>
	{

		public ModifiedUlong(Func<ulong> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedUlong(Func<ulong> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedUlong(ulong baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedUlong(ulong baseValue) => new ModifiedUlong(baseValue);

		public static Modifier<ulong> TemplateAdd(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<ulong> Add(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAdd(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateAddDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => latestValue + amountDynamic, priority, layer, order);
		}

		public Modifier<ulong> AddDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAddDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<ulong> TemplateAddMultiple(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<ulong>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<ulong> AddMultiple(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultiple(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateAddMultipleDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<ulong>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amountDynamic * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<ulong> AddMultipleDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<ulong> TemplateAddMultipleBase(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<ulong>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<ulong> AddMultipleBase(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBase(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateAddMultipleBaseDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<ulong>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amountDynamic * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<ulong> AddMultipleBaseDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBaseDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<ulong> TemplateMul(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<ulong> Mul(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMul(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateMulDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => latestValue * amountDynamic, priority, layer, order);
		}

		public Modifier<ulong> MulDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMulDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<ulong> TemplateMinCap(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<ulong> MinCap(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<ulong> MinCapFinal(ulong amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateMinCapDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => Math.Max(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<ulong> MinCapDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<ulong> MinCapFinalDynamic(ModifiedValue<ulong> amountDynamic)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<ulong> TemplateMaxCap(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<ulong> MaxCap(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<ulong> MaxCapFinal(ulong amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateMaxCapDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<ulong>.NewFromLatest((latestValue) => Math.Min(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<ulong> MaxCapDynamic(ModifiedValue<ulong> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<ulong> MaxCapFinalDynamic(ModifiedValue<ulong> amountDynamic)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

	}
}