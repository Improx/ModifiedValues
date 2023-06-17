namespace ModifiedValues;

public abstract class Modifier
{
	public event EventHandler<EventArgs> Changed;
	public event EventHandler<EventArgs> DetachingFromAll;
	public event EventHandler<ProbingAttachedModValuesEventArgs> ProbingAttachedModValues;
	protected bool _active = true;
	public bool Active
	{
		get { return _active; }
		set
		{
			_active = value;
			OnChanged();
		}
	}
	protected int _priority = 0;
	public int Priority
	{
		get { return _priority; }
		set
		{
			_priority = value;
			OnChanged();
		}
	}
	protected int _layer = 0;
	public int Layer
	{
		get { return _layer; }
		set
		{
			_layer = value;
			OnChanged();
		}
	}
	protected int _order;
	public int Order
	{
		get { return _order; }
		set
		{
			_order = value;
			OnChanged();
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

	protected Modifier(int priority = 0, int layer = 0, int order = 0)
	{
		_priority = priority;
		_layer = layer;
		_order = order;
	}

	public void DetachFrom(ModifiedValue modValue)
	{
		modValue.Detach(this);
	}

	/// <summary>
	/// Detaches this modifier from all ModifiedValues that it was applied to.
	/// </summary>
	public void DetachFromAll()
	{
		DetachingFromAll?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnChanged()
	{
		Changed?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Returns a list of all ModifiedValues that this Modifier is attached to.
	/// </summary>
	/// <returns></returns>
	public List<ModifiedValue> GetAttachedModValues()
	{
		var eventArgs = new ProbingAttachedModValuesEventArgs();
		ProbingAttachedModValues?.Invoke(this, eventArgs);
		return eventArgs.ModValues;
	}

	public class ProbingAttachedModValuesEventArgs : EventArgs
	{
		public List<ModifiedValue> ModValues = new List<ModifiedValue>();
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
			OnChanged();
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
			OnChanged();
		}
	}

	public Modifier(Func<T, T> operationCompound, int priority = 0, int layer = 0, int order = 0) : base(priority, layer, order)
	{
		_operationCompound = operationCompound;
		_compound = true;
	}

	public Modifier(Func<T, T, T> operationNonCompound, int priority = 0, int layer = 0, int order = 0) : base(priority, layer, order)
	{
		_operationNonCompound = operationNonCompound;
		_compound = false;
	}

}