using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wfQuery;


namespace wfQuery_Test_harness {
	public partial class Form1 : Form {

		wfQueryContext _ = null;

		public Form1() {
			InitializeComponent();
			_ = new wfQueryContext(this);
			ReflectionCachePropertyResolver resolver = new ReflectionCachePropertyResolver();
			resolver.CacheStructure<CheckBox>();
			resolver.CacheStructure<TextBox>();
			resolver.CacheStructure<Button>();
			wfQueryContext.DefaultPropertyResolver = resolver;
		}

		private void QueryTestRedButton_Click(object sender, EventArgs e) {
			string query = QueryTextBox.Text;
			wfQueryControlSelectorResult result = null;

			result = _[query];
			result.Attr("BackColor", Color.Red);
			UpdateCountLabel(result);

			result = _[".CheckBox"]["#checkBox2"];
			result.Attr("ForeColor", Color.White);
			UpdateCountLabel(result);
		}



		private void UpdateCountLabel(wfQueryControlSelectorResult results) {
			QueryElementCountLabel.Text = results?.Length.ToString() ?? "0";
		}

		private void SetIntAttrButton_Click(object sender, EventArgs e) {
			string query = QueryTextBox.Text;
			string attrName = AttrNameTextBox.Text;
			int value = int.Parse(AttrValueTextBox.Text.Trim());
			wfQueryControlSelectorResult result = _[query];

			result.Attr(attrName, value);

			//	If this were not interactive.. you could do this..
			//	example: set all ProgressBar's .Value to 25
			//		_[".ProgressBar"].Attr("Value", 25);
		}

		private void SetStringAttrButton_Click(object sender, EventArgs e) {
			string query = QueryTextBox.Text;
			string attrName = AttrNameTextBox.Text;
			string value = AttrValueTextBox.Text;
			wfQueryControlSelectorResult result = _[query];

			result.Attr(attrName, value);
		}

		private void EachButton_Click(object sender, EventArgs e) {
			int count = _[".CheckBox, .TextBox, .Label"]
				.Not("#checkBox2")
				.Each((i, c) => {
					c.Text = $"Control {i}";
					if (c.Parent != _.Control) {
						c.Parent.BackColor = Color.SkyBlue;
					}
				}).Length;
			QueryElementCountLabel.Text = count.ToString();
		}

		private void AddControlButton_Click(object sender, EventArgs e) {
			Button button = new Button() {
				Text = "NEW BUTTON",
				Width = 100, Height = 23,
				Top = 25, Left = 10,
				Visible = true
			};
			DynamicControlPanel.Controls.Add(button);
		}

		private void RemoveControlButton_Click(object sender, EventArgs e) {
			DynamicControlPanel.Controls.Clear();
		}

		private void OnClickButton_Click(object sender, EventArgs e) {
			_[".TextBox"]
			.Attr("BackColor", Color.SkyBlue)
			.On("Click", new EventHandler(clickHandler));

			_[".Button"]
			.On("Click", (s, args) => {
				((Control)s).BackColor = Color.Green;
			});
		}


		private void clickHandler(object sender, EventArgs e) {
			//((Control)sender).BackColor = SystemColors.Control;
			_[(Control)sender].Attr("BackColor", SystemColors.Window);
		}

		private void OffClickButton_Click(object sender, EventArgs e) {
			_[".TextBox"].Off("Click", new EventHandler(clickHandler));
		}



		private void button2_Click(object sender, EventArgs e) {
			_[".TextBox"].Attr("is-alaska", (i,c) => {
				return i % 2 == 0;
			});
			string result = string.Empty;
			_[".TextBox"].Each((i, c) => 
				{
					result += i.ToString()
					+ ": " + _[c].Attr("is-alaska").ToString()
					+ "\\" + c.Name + "\r\n";
				});
			MessageBox.Show(result);
		}


	}
}
