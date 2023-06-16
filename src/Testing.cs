using ModifiedValues;
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

		//var modCustom = Person.Speed.Modify(CustomOperation);
		//Report("Custom operation.");

		modHeadWind.Remove();
		Report("Wind ended, phew! Person is back to full speed.");

		var modEnergyDrink1 = Person.Speed.AddFraction(100);
		Report("Consumed an energy drink! Speed increased 100-fold additively.");

		var modEnergyDrink2 = Person.Speed.AddFraction(100);
		Report("Consumed an energy drink! Speed increased 100-fold additively.");

		var modSpeedLimit = Person.Speed.MaxCap(20);
		Report("Police is watching. Have to slow down to a speed of 20.");

		//This creates a totally new ModifiedFloat with base value 80
		//and forgetting all previous Modifiers:
		Person.Speed = 100;
		Report("It's a new day, all old effects are gone and we also have brand-new legs with a fast base walking speed of 100.");

		// var modCoffee1 = Person.Speed.Mul(1.2f);
		// Report("Drank a nice cup of coffee. Speed increased by 20% multiplicatively.");

		// var modCoffee2 = Person.Speed.Mul(1.2f);
		// Report("Drank a nice cup of coffee. Speed increased by 20% multiplicatively.");

		var modTee1 = Person.Speed.AddFraction(0.2f);
		Report("Drank a nice cup of tea. Speed increased by 20% additively.");

		var modTee2 = Person.Speed.AddFraction(0.2f);
		Report("Drank a nice cup of tea. Speed increased by 20% additively.");

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
			return 11;
		}
	}

	public static void Report(string message)
	{
		Console.WriteLine("----------------------------");
		Console.WriteLine(message);
		Console.WriteLine("Current speed: " + Person.Speed);
	}
}