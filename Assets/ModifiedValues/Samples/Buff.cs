namespace ModifiedValues.Samples
{
	public abstract class Buff
	{
		/// <summary>
		/// If Common is true, then the exact same Modifier objects will be applied
		/// to different Persons. This means that changing properties of one Modifier
		/// will directly affect all Persons this buff is applied to.
		/// If Common is false, each Person this buff is applied to will get their own
		/// independent Modifier objects.
		/// </summary>
		public bool Common = false;
		protected ModifierGroup _modGroup = new ModifierGroup();

		public abstract void Attach(Person person);

		public abstract void Detach(Person person);
	}

	public class BuffDivineBlessing : Buff
	{
		public override void Attach(Person person)
		{
			_modGroup += person.Speed.Add(2);
			_modGroup += person.Strength.Add(3);
			_modGroup += person.Intelligence.Add(5);
		}

		public override void Detach(Person person)
		{
			_modGroup.ClearAndDetach();
		}
	}

	public class BuffHeavyWind : Buff
	{
		public Modifier<float> SpeedMod = ModifiedFloat.TemplateAdd(5);

		public override void Attach(Person person)
		{
			person.Speed.Attach(SpeedMod);
		}

		public override void Detach(Person person)
		{
			person.Speed.Detach(SpeedMod);
		}

	}
}