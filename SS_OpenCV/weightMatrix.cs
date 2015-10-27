using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class weightMatrix : Form
    {
        private string fType = "Mean 3x3";
        private int[] weightMtx = new int[9];
        private double weight1;
        private bool cancel = false;

        public weightMatrix()
        {
            InitializeComponent();
            filterType.Text = fType;
            for (int i = 0; i < 9; i++)
            {
                weightMtx[i] = 1;
            }
            weight1 = 9;
            filterType.Items.AddRange(new object[] { "Mean 3x3", "Gaussian 3x3", "Laplacian Hard 3x3", "Mean Remove 3x3" });
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            weightMtx[0] = Convert.ToInt32(matrix11.Text);
            weightMtx[1] = Convert.ToInt32(matrix12.Text);
            weightMtx[2] = Convert.ToInt32(matrix13.Text);
            weightMtx[3] = Convert.ToInt32(matrix21.Text);
            weightMtx[4] = Convert.ToInt32(matrix22.Text);
            weightMtx[5] = Convert.ToInt32(matrix23.Text);
            weightMtx[6] = Convert.ToInt32(matrix31.Text);
            weightMtx[7] = Convert.ToInt32(matrix32.Text);
            weightMtx[8] = Convert.ToInt32(matrix33.Text);
            weight1 = Convert.ToDouble(weight.Text);
            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        private void filterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            fType = filterType.Text;
            if (fType == "Gaussian 3x3")
            {
                matrix11.Text = "1";
                matrix12.Text = "2";
                matrix13.Text = "1";
                matrix21.Text = "2";
                matrix22.Text = "4";
                matrix23.Text = "2";
                matrix31.Text = "1";
                matrix32.Text = "2";
                matrix33.Text = "1";
                weight.Text = "16";
            }
            else if (fType == "Laplacian Hard 3x3")
            {
                matrix11.Text = "1";
                matrix12.Text = "-2";
                matrix13.Text = "1";
                matrix21.Text = "-2";
                matrix22.Text = "4";
                matrix23.Text = "-2";
                matrix31.Text = "1";
                matrix32.Text = "-2";
                matrix33.Text = "1";
                weight.Text = "1";
            }
            else if(fType == "Mean Remove 3x3")
            {
                matrix11.Text = "-1";
                matrix12.Text = "-1";
                matrix13.Text = "-1";
                matrix21.Text = "-1";
                matrix22.Text = "9";
                matrix23.Text = "-1";
                matrix31.Text = "-1";
                matrix32.Text = "-1";
                matrix33.Text = "-1";
                weight.Text = "1";
            }
            else
            {
                matrix11.Text = "1";
                matrix12.Text = "1";
                matrix13.Text = "1";
                matrix21.Text = "1";
                matrix22.Text = "1";
                matrix23.Text = "1";
                matrix31.Text = "1";
                matrix32.Text = "1";
                matrix33.Text = "1";
                weight.Text = "9";
            }
        }

        public string FilterType
        {
            get { return fType; }
            set { fType = value; }
        }

        public int[] WeightMtx
        {
            get { return weightMtx; }
            set { weightMtx = value; }
        }

        public double Weight1
        {
            get { return weight1; }
            set { weight1 = value; }
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }
}
