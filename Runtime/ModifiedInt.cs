using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedInt : ModifiedValue<int>
	{

		public ModifiedInt(Func<int> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedInt(int baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedInt(int baseValue) => new ModifiedInt(baseValue);

		public static Modifier<int> TemplateSet(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return new Modifier<int>((prevValue) => amount, priority, layer, order);
		}

		public Modifier<int> Set(int amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateAdd(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return new Modifier<int>((prevValue) => prevValue + amount, priority, layer, order);
		}

		public Modifier<int> Add(int amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateAddMultiple(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return new Modifier<int>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this multiple to value that was at the beginning of this layer.
		/// Stacks additively.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public Modifier<int> AddMultiple(int amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAddMultiple(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateMul(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return new Modifier<int>((prevValue) => prevValue * amount, priority, layer, order);
		}

		public Modifier<int> Mul(int amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateMinCap(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<int>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
		}

		public Modifier<int> MinCap(int amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMinCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<int> MinCapFinal(int amount)
		{
			var mod = TemplateMinCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

		public static Modifier<int> TemplateMaxCap(int amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<int>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
		}

		public Modifier<int> MaxCap(int amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMaxCap(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public Modifier<int> MaxCapFinal(int amount)
		{
			var mod = TemplateMaxCap(amount, int.MaxValue, int.MaxValue);
			Attach(mod);
			return mod;
		}

	}
}