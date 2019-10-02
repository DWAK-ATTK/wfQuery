using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace wfQuery {
	public partial class wfQueryContext {

		public T ParseJson<T>(string json) {
			T result = JsonConvert.DeserializeObject<T>(json);
			return result;
		}

	}
}
