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

	}
}