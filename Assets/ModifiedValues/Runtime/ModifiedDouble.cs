using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedDouble : ModifiedValue<double>
	{
		public ModifiedDouble(Func<double> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedDouble(Func<double> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedDouble(double baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedDouble(double baseValue) => new ModifiedDouble(baseValue);

		public static Modifier<double> TemplateSet(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return Modifier<double>.NewFromIgnored(() => amount, priority, layer, order);
		}

		public Modifier<double> Set(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateSetDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return Modifier<double>.NewFromIgnored(() => amountDynamic, priority, layer, order);
		}

		public Modifier<double> SetDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateSetDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<double> TemplateAdd(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<double>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<double> Add(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateAddDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<double>.NewFromLatest((latestValue) => latestValue + amountDynamic, priority, layer, order);
		}

		public Modifier<double> AddDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<double> TemplateAddFraction(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<double>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<double> AddFraction(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFraction(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateAddFractionDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<double>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amountDynamic * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<double> AddFractionDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFractionDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<double> TemplateAddFractionBase(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<double>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<double> AddFractionBase(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFractionBase(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateAddFractionBaseDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<double>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amountDynamic * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amountDynamic"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<double> AddFractionBaseDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFractionBaseDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<double> TemplateMul(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<double>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<double> Mul(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateMulDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<double>.NewFromLatest((latestValue) => latestValue * amountDynamic, priority, layer, order);
		}

		public Modifier<double> MulDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateMulDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<double> TemplateMinCap(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<double>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<double> MinCap(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<double> MinCapFinal(double amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateMinCapDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<double>.NewFromLatest((latestValue) => Math.Max(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<double> MinCapDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<double> MinCapFinalDynamic(ModifiedValue<double> amountDynamic)
		{
			var mod = TemplateMinCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public static Modifier<double> TemplateMaxCap(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<double>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<double> MaxCap(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<double> MaxCapFinal(double amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateMaxCapDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<double>.NewFromLatest((latestValue) => Math.Min(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<double> MaxCapDynamic(ModifiedValue<double> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

		public Modifier<double> MaxCapFinalDynamic(ModifiedValue<double> amountDynamic)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, int.MaxValue, int.MaxValue);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

	}
}