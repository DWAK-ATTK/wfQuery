using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace wfQuery.ajax {

	public class AjaxPromise {



		protected List<AjaxDoneDelegate> _done = new List<AjaxDoneDelegate>();
		protected List<AjaxFailDelegate> _fail = new List<AjaxFailDelegate>();

		protected List<AjaxResponseDelegate> _doneResponse = new List<AjaxResponseDelegate>();
		protected List<AjaxResponseDelegate> _failResponse = new List<AjaxResponseDelegate>();
		protected List<AjaxResponseDelegate> _alwaysResponse = new List<AjaxResponseDelegate>();

		protected string _url = string.Empty;
		protected AjaxSettings _settings = null;



		private AjaxPromise() : base() { }

		public AjaxPromise(string url, AjaxSettings settings) : this() {
			_url = url;
			_settings = settings;
		}



		public void Execute() {
			if (string.IsNullOrEmpty(_url)) { throw new ArgumentNullException("url", "url can not be null."); }

			string content = string.Empty;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);

			try {
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
					//	TODO:	map the settings
					//			then execute the request
					_done?.ForEach(a => {
						a.Invoke(content, response.StatusDescription, response);
					});
					_doneResponse?.ForEach(a => {
						a.Invoke(new AjaxResponse() {
							Data = content,
							Response = response,
							TextStatus = response.StatusDescription
						});
					});
					_alwaysResponse?.ForEach(a => {
						a.Invoke(new AjaxResponse() {
							Data = content,
							Response = response,
							TextStatus = response.StatusDescription
						});
					});
				}
			} catch (Exception ex) {
				_fail?.ForEach(a => {
					a.Invoke(null, $"ERROR: {ex.Message}", ex);
				});
				_failResponse?.ForEach(a => {
					a.Invoke(new AjaxResponse() {
						TextStatus = $"ERROR: {ex.Message}",
						Exception = ex
					});
				});
				_alwaysResponse?.ForEach(a => {
					a.Invoke(new AjaxResponse() {
						TextStatus = $"ERROR: {ex.Message}",
						Exception = ex
					});
				});
			}



		}

		public void ExecuteAsync() {
			throw new NotImplementedException();
		}



		protected void ApplySettings(ref HttpWebRequest request) {
			if (null == _settings) { return; }


		}



		public AjaxPromise Done(AjaxDoneDelegate func) {
			_done.Add(func);
			return this;
		}

		public AjaxPromise Done(AjaxResponseDelegate func) {
			_doneResponse.Add(func);
			return this;
		}



		public AjaxPromise Fail(AjaxFailDelegate func) {
			_fail.Add(func);
			return this;
		}

		public AjaxPromise Fail(AjaxResponseDelegate func) {
			_failResponse.Add(func);
			return this;
		}



		public AjaxPromise Always(AjaxResponseDelegate func) {
			_alwaysResponse.Add(func);
			return this;
		}

	}

}
