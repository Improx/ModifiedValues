using System;

namespace ModifiedValues
{

	public class ModifiedUlong : ModifiedValue<ulong>
	{

		public ModifiedUlong(Func<ulong> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedUlong(ulong baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedUlong(ulong baseValue) => new ModifiedUlong(baseValue);

		public static Modifier<ulong> TemplateSet(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return new Modifier<ulong>((prevValue) => amount, priority, layer, order);
		}

		public Modifier<ulong> Set(ulong amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateAdd(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return new Modifier<ulong>((prevValue) => prevValue + amount, priority, layer, order);
		}

		public Modifier<ulong> Add(ulong amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateAddMultiple(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return new Modifier<ulong>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value that was at the beginning of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<ulong> AddMultiple(ulong amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddMultiple(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateMul(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return new Modifier<ulong>((prevValue) => prevValue * amount, priority, layer, order);
		}

		public Modifier<ulong> Mul(ulong amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateMinCap(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<ulong>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
		}

		public Modifier<ulong> MinCap(ulong amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<ulong> MinCapFinal(ulong amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<ulong> TemplateMaxCap(ulong amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<ulong>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
		}

		public Modifier<ulong> MaxCap(ulong amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<ulong> MaxCapFinal(ulong amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

	}
}