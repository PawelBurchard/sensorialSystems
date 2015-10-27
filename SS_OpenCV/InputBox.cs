using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public InputBox(string _title)
        {
            InitializeComponent();

            this.Text = _title;

        }

        double value;

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValueTextBox.Text != "")
            {
                value = Convert.ToDouble(ValueTextBox.Text);
            }
        }

        public double getValue
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}