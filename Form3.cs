using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tesseract_OCR
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void txtAdd_click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please fill Sentence / Title  ");
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please fill AOI_name");
            }
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                listBox1.Items.Add(textBox1.Text + "," + textBox2.Text);
            }

            textBox1.Text = " ";
            textBox2.Text = " ";
        }

        private void txtRemove_click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void txtClear_click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void txtSubmit_click(object sender, EventArgs e)
        {
            var items = listBox1.Items;
            Dictionary<string, List<string>> Sentences = new Dictionary<string, List<string>>();
            string[] param;
            var Sentence_txt = " ";
            string[] par_sentence;
            
            var AOI_name = " ";

            List<string> split_sentence = new List<string>();
            foreach (string item in items)
            {
                
                param = item.Split(',');
               
                Sentence_txt = param[0];
                par_sentence = Sentence_txt.Split(' ');
                for( int i =0;i<=par_sentence.Length-1;i++)
                {
                    if (par_sentence[i] != "")
                        split_sentence.Add(par_sentence[i]);
                }
                
                AOI_name = param[1];
                if (AOI_name == " ")
                {
                    continue;
                }
                Sentences.Add(AOI_name, split_sentence);
                split_sentence = new List<string>();
                //Sentences.Add(par_sentence, AOI_name);
            }
            Form1.setSentences(Sentences);
            this.Close();
            MessageBox.Show("Saved !");
            
        
        }
    }
}
