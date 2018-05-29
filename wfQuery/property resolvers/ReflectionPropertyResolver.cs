using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace wfQuery
{
	/// <summary>
	/// Basic reflection property resolver.  Reflects of each type for each request.
	/// Purposely does not implement structure cache do demonstrate this ability
	///		in another resolver.
	/// </summary>
	public class ReflectionPropertyResolver : IPropertyResolver {
		
		public PropertyInfo GetPropertyInfo(Type type, string propertyName) {
			PropertyInfo result = type.GetProperty(propertyName);
			return result;
		}

		public PropertyInfo GetPropertyInfo<T>(string propertyName) {
			return GetPropertyInfo(typeof(T), propertyName);
		}
	}
}
