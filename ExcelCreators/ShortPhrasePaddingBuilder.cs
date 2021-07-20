using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    public class ShortPhrasePaddingBuilder : PaddingBuilder
    {
        private List<WordUnit> info_word;
        private string textName;

        public ShortPhrasePaddingBuilder(List<WordUnit> words, string tn)
        {
            this.info_word = words;
            textName = tn;
        }
        public void Build(Worksheet xlWorkSheet)
        {
            xlWorkSheet.Cells[1, 1] = "Stimulus";
            xlWorkSheet.Cells[1, 2] = "Index";
            xlWorkSheet.Cells[1, 3] = "Word";
            xlWorkSheet.Cells[1, 4] = "X";
            xlWorkSheet.Cells[1, 5] = "Y";
            xlWorkSheet.Cells[1, 6] = "H";
            xlWorkSheet.Cells[1, 7] = "W";
            xlWorkSheet.Cells[1, 8] = "Target Name";

            xlWorkSheet.Columns[8].ColumnWidth = 13;

            xlWorkSheet.Cells[1, 1].EntireRow.Font.Bold = true;

            var index_row = 2;
            foreach (WordUnit word in info_word)
            {
                var len = word.Name.Length;
                xlWorkSheet.Cells[index_row, 1] = textName;
                xlWorkSheet.Cells[index_row, 2] = word.WordIndex;
                xlWorkSheet.Cells[index_row, 3] = word.Name;
                xlWorkSheet.Cells[index_row, 4] = word.X1 + word.Width/2;
                xlWorkSheet.Cells[index_row, 5] = word.Y1 + word.Height/2;
                xlWorkSheet.Cells[index_row, 6] = word.Height;
                xlWorkSheet.Cells[index_row, 7] = word.Width;
                if (word.IsTarget)
                    xlWorkSheet.Cells[index_row, 8] = word.TargetName;
                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

        }
    }
}
