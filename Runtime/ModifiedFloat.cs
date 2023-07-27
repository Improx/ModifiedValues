using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedFloat : ModifiedValue<float>
	{

		public ModifiedFloat(Func<float> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedFloat(float baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedFloat(float baseValue) => new ModifiedFloat(baseValue);

		public static Modifier<float> TemplateSet(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return new Modifier<float>((prevValue) => amount, priority, layer, order);
		}

		public Modifier<float> Set(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateAdd(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Add)
		{
			return new Modifier<float>((prevValue) => prevValue + amount, priority, layer, order);
		}

		public Modifier<float> Add(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateAdd(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateAddFraction(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.AddFraction)
		{
			return new Modifier<float>((prevValue, beginningValue) => prevValue + amount * beginningValue, priority, layer, order);
		}

		/// <summary>
		/// Adds this fraction to value that was at the beginning of this layer.
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

		public static Modifier<float> TemplateMul(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Mul)
		{
			return new Modifier<float>((prevValue) => prevValue * amount, priority, layer, order);
		}

		public Modifier<float> Mul(float amount, int priority = 0, int layer = 0)
		{
			var mod = TemplateMul(amount, priority, layer);
			Attach(mod);
			return mod;
		}

		public static Modifier<float> TemplateMinCap(float amount, int priority = 0, int layer = 0, int order = DefaultOrders.Cap)
		{
			return new Modifier<float>((prevValue) => Math.Max(prevValue, amount), priority, layer, order);
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
			return new Modifier<float>((prevValue) => Math.Min(prevValue, amount), priority, layer, order);
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

	}
}