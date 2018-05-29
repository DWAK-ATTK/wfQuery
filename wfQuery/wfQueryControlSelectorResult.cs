using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace wfQuery {
	public class wfQueryControlSelectorResult {

		protected wfQueryContext _wfQuery = null;
		protected LinkedList<Control> _results = new LinkedList<Control>();

		internal wfQueryControlSelectorResult(wfQueryContext wfQuery) : base() {
			_wfQuery = wfQuery;

			_wfQuery.Control.ControlAdded += Control_ControlAdded;
		}

		private void Control_ControlAdded(object sender, ControlEventArgs e) {
			throw new NotImplementedException();
		}

		internal wfQueryControlSelectorResult(wfQueryContext wfQuery, List<Control> results) : this(wfQuery) {
			results.ForEach(c => _results.AddLast(c));
		}



		public object Attr(string name) {
			return Attr<object>(name);
		}

		public T Attr<T>(string name) {
			Control control = Results.First();
			IAttributeProvider provider = wfQueryContext.DefaultAttributeProvider;
			T value = default(T);

			if (provider.HasAttributeValue(control, name)) {
				value = provider.GetAttributeValue<T>(control, name);
			} else {
				value = GetPropertyValue<T>(control, name);
			}
			return value;
		}

		/// <summary>
		/// Return the property value specified by <para>name</para>.
		/// If no property by that name is found, then return the attribute by that name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">Name of the property to query.</param>
		/// <returns>Value of the requested property.</returns>
		public T Prop<T>(string name) {
			T result = default(T);
			if (0 == Length) { return result; }

			Control control = Results.First();
			PropertyInfo property = GetProperty(control, name);

			if (null != property) {
				T value = (T)property.GetValue(control);
				result = value;
			} else {
				return Attr<T>(name);
			}

			return result;
		}



		internal PropertyInfo GetProperty(Control control, string name) {
			PropertyInfo property = _wfQuery
				.GetPropertyResolver(control.GetType())
				.GetPropertyInfo(control.GetType(), name);

			return property;
		}



		internal T GetPropertyValue<T>(Control control, string name) {
			T result = default(T);
			PropertyInfo property = GetProperty(control, name);
			if (null != property) {
				result = (T)property.GetValue(control);
			}
			return result;
		}



		public wfQueryControlSelectorResult Attr<T>(string name, T value) {
			if (0 == Length) { return this; }
			IAttributeProvider provider = wfQueryContext.DefaultAttributeProvider;

			_wfQuery.Control.SuspendLayout();
			foreach (Control control in Results) {
				PropertyInfo property = GetProperty(control, name);
				if (null == property) {
					provider.SetAttributeValue(control, name, value);
				} else {
					property.SetValue(control, value);
				}
			}
			_wfQuery.Control.ResumeLayout();

			return this;
		}

		public wfQueryControlSelectorResult Attr<T>(string name, Func<int, Control, T> func) {
			if (0 == Length) { return this; }
			IAttributeProvider provider = wfQueryContext.DefaultAttributeProvider;
			LinkedListNode<Control> node = Results.First;

			_wfQuery.Control.SuspendLayout();
			for (int index = 0; index < Length; index++) {
				Control control = node.Value;
				PropertyInfo property = GetProperty(control, name);
				
				if (null == property) {
					provider.SetAttributeValue(control, name, func(index, control));
				} else {
					property.SetValue(control, func(index, control));
				}
				node = node.Next;
			}
			_wfQuery.Control.ResumeLayout();

			return this;
		}

		/// <summary>
		/// Sets the property value.  If property can not be found, sets the attribute value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public wfQueryControlSelectorResult Prop<T>(string name, T value) {
			if (0 == Length) { return this; }

			_wfQuery.Control.SuspendLayout();
			foreach (Control control in Results) {
				PropertyInfo property = GetProperty(control, name);
				if (null != property) {
					property.SetValue(control, value);
				} else {
					Attr(name, value);
				}
			}
			_wfQuery.Control.ResumeLayout();

			return this;
		}



		public wfQueryControlSelectorResult Each(Action<int, Control> action) {
			if (0 == Length) { return this; }

			try {
				int index = 0;
				LinkedListNode<Control> node = _results.First;
				_wfQuery.Control.SuspendLayout();
				while (null != node) {
					action.Invoke(index, node.Value);
					node = node.Next;
					index++;
				}
			} catch (Exception ex) {
			} finally {
				_wfQuery.Control.ResumeLayout();
			}
			return this;
		}



		public wfQueryControlSelectorResult Not(string query) {
			List<Control> result = Results.ToArray().ToList();
			wfQueryControlSelectorResult removals = this[query];

			removals.Each((i, c) => result.Remove(c));

			return new wfQueryControlSelectorResult(_wfQuery, result);
		}



		//private List<EventHandler> _onActions = new List<EventHandler>();
		public wfQueryControlSelectorResult On(string eventName, EventHandler action) {
			//_onActions.Add(action);
			if (0 == Length) { return this; }
			foreach (Control item in Results) {
				typeof(Control).GetEvent(eventName).AddEventHandler(item, action);
			}
			return this;
		}



		public wfQueryControlSelectorResult Off(string eventName, EventHandler action) {
			//_onActions.Add(action);
			if (0 == Length) { return this; }
			foreach (Control item in Results) {
				typeof(Control).GetEvent(eventName).RemoveEventHandler(item, action);
			}
			return this;
		}



		public wfQueryControlSelectorResult this[string selector] {
			get {
				if (0 == Length) { return this; }

				List<Control> result = new List<Control>();
				foreach (Control control in Results) {
					result.AddRange(wfQueryContext.QuerySelection(control, selector));
				}
				return new wfQueryControlSelectorResult(_wfQuery, result);
			}
		}


		public LinkedList<Control> Results { get { return _results; } }

		public int Length { get { return null != Results ? Results.Count : 0; } }

	}
}
