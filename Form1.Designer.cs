namespace Tesseract_OCR
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
            this.btOCR = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.separate_checkBox = new System.Windows.Forms.CheckBox();
            this.RLinch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UDinch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nameImage_txt = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btOCR
            // 
            this.btOCR.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btOCR.Location = new System.Drawing.Point(33, 330);
            this.btOCR.Margin = new System.Windows.Forms.Padding(4);
            this.btOCR.Name = "btOCR";
            this.btOCR.Size = new System.Drawing.Size(326, 48);
            this.btOCR.TabIndex = 0;
            this.btOCR.Text = "Let\'s start!";
            this.btOCR.UseVisualStyleBackColor = false;
            this.btOCR.Click += new System.EventHandler(this.btOCR_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(61, 106);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(264, 48);
            this.button1.TabIndex = 2;
            this.button1.Text = "Insert figurative/literal words";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(61, 161);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(264, 43);
            this.button3.TabIndex = 4;
            this.button3.Text = "Insert sentences or titles";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button2.Location = new System.Drawing.Point(61, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(264, 51);
            this.button2.TabIndex = 5;
            this.button2.Text = "Upload image (1400 X 1082 size) ";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_click);
            // 
            // separate_checkBox
            // 
            this.separate_checkBox.AutoSize = true;
            this.separate_checkBox.Location = new System.Drawing.Point(65, 292);
            this.separate_checkBox.Name = "separate_checkBox";
            this.separate_checkBox.Size = new System.Drawing.Size(174, 20);
            this.separate_checkBox.TabIndex = 6;
            this.separate_checkBox.Text = "Create separate xml files";
            this.separate_checkBox.UseVisualStyleBackColor = true;
            // 
            // RLinch
            // 
            this.RLinch.Location = new System.Drawing.Point(217, 228);
            this.RLinch.Name = "RLinch";
            this.RLinch.Size = new System.Drawing.Size(48, 22);
            this.RLinch.TabIndex = 7;
            this.RLinch.TextChanged += new System.EventHandler(this.LRinch_click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Padding for right / left :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "inch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 259);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Padding for up / down :";
            // 
            // UDinch
            // 
            this.UDinch.Location = new System.Drawing.Point(217, 258);
            this.UDinch.Name = "UDinch";
            this.UDinch.Size = new System.Drawing.Size(48, 22);
            this.UDinch.TabIndex = 11;
            this.UDinch.TextChanged += new System.EventHandler(this.UDinch_click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 263);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "inch";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Name :";
            // 
            // nameImage_txt
            // 
            this.nameImage_txt.Location = new System.Drawing.Point(118, 12);
            this.nameImage_txt.Name = "nameImage_txt";
            this.nameImage_txt.Size = new System.Drawing.Size(164, 22);
            this.nameImage_txt.TabIndex = 14;
            this.nameImage_txt.TextChanged += new System.EventHandler(this.nameImage_click);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResult.Location = new System.Drawing.Point(159, 386);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(90, 35);
            this.txtResult.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(396, 419);
            this.Controls.Add(this.nameImage_txt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UDinch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RLinch);
            this.Controls.Add(this.separate_checkBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btOCR);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "OCR";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOCR;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox separate_checkBox;
        private System.Windows.Forms.TextBox RLinch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UDinch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox nameImage_txt;
        private System.Windows.Forms.TextBox txtResult;
        //private System.Windows.Forms.Button pic_click;
    }
}

