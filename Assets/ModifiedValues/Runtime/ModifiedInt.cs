using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedInt : ModifiedValue<int>
	{

		public ModifiedInt(Func<int> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedInt(Func<int> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedInt(int baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedInt(int baseValue) => new ModifiedInt(baseValue);

		public static Modifier<int> TemplateAdd(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<int>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<int> Add(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAdd(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateAddDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<int>.NewFromLatest((latestValue) => latestValue + amountDynamic, priority, layer, order);
		}

		public Modifier<int> AddDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			var mod = TemplateAddDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<int> TemplateAddMultiple(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<int>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<int> AddMultiple(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultiple(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateAddMultipleDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<int>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amountDynamic * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<int> AddMultipleDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<int> TemplateAddMultipleBase(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<int>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<int> AddMultipleBase(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBase(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateAddMultipleBaseDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<int>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amountDynamic * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<int> AddMultipleBaseDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			var mod = TemplateAddMultipleBaseDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<int> TemplateMul(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<int>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<int> Mul(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMul(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateMulDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<int>.NewFromLatest((latestValue) => latestValue * amountDynamic, priority, layer, order);
		}

		public Modifier<int> MulDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			var mod = TemplateMulDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<int> TemplateMinCap(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<int>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<int> MinCap(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<int> MinCapFinal(int amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateMinCapDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<int>.NewFromLatest((latestValue) => Math.Max(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<int> MinCapDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<int> MinCapFinalDynamic(ModifiedValue<int> amountDynamic)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<int> TemplateMaxCap(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<int>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<int> MaxCap(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCap(amount, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public Modifier<int> MaxCapFinal(int amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateMaxCapDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<int>.NewFromLatest((latestValue) => Math.Min(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<int> MaxCapDynamic(ModifiedValue<int> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, priority, layer, order);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<int> MaxCapFinalDynamic(ModifiedValue<int> amountDynamic)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

	}
}