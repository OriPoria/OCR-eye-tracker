using System;
using System.Windows.Forms;

namespace Tesseract_OCR
{
    partial class History
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
            this.data_grid = new System.Windows.Forms.DataGridView();
            this.phrase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.page = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.add_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.data_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // data grid
            // 
            this.data_grid.ColumnHeadersHeight = 29;
            this.data_grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.phrase,
            this.name,
            this.page});
            this.data_grid.Location = new System.Drawing.Point(35, 12);
            this.data_grid.Name = "data_grid";
            this.data_grid.RowHeadersVisible = false;
            this.data_grid.Size = new System.Drawing.Size(475, 347);
            this.data_grid.TabIndex = 0;
            this.data_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.data_grid.MultiSelect = false;
            // 
            // phrase
            // 
            this.phrase.HeaderText = "Phrase";
            this.phrase.MinimumWidth = 6;
            this.phrase.Name = "phrase";
            this.phrase.Width = 250;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.Width = 125;
            // 
            // page
            // 
            this.page.HeaderText = "Page ";
            this.page.MinimumWidth = 6;
            this.page.Name = "page";
            this.page.Width = 75;
            // 
            // add button
            // 
            this.add_btn.BackColor = System.Drawing.SystemColors.HotTrack;
            this.add_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.add_btn.Location = new System.Drawing.Point(214, 374);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(145, 34);
            this.add_btn.TabIndex = 1;
            this.add_btn.Text = "Add";
            this.add_btn.UseVisualStyleBackColor = false;
            this.add_btn.Click += this.add_btn_click;
            // 
            // History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(580, 420);
            this.Controls.Add(this.data_grid);
            this.Controls.Add(this.add_btn);
            this.Name = "History";
            this.Text = "History";
            ((System.ComponentModel.ISupportInitialize)(this.data_grid)).EndInit();
            this.ResumeLayout(false);

        }

        private void add_btn_click(object sender, EventArgs e)
        {
            string[] columns = this.lines[data_grid.CurrentRow.Index].Split(',');
            // first parameter is the custom name and second is the text (phrase)
            form3.addToList(columns[1], columns[0]);
        }

        #endregion
        private DataGridView data_grid;
        private DataGridViewTextBoxColumn phrase;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn page;
        private Button add_btn;
    }
}