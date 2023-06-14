namespace ModifiedValues;

public class ModifiedFloat : ModifiedValue<float>
{
	public ModifiedFloat(float baseValue) : base(baseValue) { }

	public Modifier<float, float> Add(float amount, int priority = 0, int layer = 0)
	{
		Modifier<float, float> mod = new Modifier<float, float>(this, (prevValue, modAmount) => prevValue + modAmount, priority, layer, 0);
		//TODO: Actually add the modifier
		return mod;
	}

}