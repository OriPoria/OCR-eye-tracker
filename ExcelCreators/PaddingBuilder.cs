using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace Tesseract_OCR
{
    public interface PaddingBuilder
    {
        void Build(Excel.Worksheet xlWorkSheet);
    }
}
