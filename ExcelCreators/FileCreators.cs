using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tesseract_OCR.Services;
using Excel = Microsoft.Office.Interop.Excel;

namespace Tesseract_OCR
{
    class FileCreators
    {
        public static void CreateResultsFile(Dictionary<string, int> wordsMap, List<WordUnit> infoWords, string textName)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet shortPhraseWorksheet;
            Excel.Worksheet longPhraseWorksheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            
            shortPhraseWorksheet = xlWorkBook.ActiveSheet as Excel.Worksheet;
            shortPhraseWorksheet.Name = "Words";
            shortPhraseWorksheet.Cells[1, 1] = "Special words:";
            shortPhraseWorksheet.Cells[1, 2] = "AOI name:";
            shortPhraseWorksheet.Cells[1, 3] = "Detected:";
            var index_row_words = 2;
            string shortSentence = "";
            foreach (KeyValuePair<string, List<string>> entry in Form1.specialShortPhrases)
            {
                shortSentence = "";
                foreach (string single in entry.Value)
                {
                    shortSentence += single + " ";
                }
                shortPhraseWorksheet.Cells[index_row_words, 1] = shortSentence;
                shortPhraseWorksheet.Cells[index_row_words, 2] = entry.Key;

                if (Form1.specialShortPhrasesRecognized.Contains(entry.Key))
                {
                    shortPhraseWorksheet.Cells[index_row_words, 3].Interior.Color = Excel.XlRgbColor.rgbLightGreen;
                }
                else
                {
                    shortPhraseWorksheet.Cells[index_row_words, 3].Interior.Color = Excel.XlRgbColor.rgbRed;
                }

                index_row_words++;
            }
            shortPhraseWorksheet.Columns[1].ColumnWidth = 15;
            shortPhraseWorksheet.Columns[2].ColumnWidth = 15;
            
            longPhraseWorksheet = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            longPhraseWorksheet.Name = "Sentences";
            longPhraseWorksheet.Cells[1, 1] = "Special sentences:";
            longPhraseWorksheet.Cells[1, 2] = "AOI name:";
            longPhraseWorksheet.Cells[1, 3] = "Detected:";
            var index_row_sen = 2;
            string longSentence = "";
            foreach (KeyValuePair<string, List<string>> entry in Form1.specialSentences)
            {
                longSentence = "";
                foreach (string single in entry.Value)
                {
                    longSentence += single + " ";
                }
                longPhraseWorksheet.Cells[index_row_sen, 1] = longSentence;
                longPhraseWorksheet.Cells[index_row_sen, 2] = entry.Key;
                if (Form1.specialSentencesRecognized.Contains(entry.Key))
                {
                    longPhraseWorksheet.Cells[index_row_sen, 3].Interior.Color = Excel.XlRgbColor.rgbLightGreen;
                }
                else
                {
                    longPhraseWorksheet.Cells[index_row_sen, 3].Interior.Color = Excel.XlRgbColor.rgbRed;
                }
                index_row_sen++;
            }
            longPhraseWorksheet.Columns[1].ColumnWidth = 85;
            longPhraseWorksheet.Columns[2].ColumnWidth = 15;


            xlWorkSheet = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            xlWorkSheet.Name = "Results";

            xlWorkSheet.Cells[1, 1] = "word";
            xlWorkSheet.Cells[1, 2] = "frequency";
            xlWorkSheet.Cells[1, 3] = "Length of word";

            xlWorkSheet.Columns[2].ColumnWidth = 15;
            xlWorkSheet.Columns[3].ColumnWidth = 15;

            var index_row = 2;
            foreach (WordUnit word in infoWords)
            {
                var len = word.Name.Length;
                xlWorkSheet.Cells[index_row, 1] = word.Name;
                xlWorkSheet.Cells[index_row, 2] = wordsMap[word.Name].ToString();
                xlWorkSheet.Cells[index_row, 3] = len.ToString();
                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

            xlApp.DisplayAlerts = false;
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\Results");
            string path = @"\Output\Results\results_" + textName + ".xls";

            xlWorkBook.SaveAs(Environment.CurrentDirectory + path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            ExcelService.CloseSources(xlApp, xlWorkBook, new List<Excel.Worksheet>() { shortPhraseWorksheet, longPhraseWorksheet, xlWorkSheet });


        }

        public static void createPaddingFile(PaddingBuilder pb, string file_name)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.DisplayRightToLeft = false;

            pb.Build(xlWorkSheet);
            xlApp.DisplayAlerts = false;
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\Padding");
            string path = @"\Output\Padding\padding_" + file_name + ".xlsx";
            xlWorkBook.SaveAs(Environment.CurrentDirectory + path, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

            ExcelService.CloseSources(xlApp, xlWorkBook, new List<Excel.Worksheet>() { xlWorkSheet });

        }




    }
}
