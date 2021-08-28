using System;
using System.Drawing;

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
            this.start_btn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.RLinch = new System.Windows.Forms.TextBox();
            this.pad_rl_lbl = new System.Windows.Forms.Label();
            this.inch_rl_lbl = new System.Windows.Forms.Label();
            this.pad_ud_lbl = new System.Windows.Forms.Label();
            this.minimum_phase_lbl = new System.Windows.Forms.Label();
            this.UDinch = new System.Windows.Forms.TextBox();
            this.mini_phase = new System.Windows.Forms.TextBox();
            this.inch_ud_lbl = new System.Windows.Forms.Label();
            this.text_name_tb = new System.Windows.Forms.TextBox();
            this.status_lbl = new System.Windows.Forms.Label();
            this.upload_imges_btn = new System.Windows.Forms.Button();
            this.words_freq_cb = new System.Windows.Forms.CheckBox();
            this.upload_word_btn = new System.Windows.Forms.Button();
            this.instructions_lbl = new System.Windows.Forms.Label();
            this.link_lbl = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // start_btn
            // 
            this.start_btn.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.start_btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.start_btn.Location = new System.Drawing.Point(32, 481);
            this.start_btn.Margin = new System.Windows.Forms.Padding(4);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(326, 48);
            this.start_btn.TabIndex = 0;
            this.start_btn.Text = "Let\'s start!";
            this.start_btn.UseVisualStyleBackColor = false;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(61, 267);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(264, 43);
            this.button3.TabIndex = 4;
            this.button3.Text = "Upload excel file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.uploadExcel_Click);
            // 
            // RLinch
            // 
            this.RLinch.Location = new System.Drawing.Point(220, 335);
            this.RLinch.Name = "RLinch";
            this.RLinch.Size = new System.Drawing.Size(48, 26);
            this.RLinch.TabIndex = 7;
            this.RLinch.Text = "40";
            this.RLinch.TextChanged += new System.EventHandler(this.LRinch_Click);
            // 
            // pad_rl_lbl
            // 
            this.pad_rl_lbl.AutoSize = true;
            this.pad_rl_lbl.Location = new System.Drawing.Point(28, 335);
            this.pad_rl_lbl.Name = "pad_rl_lbl";
            this.pad_rl_lbl.Size = new System.Drawing.Size(175, 20);
            this.pad_rl_lbl.TabIndex = 8;
            this.pad_rl_lbl.Text = "Padding for right / left:";
            this.pad_rl_lbl.Click += new System.EventHandler(this.label1_Click);
            // 
            // inch_rl_lbl
            // 
            this.inch_rl_lbl.AutoSize = true;
            this.inch_rl_lbl.Location = new System.Drawing.Point(274, 335);
            this.inch_rl_lbl.Name = "inch_rl_lbl";
            this.inch_rl_lbl.Size = new System.Drawing.Size(40, 20);
            this.inch_rl_lbl.TabIndex = 9;
            this.inch_rl_lbl.Text = "inch";
            // 
            // pad_ud_lbl
            // 
            this.pad_ud_lbl.AutoSize = true;
            this.pad_ud_lbl.Location = new System.Drawing.Point(28, 365);
            this.pad_ud_lbl.Name = "pad_ud_lbl";
            this.pad_ud_lbl.Size = new System.Drawing.Size(176, 20);
            this.pad_ud_lbl.TabIndex = 10;
            this.pad_ud_lbl.Text = "Padding for up / down:";
            // 
            // minimum_phase_lbl
            // 
            this.minimum_phase_lbl.AutoSize = true;
            this.minimum_phase_lbl.Location = new System.Drawing.Point(28, 395);
            this.minimum_phase_lbl.Name = "minimum_phase_lbl";
            this.minimum_phase_lbl.Size = new System.Drawing.Size(188, 20);
            this.minimum_phase_lbl.TabIndex = 10;
            this.minimum_phase_lbl.Text = "Minimum phrase length:";
            // 
            // UDinch
            // 
            this.UDinch.Location = new System.Drawing.Point(220, 365);
            this.UDinch.Name = "UDinch";
            this.UDinch.Size = new System.Drawing.Size(48, 26);
            this.UDinch.TabIndex = 11;
            this.UDinch.Text = "50";
            this.UDinch.TextChanged += new System.EventHandler(this.UDinch_Click);
            // 
            // mini_phase
            // 
            this.mini_phase.Location = new System.Drawing.Point(220, 395);
            this.mini_phase.Name = "mini_phase";
            this.mini_phase.Size = new System.Drawing.Size(48, 26);
            this.mini_phase.TabIndex = 11;
            this.mini_phase.Text = "3";
            this.mini_phase.TextChanged += new System.EventHandler(this.mini_phase_Click);
            // 
            // inch_ud_lbl
            // 
            this.inch_ud_lbl.AutoSize = true;
            this.inch_ud_lbl.Location = new System.Drawing.Point(274, 365);
            this.inch_ud_lbl.Name = "inch_ud_lbl";
            this.inch_ud_lbl.Size = new System.Drawing.Size(40, 20);
            this.inch_ud_lbl.TabIndex = 12;
            this.inch_ud_lbl.Text = "inch";
            // 
            // text_name_tb
            // 
            this.text_name_tb.Location = new System.Drawing.Point(118, 12);
            this.text_name_tb.Name = "text_name_tb";
            this.text_name_tb.Size = new System.Drawing.Size(164, 26);
            this.text_name_tb.TabIndex = 14;
            this.text_name_tb.Text = "Insert text name";
            this.text_name_tb.Click += new System.EventHandler(this.text_name_tb_Click);
            this.text_name_tb.TextChanged += new System.EventHandler(this.nameImage_Click);
            // 
            // status_lbl
            // 
            this.status_lbl.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.status_lbl.Location = new System.Drawing.Point(32, 537);
            this.status_lbl.Margin = new System.Windows.Forms.Padding(4);
            this.status_lbl.Name = "status_lbl";
            this.status_lbl.Size = new System.Drawing.Size(326, 35);
            this.status_lbl.TabIndex = 1;
            this.status_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // upload_imges_btn
            // 
            this.upload_imges_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.upload_imges_btn.Location = new System.Drawing.Point(61, 63);
            this.upload_imges_btn.Name = "upload_imges_btn";
            this.upload_imges_btn.Size = new System.Drawing.Size(264, 50);
            this.upload_imges_btn.TabIndex = 15;
            this.upload_imges_btn.Text = "Upload images directory";
            this.upload_imges_btn.UseVisualStyleBackColor = false;
            this.upload_imges_btn.Click += new System.EventHandler(this.upload_images_btn_click);
            // 
            // words_freq_cb
            // 
            this.words_freq_cb.AutoSize = true;
            this.words_freq_cb.Location = new System.Drawing.Point(65, 436);
            this.words_freq_cb.Name = "words_freq_cb";
            this.words_freq_cb.Size = new System.Drawing.Size(229, 24);
            this.words_freq_cb.TabIndex = 16;
            this.words_freq_cb.Text = "Calculate words frequency";
            this.words_freq_cb.UseVisualStyleBackColor = true;
            // 
            // upload_word_btn
            // 
            this.upload_word_btn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.upload_word_btn.Location = new System.Drawing.Point(61, 119);
            this.upload_word_btn.Name = "upload_word_btn";
            this.upload_word_btn.Size = new System.Drawing.Size(264, 50);
            this.upload_word_btn.TabIndex = 15;
            this.upload_word_btn.Text = "Upload word file";
            this.upload_word_btn.UseVisualStyleBackColor = false;
            this.upload_word_btn.Click += new System.EventHandler(this.upload_word_btn_Click);
            // 
            // instructions_lbl
            // 
            this.instructions_lbl.Location = new System.Drawing.Point(57, 187);
            this.instructions_lbl.Name = "instructions_lbl";
            this.instructions_lbl.Size = new System.Drawing.Size(280, 40);
            this.instructions_lbl.TabIndex = 12;
            this.instructions_lbl.Text = "To convert PDF file to directory of images use this link:";
            this.instructions_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // link_lbl
            // 
            this.link_lbl.Location = new System.Drawing.Point(109, 227);
            this.link_lbl.Name = "link_lbl";
            this.link_lbl.Size = new System.Drawing.Size(184, 23);
            this.link_lbl.TabIndex = 17;
            this.link_lbl.TabStop = true;
            this.link_lbl.Text = "https://pdftoimage.com";
            this.link_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_lbl_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(389, 596);
            this.Controls.Add(this.text_name_tb);
            this.Controls.Add(this.inch_ud_lbl);
            this.Controls.Add(this.UDinch);
            this.Controls.Add(this.mini_phase);
            this.Controls.Add(this.minimum_phase_lbl);
            this.Controls.Add(this.pad_ud_lbl);
            this.Controls.Add(this.inch_rl_lbl);
            this.Controls.Add(this.pad_rl_lbl);
            this.Controls.Add(this.RLinch);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.status_lbl);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.upload_imges_btn);
            this.Controls.Add(this.words_freq_cb);
            this.Controls.Add(this.upload_word_btn);
            this.Controls.Add(this.instructions_lbl);
            this.Controls.Add(this.link_lbl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "OCR";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button upload_imges_btn;
        private System.Windows.Forms.Button upload_word_btn;
        private System.Windows.Forms.CheckBox separate_xml_cb;
        private System.Windows.Forms.CheckBox words_freq_cb;
        private System.Windows.Forms.TextBox RLinch;
        private System.Windows.Forms.Label pad_rl_lbl;
        private System.Windows.Forms.Label inch_rl_lbl;
        private System.Windows.Forms.Label pad_ud_lbl;
        private System.Windows.Forms.Label minimum_phase_lbl;
        private System.Windows.Forms.TextBox UDinch;
        private System.Windows.Forms.TextBox mini_phase;
        private System.Windows.Forms.Label inch_ud_lbl;
        private System.Windows.Forms.TextBox text_name_tb;
        private System.Windows.Forms.Label status_lbl;
        private System.Windows.Forms.Label instructions_lbl;
        private System.Windows.Forms.LinkLabel link_lbl;

        //private System.Windows.Forms.Button pic_click;
    }
}

