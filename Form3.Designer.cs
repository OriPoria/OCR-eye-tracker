namespace Tesseract_OCR
{
    partial class Form3
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.Label();
            this.page_num_tb = new System.Windows.Forms.TextBox();
            this.txtAdd = new System.Windows.Forms.Button();
            this.txtRemove = new System.Windows.Forms.Button();
            this.txtClear = new System.Windows.Forms.Button();
            this.txtSubmit = new System.Windows.Forms.Button();
            this.page_txt_lbl = new System.Windows.Forms.Label();
            this.short_units_lbl = new System.Windows.Forms.Label();
            this.long_units_lbl = new System.Windows.Forms.Label();
            this.off_text_lbl = new System.Windows.Forms.Label();
            this.history_btn = new System.Windows.Forms.Button();
            this.excel_btn = new System.Windows.Forms.Button();
            this.short_units_lb = new System.Windows.Forms.ListBox();
            this.long_units_lb = new System.Windows.Forms.ListBox();
            this.off_text_lb = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.textBox1.Location = new System.Drawing.Point(168, 9);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(286, 85);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(168, 102);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(170, 26);
            this.textBox2.TabIndex = 2;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.textBox3.Location = new System.Drawing.Point(1, 10);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(162, 28);
            this.textBox3.TabIndex = 3;
            this.textBox3.Text = "Sentence / Word: ";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.textBox4.Location = new System.Drawing.Point(1, 105);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(114, 23);
            this.textBox4.TabIndex = 4;
            this.textBox4.Text = "AOI name:";
            // 
            // page_num_tb
            // 
            this.page_num_tb.Enabled = false;
            this.page_num_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.page_num_tb.Location = new System.Drawing.Point(168, 137);
            this.page_num_tb.Name = "page_num_tb";
            this.page_num_tb.Size = new System.Drawing.Size(86, 26);
            this.page_num_tb.TabIndex = 4;
            // 
            // txtAdd
            // 
            this.txtAdd.Location = new System.Drawing.Point(465, 12);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(103, 26);
            this.txtAdd.TabIndex = 5;
            this.txtAdd.Text = "Add";
            this.txtAdd.UseVisualStyleBackColor = true;
            this.txtAdd.Click += new System.EventHandler(this.txtAdd_click);
            // 
            // txtRemove
            // 
            this.txtRemove.Location = new System.Drawing.Point(465, 51);
            this.txtRemove.Name = "txtRemove";
            this.txtRemove.Size = new System.Drawing.Size(103, 27);
            this.txtRemove.TabIndex = 6;
            this.txtRemove.Text = "Remove";
            this.txtRemove.UseVisualStyleBackColor = true;
            this.txtRemove.Click += new System.EventHandler(this.txtRemove_click);
            // 
            // txtClear
            // 
            this.txtClear.Location = new System.Drawing.Point(465, 93);
            this.txtClear.Name = "txtClear";
            this.txtClear.Size = new System.Drawing.Size(103, 26);
            this.txtClear.TabIndex = 7;
            this.txtClear.Text = "Clear";
            this.txtClear.UseVisualStyleBackColor = true;
            this.txtClear.Click += new System.EventHandler(this.txtClear_click);
            // 
            // txtSubmit
            // 
            this.txtSubmit.BackColor = System.Drawing.SystemColors.HotTrack;
            this.txtSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtSubmit.Location = new System.Drawing.Point(465, 734);
            this.txtSubmit.Name = "txtSubmit";
            this.txtSubmit.Size = new System.Drawing.Size(103, 59);
            this.txtSubmit.TabIndex = 8;
            this.txtSubmit.Text = "Submit !";
            this.txtSubmit.UseVisualStyleBackColor = false;
            this.txtSubmit.Click += new System.EventHandler(this.txtSubmit_click);
            // 
            // page_txt_lbl
            // 
            this.page_txt_lbl.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.page_txt_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.page_txt_lbl.Location = new System.Drawing.Point(1, 140);
            this.page_txt_lbl.Name = "page_txt_lbl";
            this.page_txt_lbl.Size = new System.Drawing.Size(86, 19);
            this.page_txt_lbl.TabIndex = 4;
            this.page_txt_lbl.Text = "Page: ";
            // 
            // short_units_lbl
            // 
            this.short_units_lbl.Location = new System.Drawing.Point(1, 406);
            this.short_units_lbl.Name = "short_units_lbl";
            this.short_units_lbl.Size = new System.Drawing.Size(114, 23);
            this.short_units_lbl.TabIndex = 4;
            this.short_units_lbl.Text = "Words:";
            // 
            // long_units_lbl
            // 
            this.long_units_lbl.Location = new System.Drawing.Point(1, 185);
            this.long_units_lbl.Name = "long_units_lbl";
            this.long_units_lbl.Size = new System.Drawing.Size(100, 23);
            this.long_units_lbl.TabIndex = 0;
            this.long_units_lbl.Text = "Sentnces:";
            // 
            // off_text_lbl
            // 
            this.off_text_lbl.Location = new System.Drawing.Point(1, 639);
            this.off_text_lbl.Name = "off_text_lbl";
            this.off_text_lbl.Size = new System.Drawing.Size(100, 23);
            this.off_text_lbl.TabIndex = 11;
            this.off_text_lbl.Text = "Off Text:";
            // 
            // history_btn
            // 
            this.history_btn.BackColor = System.Drawing.Color.LightSkyBlue;
            this.history_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.history_btn.Location = new System.Drawing.Point(465, 137);
            this.history_btn.Name = "history_btn";
            this.history_btn.Size = new System.Drawing.Size(103, 42);
            this.history_btn.TabIndex = 9;
            this.history_btn.Text = "History";
            this.history_btn.UseVisualStyleBackColor = false;
            this.history_btn.Click += new System.EventHandler(this.history_btn_click);
            // 
            // excel_btn
            // 
            this.excel_btn.BackColor = System.Drawing.Color.LightSkyBlue;
            this.excel_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.excel_btn.Location = new System.Drawing.Point(465, 185);
            this.excel_btn.Name = "excel_btn";
            this.excel_btn.Size = new System.Drawing.Size(103, 60);
            this.excel_btn.TabIndex = 9;
            this.excel_btn.Text = "Upload excel file";
            this.excel_btn.UseVisualStyleBackColor = false;
            this.excel_btn.Click += new System.EventHandler(this.excel_btn_click);
            // 
            // short_units_lb
            // 
            this.short_units_lb.BackColor = System.Drawing.SystemColors.Menu;
            this.short_units_lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.short_units_lb.FormattingEnabled = true;
            this.short_units_lb.ItemHeight = 17;
            this.short_units_lb.Location = new System.Drawing.Point(0, 433);
            this.short_units_lb.Margin = new System.Windows.Forms.Padding(4);
            this.short_units_lb.Name = "short_units_lb";
            this.short_units_lb.Size = new System.Drawing.Size(454, 157);
            this.short_units_lb.TabIndex = 0;
            // 
            // long_units_lb
            // 
            this.long_units_lb.BackColor = System.Drawing.SystemColors.Menu;
            this.long_units_lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.long_units_lb.FormattingEnabled = true;
            this.long_units_lb.ItemHeight = 17;
            this.long_units_lb.Location = new System.Drawing.Point(0, 212);
            this.long_units_lb.Margin = new System.Windows.Forms.Padding(4);
            this.long_units_lb.Name = "long_units_lb";
            this.long_units_lb.Size = new System.Drawing.Size(454, 157);
            this.long_units_lb.TabIndex = 0;
            // 
            // off_text_lb
            // 
            this.off_text_lb.BackColor = System.Drawing.SystemColors.Menu;
            this.off_text_lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.off_text_lb.ItemHeight = 17;
            this.off_text_lb.Location = new System.Drawing.Point(0, 665);
            this.off_text_lb.Name = "off_text_lb";
            this.off_text_lb.Size = new System.Drawing.Size(454, 106);
            this.off_text_lb.TabIndex = 10;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(580, 805);
            this.Controls.Add(this.txtSubmit);
            this.Controls.Add(this.txtClear);
            this.Controls.Add(this.txtRemove);
            this.Controls.Add(this.txtAdd);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.long_units_lb);
            this.Controls.Add(this.page_txt_lbl);
            this.Controls.Add(this.page_num_tb);
            this.Controls.Add(this.history_btn);
            this.Controls.Add(this.excel_btn);
            this.Controls.Add(this.short_units_lb);
            this.Controls.Add(this.short_units_lbl);
            this.Controls.Add(this.long_units_lbl);
            this.Controls.Add(this.off_text_lb);
            this.Controls.Add(this.off_text_lbl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form3";
            this.Text = "Insert sentences or titles";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListBox long_units_lb;
        public System.Windows.Forms.ListBox short_units_lb;
        public System.Windows.Forms.ListBox off_text_lb;
        private System.Windows.Forms.Label short_units_lbl;
        private System.Windows.Forms.Label long_units_lbl;
        private System.Windows.Forms.Label off_text_lbl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label textBox3;
        private System.Windows.Forms.Label textBox4;
        private System.Windows.Forms.Label page_txt_lbl;
        private System.Windows.Forms.TextBox page_num_tb;
        private System.Windows.Forms.Button txtAdd;
        private System.Windows.Forms.Button txtRemove;
        private System.Windows.Forms.Button txtClear;
        private System.Windows.Forms.Button txtSubmit;
        private System.Windows.Forms.Button history_btn;
        private System.Windows.Forms.Button excel_btn;


    }
}