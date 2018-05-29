using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace wfQuery {
	public class wfQueryControlSelectorResult {

		protected wfQuery _wfQuery = null;
		protected LinkedList<Control> _results = new LinkedList<Control>();

		internal wfQueryControlSelectorResult(wfQuery wfQuery) : base() {
			_wfQuery = wfQuery;

			_wfQuery.Control.ControlAdded += Control_ControlAdded;
		}

		private void Control_ControlAdded(object sender, ControlEventArgs e) {
			throw new NotImplementedException();
		}

		internal wfQueryControlSelectorResult(wfQuery wfQuery, List<Control> results) : this(wfQuery) {
			results.ForEach(c => _results.AddLast(c));
		}



		public List<object> Attr(string name) {
			return Attr<object>(name);
		}

		public List<T> Attr<T>(string name) {
			List<T> result = new List<T>();
			if (0 == Length) { return result; }

			foreach (Control control in Results) {
				//PropertyInfo property = typeof(T).GetProperty(name);
				PropertyInfo property = _wfQuery
					.GetPropertyResolver(control.GetType())
					.GetPropertyInfo(control.GetType(), name);

				if (null != property) {
					T value = (T)property.GetValue(control);
					result.Add(value);
				} else {
					//	somehow set a pseudo-property...
				}
			}

			return result;
		}

		[Obsolete]
		public List<T> Prop<T>(string name) {
			return Attr<T>(name);
		}



		public wfQueryControlSelectorResult Attr<T>(string name, T value) {
			if (0 == Length) { return this; }

			try {
				_wfQuery.Control.SuspendLayout();
				foreach (Control control in Results) {
					//control.GetType().GetProperty(name)?.SetValue(control, value);
					_wfQuery
						.GetPropertyResolver(control.GetType())
						.GetPropertyInfo(control.GetType(), name)
							?.SetValue(control, value);
				}
			} catch (Exception ex) { } finally {
				_wfQuery.Control.ResumeLayout();
			}

			return this;
		}

		/// <summary>
		/// Alias for Attr(name, value)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[Obsolete]
		public wfQueryControlSelectorResult Prop<T>(string name, T value) {
			return Attr(name, value);
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
					result.AddRange(wfQuery.QuerySelection(control, selector));
				}
				return new wfQueryControlSelectorResult(_wfQuery, result);
			}
		}


		public LinkedList<Control> Results { get { return _results; } }

		public int Length { get { return null != Results ? Results.Count : 0; } }

	}
}
