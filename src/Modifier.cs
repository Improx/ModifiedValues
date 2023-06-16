namespace ModifiedValues;

public abstract class Modifier
{
	public ModifiedValue ModifiedValue { get; private init; }
	protected int _priority = 0;
	public int Priority
	{
		get { return _priority; }
		set
		{
			_priority = value;
			ModifiedValue.SetDirty();
		}
	}
	protected int _layer = 0;
	public int Layer
	{
		get { return _layer; }
		set
		{
			_layer = value;
			ModifiedValue.SetDirty();
		}
	}
	protected int _order;
	public int Order
	{
		get { return _order; }
		set
		{
			_order = value;
			ModifiedValue.SetDirty();
		}
	}

	protected bool _compound;

	/// <summary>
	/// If we have multiple Modifiers in the same Layer with the same Priority
	/// and Order, whether the Operations should stack multiplicatively (true)
	/// or additively. This is automatically toggled when setting either the
	/// OperationCompound or OperationNonCompound function.
	/// </summary>
	/// <value></value>
	public bool Compound => _compound;

	protected Modifier(ModifiedValue modifiedValue, int priority = 0, int layer = 0, int order = 0)
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
	private Func<T, T> _operationCompound;
	public Func<T, T> OperationCompound
	{
		get { return _operationCompound; }
		set
		{
			_operationCompound = value;
			_compound = true;
			ModifiedValue.SetDirty();
		}
	}

	private Func<T, T, T> _operationNonCompound;
	public Func<T, T, T> OperationNonCompound
	{
		get { return _operationNonCompound; }
		set
		{
			_operationNonCompound = value;
			_compound = false;
			ModifiedValue.SetDirty();
		}
	}

	public Modifier(ModifiedValue modifiedValue, Func<T, T> operationCompound, int priority = 0, int layer = 0, int order = 0) : base(modifiedValue, priority, layer, order)
	{
		//TODO: ModifiedValue should be the only class allowed to call this constructor. How?
		_operationCompound = operationCompound;
		_compound = true;
	}

	public Modifier(ModifiedValue modifiedValue, Func<T, T, T> operationNonCompound, int priority = 0, int layer = 0, int order = 0) : base(modifiedValue, priority, layer, order)
	{
		//TODO: ModifiedValue should be the only class allowed to call this constructor. How?
		_operationNonCompound = operationNonCompound;
		_compound = false;
	}
}