namespace ModifiedValues;

public class Modifier
{
	public ModifiedValue ModifiedValue { get; private init; }
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
	private int _order;
	public int Order
	{
		get { return _order; }
		set
		{
			_order = value;
			ModifiedValue.SetDirty();
		}
	}

	protected Modifier(ModifiedValue modifiedValue, int priority, int layer, int order)
	{
		ModifiedValue = modifiedValue;
		_priority = priority;
		_layer = layer;
		_order = order;
	}

	public void Remove()
	{
		ModifiedValue.RemoveModifier(this);
	}
}

public class Modifier<T> : Modifier
{
	private Func<T, T> _operation;
	public Func<T, T> Operation
	{
		get { return _operation; }
		set
		{
			_operation = value;
			ModifiedValue.SetDirty();
		}
	}

	public Modifier(ModifiedValue modifiedValue, Func<T, T> operation, int priority, int layer, int order) : base(modifiedValue, priority, layer, order)
	{
		//TODO: ModifiedValue should be the only class allowed to call this constructor
		_operation = operation;
	}
}