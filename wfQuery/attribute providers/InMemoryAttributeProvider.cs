using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wfQuery {
	public class InMemoryAttributeProvider : IAttributeProvider {

		protected Dictionary<object, Dictionary<string, object>> _attributes
			= new Dictionary<object, Dictionary<string, object>>();


		public InMemoryAttributeProvider() : base() {}



		public bool HasAttributeValue(object target, string name) {
			var attributes = _attributes
					.Where(i => i.Key == target)
					.Select(i => i.Value)
					.FirstOrDefault();
			if (null == attributes) {
				return false;
			}

			if (!attributes.ContainsKey(name)) {
				return false;
			}

			return true;
		}



		public T GetAttributeValue<T>(object target, string name) {
			var attributes = _attributes
				.Where(i => i.Key == target)
				.Select(i => i.Value)
				.FirstOrDefault();
			if (null == attributes || 0 == attributes.Count) { return default(T); }

			var attribute = attributes
				.Where(a => a.Key == name)
				.Select(a => a.Value)
				.FirstOrDefault();
			
			if (null == attribute) { return default(T); }

			return (T)attribute;
		}

		

		public void SetAttributeValue<T>(object target, string name, T value) {
			var attributes = _attributes
					.Where(i => i.Key == target)
					.Select(i => i.Value)
					.FirstOrDefault();
			if (null == attributes) {
				_attributes.Add(
					target, 
					new Dictionary<string, object>() { { name, value } }
				);
				attributes = _attributes
					.Where(i => i.Key == target)
					.Select(i => i.Value)
					.FirstOrDefault();
			} 

			if (attributes.ContainsKey(name)) {
				attributes[name] = value;
			} else {
				attributes.Add(name, value);
			}
		}



	}
}
