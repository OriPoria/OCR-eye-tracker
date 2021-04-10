using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.IO;
using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Tesseract_OCR.Services
{
    public class ExcelService
    {
        string excelFilePath;
        public ExcelService()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                Title = "Open The Excel File: ",
                Filter = "Excel files (*.xls*)|*.xls*",
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true
            })
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    excelFilePath = openFileDialog.FileName;
                }
                else
                {
                    return;
                }

            }
        }
        // The same function from EM Analyzer, excel service
        public List<IEnumerable<T>> ReadExcelFile<T>()
        {
            List<IEnumerable<T>> table = new List<IEnumerable<T>>();
            try
            {
                // added license of the package, watch: https://epplussoftware.com/developers/licenseexception
                 ExcelPackage.LicenseContext = LicenseContext.Commercial;

                using (var wb = new ExcelPackage(new FileInfo(excelFilePath)))
                {

                    ExcelWorksheet ws = wb.Workbook.Worksheets.First();
                    int firstRowUsed = ws.Dimension.Start.Row;
                    int lastColUsed = ws.Dimension.End.Column;
                    ExcelRow categoryRow = ws.Row(firstRowUsed);


                    // Move to the next row (it now has the titles)
                    for (int currentRow = firstRowUsed + 1; currentRow <= ws.Dimension.End.Row; currentRow++)
                    {
                        ExcelRow row = ws.Row(currentRow);
                        ExcelRange range = ws.Cells[currentRow, 1, currentRow, lastColUsed];
                        table.Add(range.Select(cell => cell.GetValue<T>()).ToList());
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "ערך אינו יכול להיות Null.\r\nשם פרמטר: fileName")
                    MessageBox.Show("No file selected");
            }

            return table;
        }
        public static void CloseSources(Excel.Application xlApp, Excel.Workbook xlWorkBook, List<Excel.Worksheet> xlSheets)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = 0; i < xlSheets.Count; i++)
                Marshal.FinalReleaseComObject(xlSheets[i]);
            xlWorkBook.Close(Type.Missing, Type.Missing, Type.Missing);
            Marshal.FinalReleaseComObject(xlWorkBook);
            xlApp.Quit();
            Marshal.FinalReleaseComObject(xlApp);
        }

    }
}
