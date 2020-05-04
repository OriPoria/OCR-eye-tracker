using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract_OCR;

namespace Tesseract_OCR
{
    
    public partial class Form2 : Form
    {
        //public ListBox.ObjectCollection items;
        public Form2()
        {
            InitializeComponent();
            
        }

        private void txtAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please fill name word ");
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please fill AOI_name");
            }
            if(textBox1.Text != "" && textBox2.Text != "")
            {
                listBox1.Items.Add(textBox1.Text + "," + textBox2.Text);           }

            textBox1.Text = " ";
            textBox2.Text = " ";
        }

        private void txtRemove_click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void txtClear_click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void txtSubmit_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            //if (items != null)
            //{
            //    listBox1.Items = items;
            //}
            var items = listBox1.Items;
            Dictionary<string, string> singleWords = new Dictionary<string, string>();
            string[] param;
            var word_txt = " ";
            var AOI_name = " ";
            foreach (string item in items){
                param = item.Split(',');
                word_txt = param[0];
                AOI_name = param[1];
                word_txt = word_txt.Replace(" ","");
                AOI_name = AOI_name.Replace(" ", "");
                singleWords.Add(word_txt, AOI_name);
            }
            Form1.setWords(singleWords);
            this.Close();
            MessageBox.Show("Saved !");
            //if(items != null)
            //{
            //    //form1.Controls[9].BackColor = Color.Green;
            //    //form1.BackColor = Color.Green;
            //    form1.button1_paint();
            //    form1.Refresh();
            //    //form1.BackColorChanged = true;

            //}
            



        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
