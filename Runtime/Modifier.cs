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
		private Func<T, T, T, T> _operation;

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

		public Modifier(Func<T, T, T, T> operation, int priority = 0, int layer = 0, int order = 0) : base(priority, layer, order)
		{
			_operation = operation;
		}

		/// <summary>
		/// Returns a copy object of this modifier.
		/// Everything about it will be the same, except it won't be attached to anything.
		/// </summary>
		/// <returns></returns>
		public Modifier<T> Copy()
		{
			Modifier<T> mod = new Modifier<T>(Operation, Priority, Layer, Order);
			mod.Active = Active;
			return mod;
		}

	}
}