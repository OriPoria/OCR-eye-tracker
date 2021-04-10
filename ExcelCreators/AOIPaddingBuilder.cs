using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;

namespace Tesseract_OCR
{
    public class AOIPaddingBuilder : PaddingBuilder
    {
        private List<AOI> info_aoi;
        private string textName;

        public AOIPaddingBuilder(List<AOI> aois, string tn)
        {
            info_aoi = aois;
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
            xlWorkSheet.Cells[1, 8] = "Special Name";


            xlWorkSheet.Cells[1, 1].EntireRow.Font.Bold = true;


//            var condition_start = 8;
            var index_row = 2;
//            var num_conds = 0;
            foreach (AOI block in info_aoi)
            {
                var len = block.Name.Length;
                xlWorkSheet.Cells[index_row, 1] = textName;
                xlWorkSheet.Cells[index_row, 2] = block.Name;
                xlWorkSheet.Cells[index_row, 3] = block.Group;
                // not sure which point to insert
                xlWorkSheet.Cells[index_row, 4] = block.X1;
                xlWorkSheet.Cells[index_row, 5] = block.Y1;
                xlWorkSheet.Cells[index_row, 6] = block.Height;
                xlWorkSheet.Cells[index_row, 7] = block.Width;
                if (block.IsSpecial)
                    xlWorkSheet.Cells[index_row, 8] = block.SpecialName;
                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

        }


    }
}
