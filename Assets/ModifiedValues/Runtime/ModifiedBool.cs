using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedBool : ModifiedValue<bool>
	{

		public ModifiedBool(Func<bool> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedBool(Func<bool> baseValueGetter, ModifiedValue dependency, bool updateEveryTime = false) : base(baseValueGetter, dependency, updateEveryTime) { }

		public ModifiedBool(bool baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedBool(bool baseValue) => new ModifiedBool(baseValue);

		public static Modifier<bool> TemplateNot(int priority = 0, int layer = 0, int order = DefaultOrders.Not)
		{
			return Modifier<bool>.NewFromLatest((latestValue) => !latestValue, priority, layer, order);
		}

		public Modifier<bool> Not(int priority = 0, int layer = 0, int order = DefaultOrders.Not)
		{
			var mod = TemplateNot(priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<bool> TemplateAnd(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.And)
		{
			return Modifier<bool>.NewFromLatest((latestValue) => latestValue && other, priority, layer, order);
		}

		public Modifier<bool> And(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.And)
		{
			var mod = TemplateAnd(other, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<bool> TemplateOr(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.Or)
		{
			return Modifier<bool>.NewFromLatest((latestValue) => latestValue || other, priority, layer, order);
		}

		public Modifier<bool> Or(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.Or)
		{
			var mod = TemplateOr(other, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<bool> TemplateXor(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.Xor)
		{
			return Modifier<bool>.NewFromLatest((latestValue) => latestValue ^ other, priority, layer, order);
		}

		public Modifier<bool> Xor(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.Xor)
		{
			var mod = TemplateXor(other, priority, layer, order);
			Attach(mod);
			return mod;
		}

		public static Modifier<bool> TemplateImply(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.Imply)
		{
			return Modifier<bool>.NewFromLatest((latestValue) => !latestValue || other, priority, layer, order);
		}

		public Modifier<bool> Imply(bool other, int priority = 0, int layer = 0, int order = DefaultOrders.Imply)
		{
			var mod = TemplateImply(other, priority, layer, order);
			Attach(mod);
			return mod;
		}

	}
}