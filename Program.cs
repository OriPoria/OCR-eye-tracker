using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Tesseract_OCR
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
             Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Form1());
        }
    }
}
