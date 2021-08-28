using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;

namespace Tesseract_OCR
{
    public class PhrasesPaddingBuilder : PaddingBuilder
    {
        private List<AOI> infoAoi;
        private string textName;

        public PhrasesPaddingBuilder(List<AOI> aois, string tn)
        {
            infoAoi = aois;
            textName = tn;
        }
        public void Build(Worksheet xlWorkSheet)
        {
            xlWorkSheet.Cells[1, 1] = "Stimulus";
            xlWorkSheet.Cells[1, 2] = "Name";
            xlWorkSheet.Cells[1, 3] = "Group";
            xlWorkSheet.Cells[1, 4] = "X";
            xlWorkSheet.Cells[1, 5] = "Y";
            xlWorkSheet.Cells[1, 6] = "H";
            xlWorkSheet.Cells[1, 7] = "L";
            xlWorkSheet.Cells[1, 8] = "Target Name";

            xlWorkSheet.Columns[1].ColumnWidth = 15;
            xlWorkSheet.Columns[8].ColumnWidth = 13;

            xlWorkSheet.Cells[1, 1].EntireRow.Font.Bold = true;


//            var condition_start = 8;
            var index_row = 2;
//            var num_conds = 0;
            foreach (AOI block in infoAoi)
            {
                var len = block.Name.Length;
                xlWorkSheet.Cells[index_row, 1] = textName;
                xlWorkSheet.Cells[index_row, 2] = block.Name;
                xlWorkSheet.Cells[index_row, 3] = block.Group;
                xlWorkSheet.Cells[index_row, 4] = block.X1 + block.Width/2;
                xlWorkSheet.Cells[index_row, 5] = block.Y1 + block.Height/2;
                xlWorkSheet.Cells[index_row, 6] = block.Height;
                xlWorkSheet.Cells[index_row, 7] = block.Width;
                if (block.IsTarget)
                    xlWorkSheet.Cells[index_row, 8] = block.TargetName;
                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = false;
        }


    }
}
