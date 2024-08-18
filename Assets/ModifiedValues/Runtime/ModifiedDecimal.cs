using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedDecimal : ModifiedValue<decimal>
	{

		public ModifiedDecimal(Func<decimal> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedDecimal(Func<decimal> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedDecimal(decimal baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedDecimal(decimal baseValue) => new ModifiedDecimal(baseValue);

		public static Modifier<decimal> TemplateSet(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return Modifier<decimal>.NewFromIgnored(() => amount, priority, layer, order);
		}

		public Modifier<decimal> Set(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<decimal> TemplateAdd(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<decimal>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<decimal> Add(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<decimal> TemplateAddFraction(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<decimal>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<decimal> AddFraction(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFraction(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<decimal> TemplateAddFractionBase(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<decimal>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<decimal> AddFractionBase(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFractionBase(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<decimal> TemplateMul(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<decimal>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<decimal> Mul(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<decimal> TemplateMinCap(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<decimal>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<decimal> MinCap(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<decimal> MinCapFinal(decimal amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<decimal> TemplateMaxCap(decimal amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<decimal>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<decimal> MaxCap(decimal amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<decimal> MaxCapFinal(decimal amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

	}
}