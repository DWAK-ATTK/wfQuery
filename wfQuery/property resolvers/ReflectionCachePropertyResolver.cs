using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace wfQuery
{
	/// <summary>
	/// Caching reflection property resolver.  If the type does not exist in the cache
	///		then it is added.  Returns the PropertyInfo from the cache.
	///	This method uses more memory, but provides a speedier result.
	/// </summary>
	public class ReflectionCachePropertyResolver : IPropertyResolver {

		protected Dictionary<Type, Dictionary<string, PropertyInfo>> _structureCache
			= new Dictionary<Type, Dictionary<string, PropertyInfo>>();



		public PropertyInfo GetPropertyInfo(Type type, string propertyName) {
			if(!_structureCache.ContainsKey(type)) { CacheStructure(type); }
			if (!_structureCache[type].ContainsKey(propertyName)) { return null; }
			
			return _structureCache[type][propertyName];
		}

		public PropertyInfo GetPropertyInfo<T>(string propertyName) {
			return GetPropertyInfo(typeof(T), propertyName);
		}



		public virtual void CacheStructure(Type type) {
			if (_structureCache.ContainsKey(type)) { return; }

			Dictionary<string, PropertyInfo> structure = new Dictionary<string, PropertyInfo>();
			foreach (PropertyInfo property in type.GetProperties()) {
				structure.Add(property.Name, property);
			}
			_structureCache.Add(type, structure);
		}

		public virtual void CacheStructure<T>() {
			CacheStructure(typeof(T));
		}


	}
}
