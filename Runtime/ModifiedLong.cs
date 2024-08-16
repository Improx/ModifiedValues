using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedLong : ModifiedValue<long>
	{

		public ModifiedLong(Func<long> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedLong(long baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedLong(long baseValue) => new ModifiedLong(baseValue);



	}
}