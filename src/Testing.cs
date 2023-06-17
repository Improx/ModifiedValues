using ModifiedValues.Samples;

public class Program
{
	public static Person? Person;
	public static void Main()
	{
		Person = new Person();
		Report("Created Person");

		if (Person.Speed == 3)
		{
			Console.WriteLine("This condition should pass. Speed value was 3.");
			Console.WriteLine("Comparisons with floats work because of implicit casting");
		}

		//Modifiers:
		var modRollerScates = Person.Speed.Add(10);
		Report("Put on rollerscates. Added 10 speed.");

		var modHeadWind = Person.Speed.Mul(0.7f);
		Report("Headwind is slowing us down to 70% speed.");

		var modCustom = Person.Speed.Modify(CustomOperation, order : 999999);
		Report("Custom operation. If speed is below 2, slow down to 0. Else, set speed to 5.");

		modCustom.RemoveFromAll();
		Report("Removed custom operation.");

		modHeadWind.RemoveFromAll();
		Report("Wind ended, phew! Person is back to full speed.");

		var modEnergyDrink1 = Person.Speed.AddFraction(0.2f);
		Report("Consumed an energy drink! Ground speed increased by 20% additively. Note that this this fraction increase is in relation to the base value.");

		var modEnergyDrink2 = Person.Speed.AddFraction(0.2f);
		Report("Consumed an energy drink! Ground speed increased by 20% additively.Note that this this fraction increase is in relation to the base value.");

		var modSpeedLimit = Person.Speed.MaxCap(10);
		Report("Police is watching. Have to slow down to a speed of 10.");

		var modWings = Person.Speed.Add(500, priority : 1);
		Report("We got wings, increasing our speed by 500. We are no longer on the ground, so previous mods have no effect anymore. This mod's Priority is higher than the others', so it is applied directly to the base value and ignores other mods.");

		var modMotivation = Person.Speed.Mul(1.2f, layer : 1);
		Report("You feel very motivated, increasing all speed by 20% multiplicatively. This effects acts on top of all old ones, because it is on a higher layer.");

		var modMegaEnergyDrink1 = Person.Speed.AddFraction(0.5f, layer : 2);
		Report("You drink a Mega energy drink, increasing all speed by 50% additively. This effects acts on top of all old ones, because it is on a higher layer.");

		var modMegaEnergyDrink2 = Person.Speed.AddFraction(0.5f, layer : 2);
		Report("You drink a Mega energy drink, increasing all speed by 50% additively. This effects acts on top of all old ones, because it is on a higher layer.");

		//This creates a totally new ModifiedFloat with base value 100
		//and forgetting all previous Modifiers:
		Person.Speed = 100;
		Report("It's a new day, all old effects are gone and we also have brand-new legs with a fast base walking speed of 100.");

		var modCoffee1 = Person.Speed.Mul(1.2f);
		Report("Drank a nice cup of coffee. Speed increased by 20% multiplicatively.");

		var modCoffee2 = Person.Speed.Mul(1.2f);
		Report("Drank a nice cup of coffee. Speed increased by 20% multiplicatively.");

		Person.Speed.BaseValue = 80;
		Report("Legs got tired, affecting the fundamentals of how fast we are. Base speed dropped from 100 to 80, but the coffee cups are still in effect.");
	}

	private static float CustomOperation(float prevValue)
	{
		if (prevValue < 2)
		{
			//Come to a halt, set speed to 0:
			return 0;
		}
		else
		{
			//Else, set speed to exactly 11:
			return 5;
		}
	}

	public static void Report(string message)
	{
		Console.WriteLine("----------------------------");
		Console.WriteLine(message);
		Console.WriteLine("Current speed: " + Person.Speed);
	}
}