using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace wfQuery {
	public partial class wfQueryControlSelectorResult {

		public wfQueryControlSelectorResult Hide() {
			foreach(Control control in Results) {
				control.Visible = false;
			}
			return this;
		}



		public wfQueryControlSelectorResult Show() {
			foreach (Control control in Results) {
				control.Visible = true;
			}
			return this;
		}



		public wfQueryControlSelectorResult Toggle() {
			foreach (Control control in Results) {
				control.Visible = !control.Visible;
			}
			return this;
		}

	}
}
