using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Tesseract_OCR.Services;

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
                MessageBox.Show("Please fill AOI name");
            }
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                string text = StringService.CleanString(StringService.RemovePunctuation(textBox1.Text));
                string name = textBox2.Text;
                string dataStr = text + "," + name;
                if (page_num_tb.Text.Length > 0)
                {
                    try
                    {
                        int.Parse(page_num_tb.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("invalid page number");
                        return;
                    }
                    finally
                    {
                        cleanTextBoxes();
                    }
                    dataStr = dataStr + "," + page_num_tb.Text;
                }
                addToList(name, text);
                string[] strs = { dataStr };
                File.AppendAllLines(@"./history.txt", strs);

            }
            cleanTextBoxes();

        }
        private void cleanTextBoxes()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            page_num_tb.Text = "";

        }

        private void txtRemove_click(object sender, EventArgs e)
        {
            if (long_units_lb.SelectedIndex != -1)
                long_units_lb.Items.RemoveAt(long_units_lb.SelectedIndex);
        }

        private void txtClear_click(object sender, EventArgs e)
        {
            long_units_lb.Items.Clear();
        }

        private void txtSubmit_click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> longUnits = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> shortUnits = new Dictionary<string, List<string>>();
            string[] param;
            var sentenceTxt = " ";
            string[] parSentence;
            
            var aoiName = " ";
            var items = long_units_lb.Items;
            // sentences include long and short phrases
            foreach (string item in short_units_lb.Items)
            {
                param = item.Split(',');
                string[] words = param[0].Split(' ');
                List<string> shortPhrase = new List<string>();
                for (int i = 0; i < words.Length; i++)
                    shortPhrase.Add(words[i]);
                shortUnits.Add(param[1], shortPhrase);
                items.Add(item);
            }

            List<string> splitSentence = new List<string>();
            foreach (string item in items)
            {
                param = item.Split(',');
               
                sentenceTxt = param[0];
                parSentence = sentenceTxt.Split(' ');
                for (int i = 0; i <= parSentence.Length - 1; i++)
                {
                    if (parSentence[i] != "")
                        splitSentence.Add(parSentence[i]);
                }
                
                aoiName = param[1];
                if (aoiName == " ")
                    continue;

                longUnits.Add(aoiName, splitSentence);

                splitSentence = new List<string>();
            }
            Form1.SetSentences(longUnits, shortUnits);
            this.Close();
            MessageBox.Show("Saved !");

            
        
        }


        private void history_btn_click(object sender, EventArgs e)
        {
            History histryForm = new History(this);
            histryForm.Show();
        }
        private void excel_btn_click(object sender, EventArgs e)
        {
            ExcelService excelService = new ExcelService();
            List<IEnumerable<string>> table = excelService.ReadExcelFile<string>();
            foreach (List<string> line in table)
            {
                string text = (StringService.RemovePunctuationPhrase(line[1]));
                string name = line[2];
                addToList(name, text);
            }
        }
        // the function gets the custom name and the text and decides the category of the text
        // meanwhile without page
        public void addToList(string customName, string text)
        {
            if (text.Split(' ').Length > 1)
                long_units_lb.Items.Add(text + "," + customName);
            else
                short_units_lb.Items.Add(text + "," + customName);

        }

    }
}
