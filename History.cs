using System;
using System.Windows.Forms;

namespace Tesseract_OCR
{
    public partial class History : Form
    {
        Form3 form3;
        string[] lines;
        public History(Form3 f3)
        {
            form3 = f3;
            InitializeComponent();
            try
            {
                lines = System.IO.File.ReadAllLines(@"./history.txt");
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] data = line.Split(',');
                    data_grid.Rows.Add(data);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("no history file found");
            }
        }
            
    }
}
