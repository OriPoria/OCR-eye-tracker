using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    public class WordPaddingBuilder : PaddingBuilder
    {
        private List<WordUnit> info_word;

        public WordPaddingBuilder(List<WordUnit> words)
        {
            this.info_word = words;
        }
        public void Build(Worksheet xlWorkSheet)
        {

            xlWorkSheet.Cells[1, 1] = "word";
            xlWorkSheet.Cells[1, 2] = "X1";
            xlWorkSheet.Cells[1, 3] = "Y1";
            xlWorkSheet.Cells[1, 4] = "X2";
            xlWorkSheet.Cells[1, 5] = "Y2";
            xlWorkSheet.Cells[1, 6] = "Height";
            xlWorkSheet.Cells[1, 7] = "Width";

            xlWorkSheet.Columns[6].ColumnWidth = 13;
            xlWorkSheet.Columns[7].ColumnWidth = 13;

            var index_row = 2;
            foreach (BlockInfo block in info_word)
            {
                var len = block.Name.Length;
                xlWorkSheet.Cells[index_row, 1] = block.Name;
                xlWorkSheet.Cells[index_row, 2] = block.X1;
                xlWorkSheet.Cells[index_row, 3] = block.Y1;
                xlWorkSheet.Cells[index_row, 4] = block.X2;
                xlWorkSheet.Cells[index_row, 5] = block.Y2;
                xlWorkSheet.Cells[index_row, 6] = block.Height;
                xlWorkSheet.Cells[index_row, 7] = block.Width;

                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

        }
    }
}
