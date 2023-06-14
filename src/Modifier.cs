namespace ModifiedValues;

public class Modifier
{
	public ModifiedValue ModifiedValue { get; private set; }
	private int _priority = 0;
	public int Priority
	{
		get { return _priority; }
		set
		{
			_priority = value;
			ModifiedValue.SetDirty();
		}
	}
	private int _layer = 0;
	public int Layer
	{
		get { return _layer; }
		set
		{
			_layer = value;
			ModifiedValue.SetDirty();
		}
	}
	private int _operationOrder;
	public int OperationOrder
	{
		get { return _operationOrder; }
		set
		{
			_operationOrder = value;
			ModifiedValue.SetDirty();
		}
	}

	protected Modifier(ModifiedValue modifiedValue, int priority, int layer, int operationOrder)
	{
		ModifiedValue = modifiedValue;
		_priority = priority;
		_layer = layer;
		_operationOrder = operationOrder;
	}

	public void Remove()
	{
		//TODO: Actually remove it from ModifiedValue
		ModifiedValue.SetDirty();
	}
}

public class Modifier<TValue, TModifier> : Modifier
{
	private Func<TValue, TValue, TModifier> _operation;
	public Func<TValue, TValue, TModifier> Operation
	{
		get { return _operation; }
		set
		{
			_operation = value;
			ModifiedValue.SetDirty();
		}
	}

	public Modifier(ModifiedValue modifiedValue, Func<TValue, TValue, TModifier> operation, int priority, int layer, int operationOrder) : base(modifiedValue, priority, layer, operationOrder)
	{
		_operation = operation;
	}
}