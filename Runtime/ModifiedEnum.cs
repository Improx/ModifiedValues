using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedEnum<T> : ModifiedValue<T> where T : Enum
	{

		public ModifiedEnum(Func<T> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedEnum(T baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedEnum<T>(T baseValue) => new ModifiedEnum<T>(baseValue);

		public static Modifier<T> TemplateSet(T other, int priority = 0, int layer = 0, int order = DefaultOrders.Set)
		{
			return new Modifier<T>((prevValue) => other, priority, layer, order);
		}

		public Modifier<T> Set(T other, int priority = 0, int layer = 0)
		{
			var mod = TemplateSet(other, priority, layer);
			Attach(mod);
			return mod;
		}

	}
}