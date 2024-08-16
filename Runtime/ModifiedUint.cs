using System;

namespace ModifiedValues
{
	[Serializable]
	public class ModifiedUint : ModifiedValue<uint>
	{

		public ModifiedUint(Func<uint> baseValueGetter, bool updateEveryTime = false) : base(baseValueGetter, updateEveryTime) { }

		public ModifiedUint(uint baseValue, bool updateEveryTime = false) : base(baseValue, updateEveryTime) { }

		public static implicit operator ModifiedUint(uint baseValue) => new ModifiedUint(baseValue);



	}
}