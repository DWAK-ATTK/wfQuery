using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace wfQuery {
	public class AjaxResponse {

		public string Data { get; set; } = string.Empty;

		public string TextStatus { get; set; } = string.Empty;

		public HttpWebResponse Response { get; set; } = null;

		public Exception Exception { get; set; } = null;
	}
}
