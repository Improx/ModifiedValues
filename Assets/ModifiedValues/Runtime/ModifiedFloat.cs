using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedFloat : ModifiedValue<float>
	{

		public ModifiedFloat(Func<float> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedFloat(Func<float> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedFloat(float baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedFloat(float baseValue) => new ModifiedFloat(baseValue);

		public static Modifier<float> TemplateSet(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return Modifier<float>.NewFromIgnored(() => amount, priority, layer, order);
		}

		public Modifier<float> Set(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateAdd(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return Modifier<float>.NewFromLatest((latestValue) => latestValue + amount, priority, layer, order);
		}

		public Modifier<float> Add(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateAddFraction(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<float>.NewFromLayerStartAndLatest((layerStartValue, latestValue) => latestValue + amount * layerStartValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction of value as it was at the start of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<float> AddFraction(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFraction(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateAddFractionBase(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return Modifier<float>.NewFromBaseAndLatest((baseValue, latestValue) => latestValue + amount * baseValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction with respect to the base value.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<float> AddFractionBase(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddFractionBase(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateMul(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return Modifier<float>.NewFromLatest((latestValue) => latestValue * amount, priority, layer, order);
		}

		public Modifier<float> Mul(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateMinCap(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<float>.NewFromLatest((latestValue) => Math.Max(latestValue, amount), priority, layer, order);
		}

		public Modifier<float> MinCap(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<float> MinCapFinal(float amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateMaxCap(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<float>.NewFromLatest((latestValue) => Math.Min(latestValue, amount), priority, layer, order);
		}

		public Modifier<float> MaxCap(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<float> MaxCapFinal(float amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateMaxCapDynamic(ModifiedValue<float> amountDynamic, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return Modifier<float>.NewFromLatest((latestValue) => Math.Min(latestValue, amountDynamic), priority, layer, order);
		}

		public Modifier<float> MaxCapDynamic(ModifiedValue<float> amountDynamic, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCapDynamic(amountDynamic, priority, layer);
			Attach(mod);
			AddDependency(amountDynamic);
			return mod;
		}

	}
}