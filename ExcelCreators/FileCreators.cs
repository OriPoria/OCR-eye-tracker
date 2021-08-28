using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.IO;

using Tesseract_OCR.ExcelCreators;
using Tesseract_OCR.Services;
using Excel = Microsoft.Office.Interop.Excel;

namespace Tesseract_OCR
{

    class FileCreators
    {
        public static void CreateTargetAOIDetectionFile(
            Dictionary<string, TargetShortPhrase> shortPhrases,
            List<string> targetShortPhrasesRecognized,
            Dictionary<string, List<string>> sentences,
            List<string> targetSentencesRecognized,
            string textName)
        {
            List<WordAOIDetection> wordsTable = new List<WordAOIDetection>();
            List<PhraseAOIDetection> phrasesTable = new List<PhraseAOIDetection>();
            List<OffTextDetection> offTextTable = new List<OffTextDetection>();

            using (var wb = new ExcelPackage())
            {
                foreach (KeyValuePair<string, TargetShortPhrase> entry in shortPhrases)
                {
                    WordAOIDetection wd = new WordAOIDetection();
                    var shortSentence = "";
                    foreach (string single in entry.Value.Phrase)
                    {
                        shortSentence += single + " ";
                    }
                    wd.TargetWord = shortSentence;
                    wd.AOIName = entry.Key;

                    if (targetShortPhrasesRecognized.Contains(entry.Key))
                    {
                        wd.Detected = "V";
                    }
                    else
                    {
                        wd.Detected = "X";
                    }

                    wordsTable.Add(wd);
                }
                ExcelWorksheet wsWords = wb.Workbook.Worksheets.Add("Words");
                wsWords.Cells[1,1].LoadFromCollectionFiltered(wordsTable);

                string longSentence = "";
                foreach (KeyValuePair<string, List<string>> entry in sentences)
                {
                    PhraseAOIDetection pd = new PhraseAOIDetection();
                    longSentence = "";
                    foreach (string single in entry.Value)
                    {
                        longSentence += single + " ";
                    }
                    pd.TargetPhrase = longSentence;
                    pd.AOIName = entry.Key;
                    if (targetSentencesRecognized.Contains(entry.Key))
                    {
                        pd.Detected = "V";
                    }
                    else
                    {
                        pd.Detected = "X";
                    }
                    phrasesTable.Add(pd);
                }
                ExcelWorksheet wsPhrases = wb.Workbook.Worksheets.Add("Phrases");
                wsPhrases.Cells[1, 1].LoadFromCollectionFiltered(phrasesTable);

                int sentenceIndex = 0;
                foreach (List<string> entry in Form1.offTextSentenecs)
                {
                    OffTextDetection otd = new OffTextDetection();
                    longSentence = "";
                    foreach (string single in entry)
                    {
                        longSentence += single + " ";
                    }
                    otd.Sentence = longSentence;
                    if (Form1.offTextRecognized.Contains(entry))
                    {
                        otd.Detected = "V";
                    }
                    else
                    {
                        otd.Detected = "X";
                    }
                    offTextTable.Add(otd);
                    sentenceIndex++;
                }
                ExcelWorksheet wsOffText = wb.Workbook.Worksheets.Add("Off Text");
                wsOffText.Cells[1, 1].LoadFromCollectionFiltered(offTextTable);

                string path = @"\Target AOI detection.xlsx";

                wb.SaveAs(new FileInfo(Form1.filesPath + path));


            }
        }
            public static void CreateResultsFile(Dictionary<string, int> wordsMap, List<WordUnit> infoWords,
            Dictionary<string, TargetShortPhrase> shortPhrases, 
            List<string> targetShortPhrasesRecognized,
            Dictionary<string, List<string>> sentences, 
            List<string> targetSentencesRecognized,
            string textName)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            Excel.Workbook xlWorkBook;
            Excel.Worksheet shortPhraseWorksheet;
            Excel.Worksheet longPhraseWorksheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            
            shortPhraseWorksheet = xlWorkBook.ActiveSheet as Excel.Worksheet;
            shortPhraseWorksheet.Name = "Words";
            shortPhraseWorksheet.Cells[1, 1] = "Target words:";
            shortPhraseWorksheet.Cells[1, 2] = "AOI name:";
            shortPhraseWorksheet.Cells[1, 3] = "Detected:";
            var index_row_words = 2;
            string shortSentence = "";
            foreach (KeyValuePair<string, TargetShortPhrase> entry in shortPhrases)
            {
                shortSentence = "";
                foreach (string single in entry.Value.Phrase)
                {
                    shortSentence += single + " ";
                }
                shortPhraseWorksheet.Cells[index_row_words, 1] = shortSentence;
                shortPhraseWorksheet.Cells[index_row_words, 2] = entry.Key;

                if (targetShortPhrasesRecognized.Contains(entry.Key))
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
            longPhraseWorksheet.Cells[1, 1] = "Target sentences:";
            longPhraseWorksheet.Cells[1, 2] = "AOI name:";
            longPhraseWorksheet.Cells[1, 3] = "Detected:";
            var index_row_sen = 2;
            string longSentence = "";
            foreach (KeyValuePair<string, List<string>> entry in sentences)
            {
                longSentence = "";
                foreach (string single in entry.Value)
                {
                    longSentence += single + " ";
                }
                longPhraseWorksheet.Cells[index_row_sen, 1] = longSentence;
                longPhraseWorksheet.Cells[index_row_sen, 2] = entry.Key;
                if (targetSentencesRecognized.Contains(entry.Key))
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



            xlApp.DisplayAlerts = false;
            System.IO.Directory.CreateDirectory(Form1.filesPath + @"\Results");
            
            string path = @"\Results\results_" + textName + ".xls";
            xlWorkBook.SaveAs(Form1.filesPath + path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            ExcelService.CloseSources(xlApp, xlWorkBook, new List<Excel.Worksheet>() { shortPhraseWorksheet, longPhraseWorksheet });


        }

        public static void CreatePaddingFile(PaddingBuilder pb, string file_name)
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
            System.IO.Directory.CreateDirectory(Form1.filesPath + @"\AOI boundaries");
            string path = @"\AOI boundaries\AOI_boundaries-" + file_name + ".xlsx";
            xlWorkBook.SaveAs(Form1.filesPath + path, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

            ExcelService.CloseSources(xlApp, xlWorkBook, new List<Excel.Worksheet>() { xlWorkSheet });

        }
        public static void CreateBounderiesPhraseFile(List<AOI> aois, string textName)
        {
            List<PhraseBoundaries> phraseTable = new List<PhraseBoundaries>();
            using (var wb = new ExcelPackage())
            {
                foreach (AOI block in aois)
                {
                    PhraseBoundaries pb = new PhraseBoundaries();
                    pb.Stimulus = textName;
                    pb.Name = block.Name;
                    pb.Group = block.Group;
                    pb.X = block.X1 + block.Width / 2;
                    pb.Y = block.Y1 + block.Height / 2;
                    pb.H = block.Height;
                    pb.W = block.Width;
                    if (block.IsTarget)
                        pb.TargetName = block.TargetName;
                    phraseTable.Add(pb);
                }
                ExcelWorksheet ws = wb.Workbook.Worksheets.Add("Sheet 1");

                ws.Cells[1, 1].LoadFromCollectionFiltered(phraseTable);
                string dirPath = Form1.filesPath + @"\AOI boundaries";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(Form1.filesPath + @"\AOI boundaries");

                string fileName = @"\AOI_boundaries-" + textName + "_c.xlsx";
                wb.SaveAs(new FileInfo(dirPath + fileName));

            }
        }
        public static void CreateBounderiesWordsFile(List<WordUnit> words, string textName, bool frequencyWords, Dictionary<string, int> wordsMap)
        {
            // added license of the package, watch: https://epplussoftware.com/developers/licenseexception
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            List<WordBoundaries> wordsTable = new List<WordBoundaries>();
            using (var wb = new ExcelPackage())
            {
                foreach (WordUnit word in words)
                {
                    WordBoundaries wordBoundaries = new WordBoundaries();
                    wordBoundaries.Stimulus = textName;
                    wordBoundaries.Index = word.WordIndex;
                    wordBoundaries.Word = word.Name;
                    wordBoundaries.X = word.X1 + word.Width / 2;
                    wordBoundaries.Y = word.Y1 + word.Height / 2;
                    wordBoundaries.H = word.Height;
                    wordBoundaries.W = word.Width;
                    if (word.IsTarget)
                        wordBoundaries.TargetName = word.TargetName;
                    wordBoundaries.LengthWord = word.Name.Length;
                    if (frequencyWords)
                        wordBoundaries.Frequency = wordsMap[word.Name];

                    wordsTable.Add(wordBoundaries);
                }
                ExcelWorksheet ws = wb.Workbook.Worksheets.Add("Sheet 1");

                ws.Cells[1, 1].LoadFromCollectionFiltered(wordsTable);
                if (!frequencyWords)
                    ws.DeleteColumn(10);
                string dirPath = Form1.filesPath + @"\AOI boundaries";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(Form1.filesPath + @"\AOI boundaries");

                string fileName = @"\AOI_boundaries-" + textName + "_w.xlsx";
                wb.SaveAs(new FileInfo(dirPath + fileName));

            }
        }
    }
}
