using System;
using System.Collections.Generic;

namespace ModifiedValues
{
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
		/// <summary>
		/// Default operation is just returning the base value.
		/// In case constructing a new Modifier<T> without specifying an operation
		/// right away.
		/// </summary>
		/// <returns></returns>
		private Func<T, T, T, T> _operation = (baseValue, _, _) => baseValue;

		/// <summary>
		/// Input values are: baseValue, layerStartValue, latestValue
		/// </summary>
		/// <value></value>
		public Func<T, T, T, T> Operation
		{
			get { return _operation; }
			set
			{
				_operation = value;
				OnChanged();
			}
		}

		private T _amount;

		public T Amount
		{
			get { return _amount; }
			set
			{
				_amount = value;
				OnChanged();
			}
		}

		/// <summary>
		/// Keeping private, to have all public constructors static, for consitency
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="priority"></param>
		/// <param name="layer"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		private Modifier(Func<T, T, T, T> operation, int priority = 0, int layer = 0, int order = 0) : base(priority, layer, order)
		{
			_operation = operation;
		}

		public static Modifier<T> New(Func<T, T, T, T> operation, int priority = 0, int layer = 0, int order = 0)
		{
			return new Modifier<T>(operation, priority, layer, order);
		}

		public static Modifier<T> NewFromBase(Func<T, T> operationFromBase, int priority = 0, int layer = 0, int order = 0)
		{
			return New((baseValue, _, _) => operationFromBase(baseValue), priority, layer, order);
		}

		public static Modifier<T> NewFromLayerStart(Func<T, T> operationFromLayerStart, int priority = 0, int layer = 0, int order = 0)
		{
			return New((_, layerStartValue, _) => operationFromLayerStart(layerStartValue), priority, layer, order);
		}

		public static Modifier<T> NewFromLatest(Func<T, T> operationFromLatest, int priority = 0, int layer = 0, int order = 0)
		{
			return New((_, _, latestValue) => operationFromLatest(latestValue), priority, layer, order);
		}

		public static Modifier<T> NewFromBaseAndLayerStart(Func<T, T, T> operationFromBaseAndLayerStart, int priority = 0, int layer = 0, int order = 0)
		{
			return New((baseValue, layerStartValue, _) => operationFromBaseAndLayerStart(baseValue, layerStartValue), priority, layer, order);
		}

		public static Modifier<T> NewFromBaseAndLatest(Func<T, T, T> operationFromBaseAndLatest, int priority = 0, int layer = 0, int order = 0)
		{
			return New((baseValue, _, latestValue) => operationFromBaseAndLatest(baseValue, latestValue), priority, layer, order);
		}

		public static Modifier<T> NewFromLayerStartAndLatest(Func<T, T, T> operationFromLayerStartAndLatest, int priority = 0, int layer = 0, int order = 0)
		{
			return New((_, layerStartValue, latestValue) => operationFromLayerStartAndLatest(layerStartValue, latestValue), priority, layer, order);
		}
		public static Modifier<T> NewFromIgnored(Func<T> operationFromIgnored, int priority = 0, int layer = 0, int order = 0)
		{
			return New((_, _, _) => operationFromIgnored(), priority, layer, order);
		}

		public void SetOperationFromBase(Func<T, T> operationFromBase) => Operation = (baseValue, _, _) => operationFromBase(baseValue);
		public void SetOperationFromLayerStart(Func<T, T> operationFromLayerStart) => Operation = (_, layerStartValue, _) => operationFromLayerStart(layerStartValue);
		public void SetOperationFromLatest(Func<T, T> operationFromLatest) => Operation = (_, _, latestValue) => operationFromLatest(latestValue);
		public void SetOperationFromBaseAndLayerStart(Func<T, T, T> operationFromBaseAndLayerStart) => Operation = (baseValue, layerStartValue, _) => operationFromBaseAndLayerStart(baseValue, layerStartValue);
		public void SetOperationFromBaseAndLatest(Func<T, T, T> operationFromBaseAndLatest) => Operation = (baseValue, _, latestValue) => operationFromBaseAndLatest(baseValue, latestValue);
		public void SetOperationFromLayerStartAndLatest(Func<T, T, T> operationFromLayerStartAndLatest) => Operation = (_, layerStartValue, latestValue) => operationFromLayerStartAndLatest(layerStartValue, latestValue);
		public void SetOperationFromIgnored(Func<T> operationFromIgnored) => Operation = (_, _, _) => operationFromIgnored();

		/// <summary>
		/// Returns a copy object of this modifier.
		/// Everything about it will be the same, except it won't be attached to anything.
		/// </summary>
		/// <returns></returns>
		public Modifier<T> Copy()
		{
			Modifier<T> mod = new Modifier<T>(Operation, Priority, Layer, Order)
			{
				Active = Active
			};
			return mod;
		}

	}
}