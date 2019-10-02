using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace wfQuery {

	//	INFO:	Modeled after http://api.jquery.com/jquery.ajax/

	public class AjaxSettings {

		public delegate bool AjaxBeforeSendDelegate(ref HttpWebRequest request);
		public delegate string AjaxDataFilterDelegate(string rawInput, string dataType);

		protected bool? _cache = null;






		public Dictionary<string, string> Accepts { get; } = new Dictionary<string, string>();

		public AjaxBeforeSendDelegate BeforeSend { get; set; } = null;

		public bool Cache {
			get {
				if (null != _cache && _cache.HasValue) {
					return _cache.Value;
				}

				switch (DataType.Trim().ToLower()) {
					case "script":
					case "jsonp":
						return false;
				}
				return true;
			}
			set {

			}
		}

		public string ContentType { get; set; } = @"application/x-www-form-urlencoded; charset=UTF-8";

		//	TODO:	Add Converters property

		/// <summary>
		/// Accepts an object (converted to query string: prop=value); a string (in querystring format);
		/// or an array (see jQuery documentation for how these get serialized).
		/// </summary>
		public object Data { get; set; } = null;



		public AjaxDataFilterDelegate DataFilter { get; set; } = null;

		public string DataType { get; set; } = string.Empty;

		public bool Global { get; set; } = true;

		public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

		public bool IfModified { get; set; } = false;

		public bool IsLocal { get { return false; } }

		public string Method { get; set; } = "GET";

		/// <summary>
		/// Can be set using any string, or a value from System.Net.Mime.MediaTypeNames.*
		/// </summary>
		public string Mime { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public bool ProcessData { get; set; } = true;

		/// <summary>
		/// If the response contains C# or VB code (mabey js too), 
		/// it will be run immediately; DataType = "script".
		/// </summary>
		public string ScriptCharset { get; set; } = string.Empty;

		public Dictionary<int, AjaxResponseDelegate> StatusCode { get; } = new Dictionary<int, AjaxResponseDelegate>();

		public uint Timeout { get; set; } = (uint)TimeSpan.FromSeconds(30).TotalMilliseconds;

		public bool Traditional { get; set; } = false;

		public string Type { get => Method; set { Method = value; } }

		public string Url { get; set; } = string.Empty;

		public string Username { get; set; } = string.Empty;

		/// <summary>
		/// See documentation.
		/// </summary>
		public object XhrFields { get; set; } = null;
		//	TODO: xhrFields should accept any/anonymous object and do property matching against the HttpWebRequest.
	}

}
