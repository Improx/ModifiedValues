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

	private bool _compound;

	/// <summary>
	/// If we have multiple Modifiers in the same Layer with the same Priority
	/// and Order, whether the Operations should use the previous Operation's output or
	/// the calculated value before applying these Operations.
	/// One way to think about this: Compound = true means multiplicative stacking and
	/// Compound = false means additive stacking.
	/// </summary>
	/// <value></value>
	public bool Compound
	{
		get { return _compound; }
		set
		{
			Compound = value;
			ModifiedValue.SetDirty();
		}
	}

	protected Modifier(ModifiedValue modifiedValue, int priority = 0, int layer = 0, int order = 0, bool compound = false)
	{
		ModifiedValue = modifiedValue;
		_priority = priority;
		_layer = layer;
		_order = order;
		_compound = compound;
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

	public Modifier(ModifiedValue modifiedValue, Func<T, T> operation, int priority = 0, int layer = 0, int order = 0, bool compound = false) : base(modifiedValue, priority, layer, order, compound)
	{
		//TODO: ModifiedValue should be the only class allowed to call this constructor
		_operation = operation;
	}
}