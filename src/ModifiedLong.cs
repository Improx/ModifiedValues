using System;

namespace ModifiedValues
{

	public class ModifiedLong : ModifiedValue<long>
	{

		public ModifiedLong(Func<long> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedLong(long baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedLong(long baseValue) => new ModifiedLong(baseValue);

		public static Modifier<long> TemplateSet(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return new Modifier<long>((prevValue) => amount, priority, layer, order);
		}

		public Modifier<long> Set(long amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateAdd(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return new Modifier<long>((prevValue) => prevValue + amount, priority, layer, order);
		}

		public Modifier<long> Add(long amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateAddMultiple(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return new Modifier<long>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value that was at the beginning of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<long> AddMultiple(long amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddMultiple(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateMul(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return new Modifier<long>((prevValue) => prevValue * amount, priority, layer, order);
		}

		public Modifier<long> Mul(long amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateMinCap(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<long>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
		}

		public Modifier<long> MinCap(long amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<long> MinCapFinal(long amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<long> TemplateMaxCap(long amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<long>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
		}

		public Modifier<long> MaxCap(long amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<long> MaxCapFinal(long amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

	}
}