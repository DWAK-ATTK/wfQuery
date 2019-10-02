using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wfQuery {

	public delegate void AjaxDoneDelegate(string data, string textStatus, HttpWebResponse response);
	public delegate void AjaxFailDelegate(HttpWebResponse response, string textStatus, Exception exception);
	public delegate void AjaxResponseDelegate(AjaxResponse response);

	//	TODO: Create a surrogate for XMLHttpRequest.
	//		See last third of http://api.jquery.com/jquery.ajax/ for starting point.

	public partial class wfQueryContext_ajax {

		public void Ajax(string url, AjaxSettings settings) {

		}

		public async Task AjaxAsync(string url, AjaxSettings settings) {
			
		}

	}


}
