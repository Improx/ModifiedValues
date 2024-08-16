using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedInt : ModifiedValue<int>
	{

		public ModifiedInt(Func<int> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedInt(int baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedInt(int baseValue) => new ModifiedInt(baseValue);



	}
}