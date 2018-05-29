using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace wfQuery
{
    public interface IPropertyResolver {
		PropertyInfo GetPropertyInfo(Type type, string propertyName);

		PropertyInfo GetPropertyInfo<T>(string propertyName);
	}
}
