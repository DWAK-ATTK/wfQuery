using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace wfQuery {
	public class wfQueryContext {

		internal event EventHandler<ControlEventArgs> ControlAdded;
		internal event EventHandler<ControlEventArgs> ControlRemoved;

		public static IPropertyResolver DefaultPropertyResolver { get; set; } = new ReflectionPropertyResolver();
		public static IAttributeProvider DefaultAttributeProvider { get; set; } = new InMemoryAttributeProvider();

		protected Control _control = null;
		protected Dictionary<Type, IPropertyResolver> _propertyResolvers = new Dictionary<Type, IPropertyResolver>();



		public wfQueryContext(Control control) : base() {
			_control = control ?? throw new NullReferenceException("control can not be null.");

			HookupControlEvents(_control);
		}



		private void HookupControlEvents(Control parent) {
			parent.ControlAdded += _control_ControlAdded;
			parent.ControlRemoved += _control_ControlRemoved;

			foreach (Control control in parent.Controls) {
				HookupControlEvents(control);
			}
		}



		private void UnhookControlEvents(Control parent) {
			parent.ControlAdded -= _control_ControlAdded;
			parent.ControlRemoved -= _control_ControlRemoved;

			foreach (Control control in parent.Controls) {
				UnhookControlEvents(control);
			}
		}



		private void _control_ControlRemoved(object sender, ControlEventArgs e) {
			ControlRemoved?.Invoke(_control, e);
			UnhookControlEvents(e.Control);
		}

		private void _control_ControlAdded(object sender, ControlEventArgs e) {
			HookupControlEvents(e.Control);
			ControlAdded?.Invoke(_control, e);

		}



		public void AddPropertyResolver(Type type, IPropertyResolver propertyResolver) {
			if (!_propertyResolvers.ContainsKey(type)) {
				_propertyResolvers.Add(type, propertyResolver);
			} else {
				_propertyResolvers[type] = propertyResolver;
			}
		}

		public void AddPropertyResolver<T>(IPropertyResolver propertyResolver) {
			AddPropertyResolver(typeof(T), propertyResolver);
		}



		public IPropertyResolver GetPropertyResolver(Type type) {
			if (_propertyResolvers.ContainsKey(type)) {
				return _propertyResolvers[type];
			}

			return DefaultPropertyResolver;
		}

		public IPropertyResolver GetPropertyResolver<T>() {
			return GetPropertyResolver(typeof(T));
		}



		private wfQueryControlSelectorResult QuerySelection(string query) {
			List<Control> controls = QuerySelection(_control, query);
			return CreateControlSelectorResult(controls);
		}

		internal static List<Control> QuerySelection(Control parent, string query) {
			string[] selectors = query.Split(new char[] { ',' }); //	TODO: Replace this with output from a real query parser
			selectors = selectors.Select(s => s.Trim()).ToArray();
			return QuerySelection(parent, selectors);
		}

		private static List<Control> QuerySelection(Control parent, string[] selectors) {

			List<Control> controls = new List<Control>();

			if (0 == selectors.Length) { return controls; }
			if (null == parent) { return controls; }

			List<Control> targets = new List<Control>();
			targets.Add(parent);
			foreach (Control control in parent.Controls) { targets.Add(control); }

			if (selectors.Contains("*") && 1 < targets.Count) {
				for (int index = 1; index < targets.Count; index++) {
					controls.Add(targets[index]);
					controls.AddRange(
						QuerySelection(targets[index], new string[] { "*" })
					);
				}
				return controls;
			}

			for (int index = 0; index < selectors.Length; index++) {
				string selector = selectors[index].Trim();
				if (string.IsNullOrEmpty(selector)) { continue; }
				string selectorType = selector.Substring(0, 1);
				string selectorValue = selector.Substring(1);

				foreach (Control control in targets) {
					switch (selectorType) {
						case "#":
							//	CONTROL NAME
							if (control.Name == selectorValue) {
								controls.Add(control);
							}
							break;

						case ".":
							if (control.GetType().Name == selectorValue) {
								controls.Add(control);
							}
							break;
					}

					if ((control != targets[0]) && (0 < control.Controls.Count)) {
						controls.AddRange(
							QuerySelection(control, new string[] { selector })
						);
					}
				}
			}

			return controls;
		}



		internal wfQueryControlSelectorResult CreateControlSelectorResult() {
			return new wfQueryControlSelectorResult(this);
		}
		internal wfQueryControlSelectorResult CreateControlSelectorResult(List<Control> controls) {
			return new wfQueryControlSelectorResult(this, controls);
		}



		public wfQueryControlSelectorResult this[string selector] {
			get {
				return QuerySelection(selector);
			}
		}

		public wfQueryControlSelectorResult this[Control control] {
			get {
				return new wfQueryControlSelectorResult(this, new List<Control>() { control });
			}
		}



		public Control Control {
			get { return _control; }
		}



		public string Version {
			get {
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

	}
}
