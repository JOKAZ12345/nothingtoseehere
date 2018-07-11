namespace SoccerDB
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.soccerDataSet1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.startDateTextBox = new System.Windows.Forms.TextBox();
            this.endDateTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.robinsonLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.robinsonOver = new System.Windows.Forms.Label();
            this.Poisson5LastX1 = new System.Windows.Forms.TextBox();
            this.Poisson5LastX2 = new System.Windows.Forms.TextBox();
            this.Poisson5LastX3 = new System.Windows.Forms.TextBox();
            this.Poisson5LastX4 = new System.Windows.Forms.TextBox();
            this.Poisson5LastX5 = new System.Windows.Forms.TextBox();
            this.Poisson5LastY1 = new System.Windows.Forms.TextBox();
            this.Poisson5LastY2 = new System.Windows.Forms.TextBox();
            this.Poisson5LastY3 = new System.Windows.Forms.TextBox();
            this.Poisson5LastY4 = new System.Windows.Forms.TextBox();
            this.Poisson5LastY5 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.thetaTextBox = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.soccerDataSet1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(232, 320);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 44);
            this.button3.TabIndex = 0;
            this.button3.Text = "Calculate Goals (Poisson)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(409, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 405);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(263, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(500, 405);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(302, 20);
            this.textBox2.TabIndex = 3;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 25);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "Update from CSV";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 125);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "País";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 54);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(120, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Location = new System.Drawing.Point(585, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(212, 125);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ligas";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(20, 57);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(152, 21);
            this.comboBox2.TabIndex = 0;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox3);
            this.groupBox3.Location = new System.Drawing.Point(12, 239);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 125);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Equipa Casa";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(6, 38);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 0;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBox4);
            this.groupBox4.Location = new System.Drawing.Point(585, 239);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 125);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Equipa Fora";
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(6, 38);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(485, 329);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 48);
            this.button6.TabIndex = 9;
            this.button6.Text = "Get segunda liga";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // startDateTextBox
            // 
            this.startDateTextBox.Location = new System.Drawing.Point(232, 278);
            this.startDateTextBox.Name = "startDateTextBox";
            this.startDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.startDateTextBox.TabIndex = 10;
            // 
            // endDateTextBox
            // 
            this.endDateTextBox.Location = new System.Drawing.Point(460, 278);
            this.endDateTextBox.Name = "endDateTextBox";
            this.endDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.endDateTextBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 529);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Robinson Under 2.5 :";
            // 
            // robinsonLabel
            // 
            this.robinsonLabel.AutoSize = true;
            this.robinsonLabel.Location = new System.Drawing.Point(123, 529);
            this.robinsonLabel.Name = "robinsonLabel";
            this.robinsonLabel.Size = new System.Drawing.Size(0, 13);
            this.robinsonLabel.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 516);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Robinson  Over 2.5  :";
            // 
            // robinsonOver
            // 
            this.robinsonOver.AutoSize = true;
            this.robinsonOver.Location = new System.Drawing.Point(123, 516);
            this.robinsonOver.Name = "robinsonOver";
            this.robinsonOver.Size = new System.Drawing.Size(0, 13);
            this.robinsonOver.TabIndex = 15;
            // 
            // Poisson5LastX1
            // 
            this.Poisson5LastX1.Location = new System.Drawing.Point(247, 102);
            this.Poisson5LastX1.Name = "Poisson5LastX1";
            this.Poisson5LastX1.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastX1.TabIndex = 16;
            // 
            // Poisson5LastX2
            // 
            this.Poisson5LastX2.Location = new System.Drawing.Point(247, 128);
            this.Poisson5LastX2.Name = "Poisson5LastX2";
            this.Poisson5LastX2.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastX2.TabIndex = 17;
            // 
            // Poisson5LastX3
            // 
            this.Poisson5LastX3.Location = new System.Drawing.Point(247, 154);
            this.Poisson5LastX3.Name = "Poisson5LastX3";
            this.Poisson5LastX3.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastX3.TabIndex = 18;
            // 
            // Poisson5LastX4
            // 
            this.Poisson5LastX4.Location = new System.Drawing.Point(247, 180);
            this.Poisson5LastX4.Name = "Poisson5LastX4";
            this.Poisson5LastX4.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastX4.TabIndex = 19;
            // 
            // Poisson5LastX5
            // 
            this.Poisson5LastX5.Location = new System.Drawing.Point(247, 206);
            this.Poisson5LastX5.Name = "Poisson5LastX5";
            this.Poisson5LastX5.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastX5.TabIndex = 20;
            // 
            // Poisson5LastY1
            // 
            this.Poisson5LastY1.Location = new System.Drawing.Point(326, 102);
            this.Poisson5LastY1.Name = "Poisson5LastY1";
            this.Poisson5LastY1.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastY1.TabIndex = 21;
            // 
            // Poisson5LastY2
            // 
            this.Poisson5LastY2.Location = new System.Drawing.Point(326, 128);
            this.Poisson5LastY2.Name = "Poisson5LastY2";
            this.Poisson5LastY2.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastY2.TabIndex = 22;
            // 
            // Poisson5LastY3
            // 
            this.Poisson5LastY3.Location = new System.Drawing.Point(326, 154);
            this.Poisson5LastY3.Name = "Poisson5LastY3";
            this.Poisson5LastY3.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastY3.TabIndex = 23;
            // 
            // Poisson5LastY4
            // 
            this.Poisson5LastY4.Location = new System.Drawing.Point(326, 180);
            this.Poisson5LastY4.Name = "Poisson5LastY4";
            this.Poisson5LastY4.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastY4.TabIndex = 25;
            // 
            // Poisson5LastY5
            // 
            this.Poisson5LastY5.Location = new System.Drawing.Point(326, 206);
            this.Poisson5LastY5.Name = "Poisson5LastY5";
            this.Poisson5LastY5.Size = new System.Drawing.Size(28, 20);
            this.Poisson5LastY5.TabIndex = 26;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(422, 109);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 27;
            this.button7.Text = "Poisson 100";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(257, 36);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 28;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // thetaTextBox
            // 
            this.thetaTextBox.Location = new System.Drawing.Point(605, 15);
            this.thetaTextBox.Name = "thetaTextBox";
            this.thetaTextBox.Size = new System.Drawing.Size(100, 20);
            this.thetaTextBox.TabIndex = 29;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(711, 12);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 30;
            this.button9.Text = "Theta";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(814, 551);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.thetaTextBox);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.Poisson5LastY5);
            this.Controls.Add(this.Poisson5LastY4);
            this.Controls.Add(this.Poisson5LastY3);
            this.Controls.Add(this.Poisson5LastY2);
            this.Controls.Add(this.Poisson5LastY1);
            this.Controls.Add(this.Poisson5LastX5);
            this.Controls.Add(this.Poisson5LastX4);
            this.Controls.Add(this.Poisson5LastX3);
            this.Controls.Add(this.Poisson5LastX2);
            this.Controls.Add(this.Poisson5LastX1);
            this.Controls.Add(this.robinsonOver);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.robinsonLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.endDateTextBox);
            this.Controls.Add(this.startDateTextBox);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.soccerDataSet1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource soccerDBDataSetBindingSource;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.BindingSource soccerDataSetBindingSource;
        //private soccerDataSet soccerDataSet;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ComboBox comboBox4;
        //private soccerDataSet soccerDataSet1;
        private System.Windows.Forms.BindingSource soccerDataSet1BindingSource;
        private System.Windows.Forms.TextBox startDateTextBox;
        private System.Windows.Forms.TextBox endDateTextBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label robinsonLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label robinsonOver;
        private System.Windows.Forms.TextBox Poisson5LastX1;
        private System.Windows.Forms.TextBox Poisson5LastX2;
        private System.Windows.Forms.TextBox Poisson5LastX3;
        private System.Windows.Forms.TextBox Poisson5LastX4;
        private System.Windows.Forms.TextBox Poisson5LastX5;
        private System.Windows.Forms.TextBox Poisson5LastY1;
        private System.Windows.Forms.TextBox Poisson5LastY2;
        private System.Windows.Forms.TextBox Poisson5LastY3;
        private System.Windows.Forms.TextBox Poisson5LastY4;
        private System.Windows.Forms.TextBox Poisson5LastY5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox thetaTextBox;
        private System.Windows.Forms.Button button9;
    }
}

