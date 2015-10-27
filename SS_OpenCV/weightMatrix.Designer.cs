namespace SS_OpenCV
{
    partial class weightMatrix
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnApply = new System.Windows.Forms.Button();
            this.matrix21 = new System.Windows.Forms.TextBox();
            this.matrix31 = new System.Windows.Forms.TextBox();
            this.matrix11 = new System.Windows.Forms.TextBox();
            this.matrix13 = new System.Windows.Forms.TextBox();
            this.matrix23 = new System.Windows.Forms.TextBox();
            this.matrix33 = new System.Windows.Forms.TextBox();
            this.matrix32 = new System.Windows.Forms.TextBox();
            this.matrix22 = new System.Windows.Forms.TextBox();
            this.matrix12 = new System.Windows.Forms.TextBox();
            this.filterType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.weight = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnApply
            // 
            this.BtnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnApply.Location = new System.Drawing.Point(204, 194);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 23);
            this.BtnApply.TabIndex = 0;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // matrix21
            // 
            this.matrix21.Location = new System.Drawing.Point(106, 110);
            this.matrix21.Name = "matrix21";
            this.matrix21.Size = new System.Drawing.Size(20, 20);
            this.matrix21.TabIndex = 1;
            this.matrix21.Text = "1";
            // 
            // matrix31
            // 
            this.matrix31.Location = new System.Drawing.Point(106, 137);
            this.matrix31.Name = "matrix31";
            this.matrix31.Size = new System.Drawing.Size(20, 20);
            this.matrix31.TabIndex = 2;
            this.matrix31.Text = "1";
            // 
            // matrix11
            // 
            this.matrix11.Location = new System.Drawing.Point(106, 84);
            this.matrix11.Name = "matrix11";
            this.matrix11.Size = new System.Drawing.Size(20, 20);
            this.matrix11.TabIndex = 3;
            this.matrix11.Text = "1";
            // 
            // matrix13
            // 
            this.matrix13.Location = new System.Drawing.Point(157, 84);
            this.matrix13.Name = "matrix13";
            this.matrix13.Size = new System.Drawing.Size(20, 20);
            this.matrix13.TabIndex = 4;
            this.matrix13.Text = "1";
            // 
            // matrix23
            // 
            this.matrix23.Location = new System.Drawing.Point(157, 110);
            this.matrix23.Name = "matrix23";
            this.matrix23.Size = new System.Drawing.Size(20, 20);
            this.matrix23.TabIndex = 5;
            this.matrix23.Text = "1";
            // 
            // matrix33
            // 
            this.matrix33.Location = new System.Drawing.Point(158, 137);
            this.matrix33.Name = "matrix33";
            this.matrix33.Size = new System.Drawing.Size(20, 20);
            this.matrix33.TabIndex = 6;
            this.matrix33.Text = "1";
            // 
            // matrix32
            // 
            this.matrix32.Location = new System.Drawing.Point(132, 137);
            this.matrix32.Name = "matrix32";
            this.matrix32.Size = new System.Drawing.Size(20, 20);
            this.matrix32.TabIndex = 9;
            this.matrix32.Text = "1";
            // 
            // matrix22
            // 
            this.matrix22.Location = new System.Drawing.Point(132, 110);
            this.matrix22.Name = "matrix22";
            this.matrix22.Size = new System.Drawing.Size(20, 20);
            this.matrix22.TabIndex = 8;
            this.matrix22.Text = "1";
            // 
            // matrix12
            // 
            this.matrix12.Location = new System.Drawing.Point(132, 84);
            this.matrix12.Name = "matrix12";
            this.matrix12.Size = new System.Drawing.Size(20, 20);
            this.matrix12.TabIndex = 7;
            this.matrix12.Text = "1";
            // 
            // filterType
            // 
            this.filterType.FormattingEnabled = true;
            this.filterType.Location = new System.Drawing.Point(89, 21);
            this.filterType.Name = "filterType";
            this.filterType.Size = new System.Drawing.Size(121, 21);
            this.filterType.TabIndex = 10;
            this.filterType.SelectedIndexChanged += new System.EventHandler(this.filterType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Filter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Coeficients";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Weight";
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(8, 194);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 14;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // weight
            // 
            this.weight.Location = new System.Drawing.Point(105, 163);
            this.weight.Name = "weight";
            this.weight.Size = new System.Drawing.Size(72, 20);
            this.weight.TabIndex = 15;
            this.weight.Text = "9";
            // 
            // weightMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 227);
            this.Controls.Add(this.weight);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filterType);
            this.Controls.Add(this.matrix32);
            this.Controls.Add(this.matrix22);
            this.Controls.Add(this.matrix12);
            this.Controls.Add(this.matrix33);
            this.Controls.Add(this.matrix23);
            this.Controls.Add(this.matrix13);
            this.Controls.Add(this.matrix11);
            this.Controls.Add(this.matrix31);
            this.Controls.Add(this.matrix21);
            this.Controls.Add(this.BtnApply);
            this.Name = "weightMatrix";
            this.Text = "weightMatrix";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.TextBox matrix21;
        private System.Windows.Forms.TextBox matrix31;
        private System.Windows.Forms.TextBox matrix11;
        private System.Windows.Forms.TextBox matrix13;
        private System.Windows.Forms.TextBox matrix23;
        private System.Windows.Forms.TextBox matrix33;
        private System.Windows.Forms.TextBox matrix32;
        private System.Windows.Forms.TextBox matrix22;
        private System.Windows.Forms.TextBox matrix12;
        private System.Windows.Forms.ComboBox filterType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.TextBox weight;
    }
}