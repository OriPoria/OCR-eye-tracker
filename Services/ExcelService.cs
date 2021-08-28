using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Forms;
using System.Data;
using OfficeOpenXml.Attributes;
using OfficeOpenXml.Table;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;

namespace Tesseract_OCR.Services
{
    public class ExcelService
    {
        public string ExcelFilePath { get; set; }
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
                    ExcelFilePath = openFileDialog.FileName;
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
            // added license of the package, watch: https://epplussoftware.com/developers/licenseexception
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            List<IEnumerable<T>> table = new List<IEnumerable<T>>();
            try
            {
                using (var wb = new ExcelPackage(new FileInfo(ExcelFilePath)))
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
    public static class Extensions
    {
        public static ExcelRangeBase LoadFromCollectionFiltered<T>(this ExcelRangeBase @this, IEnumerable<T> collection)//, bool PrintHeaders, TableStyles styles) where T : class
        {
            MemberInfo[] membersToInclude = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore)))
                .ToArray();

            return @this.LoadFromCollection(collection, true,
                TableStyles.None,
                BindingFlags.Instance | BindingFlags.Public,
                membersToInclude);
        }
    }
}
