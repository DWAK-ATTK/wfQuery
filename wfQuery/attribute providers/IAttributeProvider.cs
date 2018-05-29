using System;
using System.Collections.Generic;
using System.Text;

namespace wfQuery {
    
	public interface IAttributeProvider {
		bool HasAttributeValue(object target, string name);
		T GetAttributeValue<T>(object target, string name);
		void SetAttributeValue<T>(object target, string name, T value);
    }
}
