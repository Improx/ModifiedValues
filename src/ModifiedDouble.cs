using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedDouble : ModifiedValue<double>
	{

		public ModifiedDouble(Func<double> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedDouble(double baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedDouble(double baseValue) => new ModifiedDouble(baseValue);

		public static Modifier<double> TemplateSet(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return new Modifier<double>((prevValue) => amount, priority, layer, order);
		}

		public Modifier<double> Set(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateAdd(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return new Modifier<double>((prevValue) => prevValue + amount, priority, layer, order);
		}

		public Modifier<double> Add(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateAddFraction(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return new Modifier<double>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction to value that was at the beginning of this layer.
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

		public static Modifier<double> TemplateMul(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return new Modifier<double>((prevValue) => prevValue * amount, priority, layer, order);
		}

		public Modifier<double> Mul(double amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<double> TemplateMinCap(double amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<double>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
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
			return new Modifier<double>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
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