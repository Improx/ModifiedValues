using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedUlong : ModifiedValue<ulong>
	{

		public ModifiedUlong(Func<ulong> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedUlong(ulong baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedUlong(ulong baseValue) => new ModifiedUlong(baseValue);



	}
}