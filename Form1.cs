using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using System.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net;
using System.Xml;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Drawing2D;


namespace Tesseract_OCR
{

    //static class Constants
    //{
    //    public const int NO_SENTENCE = 0;
    //    public const int SENTENCE = 1;
    //}
    public partial class Form1 : Form
    {
        public static Dictionary<string, string> singleWords = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> sentences_list = new Dictionary<string, List<string>>();
        Dictionary<string, List<int>> special_AOI = new Dictionary<string, List<int>>();
        public static List<string> special_words_recognized = new List<string>();
        public static List<string> special_sentences_recognized = new List<string>();
  
        public Bitmap img;
        public string path = "";
        public Color ButtonColor = new Color();
        int RL_padding, UD_padding;
        string name_image;

        public Form1()
        {
            InitializeComponent();

           

        }

        private void nameImage_click(object sender, EventArgs e)
        {
            this.name_image = nameImage_txt.Text;

        }

        private void LRinch_click(object sender, EventArgs e)
        {
           
            this.RL_padding = Int32.Parse(RLinch.Text);
        }

        private void UDinch_click(object sender, EventArgs e)
        {
            this.UD_padding = Int32.Parse(UDinch.Text);
            
        }

        public static void setWords(Dictionary<string,string> singles)
        {
            singleWords = singles;
        }

        public static void setSentences(Dictionary<string, List<string>> sentences)
        {
         
            sentences_list = sentences;
        }

        public static void frequency_word(Dictionary<string, int> wordsMap, List<Word> info_words, string name_image)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            foreach (string word in wordsMap.Keys.ToList())
            {
                //var to_url = word.Replace("\"", "");
                var suffix = "?q=" + word + "&type=word&format=web";
                var prefix = "https://tallinzen.net/frequency/";
                var url = prefix + suffix;

                //var url = "https://tallinzen.net/frequency/?q=word&type=word&format=web";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var nodes = doc.DocumentNode.Descendants("table").Where(x => x.Id == "results_table").ToList();
                var value = nodes[0].ChildNodes[3].ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerHtml;//for total value
                wordsMap[word] = Int32.Parse(value);
            }
            create_Results_file(wordsMap, info_words,name_image);
            
        }

        public static void create_Results_file(Dictionary<string, int> wordsMap,List<Word> info_words, string name)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet words_worksheet;
            Excel.Worksheet sentences_worksheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            words_worksheet = xlWorkBook.ActiveSheet as Excel.Worksheet;
            words_worksheet.Name = "Words";
            words_worksheet.Cells[1, 1] = "Special words:";
            words_worksheet.Cells[1, 2] = "AOI name:";
            words_worksheet.Cells[1, 3] = "Detected:";
            var index_row_words = 2;

            foreach(KeyValuePair<string,string> entry in singleWords)
            {
                words_worksheet.Cells[index_row_words, 1] = entry.Key;
                words_worksheet.Cells[index_row_words, 2] = entry.Value;
                //words_worksheet.Range["C3:C3"].Interior.Color = System.Drawing.Color.Red;
                if (special_words_recognized.Contains(entry.Key))
                {
                    words_worksheet.Cells[index_row_words, 3].Interior.Color = Excel.XlRgbColor.rgbLightGreen;
                }
                else
                {
                    words_worksheet.Cells[index_row_words, 3].Interior.Color = Excel.XlRgbColor.rgbRed;
                }
                
                index_row_words++;
            }
            words_worksheet.Columns[1].ColumnWidth = 15;
            words_worksheet.Columns[2].ColumnWidth = 15;

            sentences_worksheet = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            sentences_worksheet.Name = "Sentences";
            sentences_worksheet.Cells[1, 1] = "Special sentences:";
            sentences_worksheet.Cells[1, 2] = "AOI name:";
            sentences_worksheet.Cells[1, 3] = "Detected:";
            var index_row_sen = 2;
            string sen = "";
            foreach (KeyValuePair<string, List<string>> entry in sentences_list)
            {
                sen = "";
                foreach(string single in entry.Value)
                {
                    sen += single+ " ";
                }
                sentences_worksheet.Cells[index_row_sen, 1] = sen;
                sentences_worksheet.Cells[index_row_sen, 2] = entry.Key;
                if (special_sentences_recognized.Contains(entry.Key))
                {
                    sentences_worksheet.Cells[index_row_sen, 3].Interior.Color = Excel.XlRgbColor.rgbLightGreen;
                }
                else
                {
                    sentences_worksheet.Cells[index_row_sen, 3].Interior.Color = Excel.XlRgbColor.rgbRed;
                }
                index_row_sen++;
            }
            sentences_worksheet.Columns[1].ColumnWidth = 55;
            sentences_worksheet.Columns[2].ColumnWidth = 15;


            //xlWorkSheet = xlWorkBook.ActiveSheet as Excel.Worksheet;
            xlWorkSheet = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            xlWorkSheet.Name = "Results";

            xlWorkSheet.Cells[1, 1] = "word";
            xlWorkSheet.Cells[1, 2] = "frequency";
            xlWorkSheet.Cells[1, 3] = "Length of word";

            xlWorkSheet.Columns[2].ColumnWidth = 15;
            xlWorkSheet.Columns[3].ColumnWidth = 15;

            var index_row = 2;
            foreach (Word word in info_words)
            {
                var len = word.name.Length;
                xlWorkSheet.Cells[index_row, 1] = word.name;
                xlWorkSheet.Cells[index_row, 2] = wordsMap[word.name].ToString();
                xlWorkSheet.Cells[index_row, 3] = len.ToString();
                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;


            //xlWorkSheet.SaveAs(Environment.CurrentDirectory + @"\Results_new.xls", Excel.XlFileFormat.xlWorkbookNormal);
            xlApp.DisplayAlerts = false;
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\Results");
            string path = @"\Output\Results\results_" + name + ".xls";

            xlWorkBook.SaveAs(Environment.CurrentDirectory + path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public static void create_padding_file(List<Word> info_words, string name)
        {

            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

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
            foreach (Word word in info_words)
            {
                var len = word.name.Length;
                xlWorkSheet.Cells[index_row, 1] = word.name;
                xlWorkSheet.Cells[index_row, 2] = word.X1;
                xlWorkSheet.Cells[index_row, 3] = word.Y1;
                xlWorkSheet.Cells[index_row, 4] = word.X2;
                xlWorkSheet.Cells[index_row, 5] = word.Y2;
                xlWorkSheet.Cells[index_row, 6] = word.Height;
                xlWorkSheet.Cells[index_row, 7] = word.Width;

                index_row++;
            }
            xlWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

            //xlWorkSheet.SaveAs(Environment.CurrentDirectory + @"\Results_new.xls", Excel.XlFileFormat.xlWorkbookNormal);
            xlApp.DisplayAlerts = false;
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\Padding");
            string path = @"\Output\Padding\padding_" + name + ".xls";
            xlWorkBook.SaveAs(Environment.CurrentDirectory + path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }
        
        public static void create_special_AOI(int part,int divided, string chosen_key, Dictionary<string, List<int>> special_AOI,Word word,int x1, int y1, int x2, int y2)
        {
            List<int> special_values = new List<int>();
            var AOI_special_name = "";
            if (divided == 0)
            {
                if (part == 1)
                    AOI_special_name = chosen_key;
                else
                    AOI_special_name = chosen_key + "_" + part.ToString();
            }
            else
            {
                AOI_special_name = chosen_key + "_" + part.ToString();
            }
      

            special_values = new List<int>();
            special_values.Add(x1);
            special_values.Add(y1);
            special_values.Add(x2);
            special_values.Add(y2);

            special_AOI.Add(AOI_special_name, special_values);
        }
        public void create_dynamicAOI_xml(int id_aoi,int x1_value,int y1_value, int x2_value, int y2_value, StreamWriter xmlFile, string name, int with_sentence)
        {
            var is_tens = 0;
            var is_hunderd = 0;
            int isSpecialWord = 0;

            if (id_aoi > 9 && id_aoi < 100)
            {
                is_tens = 1;
            }
            else if (id_aoi >= 100)
            {
                is_hunderd = 1;
            }
            
            //var x1_value = word.X1;
            //var y1_value = word.Y1;
            //var x2_value = word.X2;
            //var y2_value = word.Y2;
            var x1_line = "        <X>" + x1_value + "</X>";
            var y1_line = "        <Y>" + y1_value + "</Y>";
            var x2_line = "        <X>" + x2_value + "</X>";
            var y2_line = "        <Y>" + y2_value + "</Y>";

            xmlFile.WriteLine("  <DynamicAOI>");
            var id = "    <ID>" + id_aoi + "</ID>";
            xmlFile.WriteLine(id);
            xmlFile.WriteLine("    <ParentID>1</ParentID>");
            xmlFile.WriteLine("    <TrackingUsage>None</TrackingUsage>");
            xmlFile.WriteLine("    <Group />");
            xmlFile.WriteLine("    <Enabled>true</Enabled>");
            xmlFile.WriteLine("    <Scope>Local</Scope>");
            xmlFile.WriteLine("    <Transparency>50</Transparency>");
            xmlFile.WriteLine("    <Points>");
            xmlFile.WriteLine("      <Point>");
            xmlFile.WriteLine(x1_line);
            xmlFile.WriteLine(y1_line);
            xmlFile.WriteLine("      </Point>");
            xmlFile.WriteLine("      <Point>");
            xmlFile.WriteLine(x2_line);
            xmlFile.WriteLine(y2_line);
            xmlFile.WriteLine("      </Point>");
            xmlFile.WriteLine("    </Points>");
            xmlFile.WriteLine("    <BorderWidth>2</BorderWidth>");
            xmlFile.WriteLine("    <Type>Rectangle</Type>");
            xmlFile.WriteLine("    <Style>HalfTransparent</Style>");
            xmlFile.WriteLine("    <HatchStyle>DarkDownwardDiagonal</HatchStyle>");
            //xmlFile.WriteLine("    <Color>NamedColor:DarkGray</Color>");
            var triple_digits = " ";
            if (is_hunderd == 0 && is_tens == 0)
            {
                triple_digits = "00" + id_aoi;
            }
            if (is_tens == 1)
            {
                triple_digits = "0" + id_aoi;
            }
            else if (is_hunderd == 1)
            {
                triple_digits = id_aoi.ToString();
            }

            var name_aoi = "AOI " + triple_digits;
            if (with_sentence == 0)
            {
                if (singleWords.ContainsKey(name))
                {
                    special_words_recognized.Add(name);
                    name_aoi = singleWords[name];
                    isSpecialWord = 1;
                }
            }
            else
            {
                name_aoi = name;
            }
           
            if (with_sentence == 0 && isSpecialWord == 1)
            {
                xmlFile.WriteLine("    <Color>NamedColor:Coral</Color>");
            }
            else if (with_sentence == 1)
            {
                xmlFile.WriteLine("    <Color>NamedColor:Blue</Color>");
            }
            else
            {
                xmlFile.WriteLine("    <Color>NamedColor:DarkGray</Color>");
            }
            var name_line = "    <Name>" + name_aoi + "</Name>";
            xmlFile.WriteLine(name_line);

            xmlFile.WriteLine("    <Font>");
            xmlFile.WriteLine("      <FontName>Tahoma</FontName>");
            xmlFile.WriteLine("      <FontSize>13</FontSize>");
            xmlFile.WriteLine("      <FontStyle>Regular</FontStyle>");
            xmlFile.WriteLine("      <FontUnit>Point</FontUnit>");
            xmlFile.WriteLine("      <FontGdiCharSet>1</FontGdiCharSet>");
            xmlFile.WriteLine("      <FontGdiVerticalFont>false</FontGdiVerticalFont>");
            xmlFile.WriteLine("     </Font>");
            xmlFile.WriteLine("    <ReferenceStimulusID>0</ReferenceStimulusID>");
            xmlFile.WriteLine("    <MovieFrameRate>0</MovieFrameRate>");
            xmlFile.WriteLine("    <PlaneGridRows>0</PlaneGridRows>");
            xmlFile.WriteLine("    <PlaneGridColumns>0</PlaneGridColumns>");
            xmlFile.WriteLine("    <Visible>true</Visible>");
            xmlFile.WriteLine("    <CurrentTimestamp>0</CurrentTimestamp>");
            xmlFile.WriteLine("    <KeyFrames>");
            xmlFile.WriteLine("      <KeyFrame>");
            xmlFile.WriteLine("        <Points>");
            xmlFile.WriteLine("      <Point>");
            xmlFile.WriteLine(x1_line);
            xmlFile.WriteLine(y1_line);
            xmlFile.WriteLine("      </Point>");
            xmlFile.WriteLine("      <Point>");
            xmlFile.WriteLine(x2_line);
            xmlFile.WriteLine(y2_line);
            xmlFile.WriteLine("      </Point>");
            xmlFile.WriteLine("    </Points>");
            xmlFile.WriteLine("        <PinPoints />");
            xmlFile.WriteLine("        <Visible>true</Visible>");
            xmlFile.WriteLine("        <Timestamp>0</Timestamp>");
            xmlFile.WriteLine("        <Angle>0</Angle>");
            xmlFile.WriteLine("        <Area>0</Area>");
            xmlFile.WriteLine("        <ManuallyCreated>true</ManuallyCreated>");
            xmlFile.WriteLine("      </KeyFrame>");
            xmlFile.WriteLine("    </KeyFrames>");
            xmlFile.WriteLine("    <TrackedKeyFrames />");
            xmlFile.WriteLine("  </DynamicAOI>");
            //id_aoi += 1;

        }

        public bool similar_words(string s1, string s2)
        {
            if (s1.Length != s2.Length)
                return false;
            var len = s1.Length;
            //var len = Math.Min(s1.Length, s2.Length);
            int mistake = 0;
        
            for(int i = 0; i <= len-1; i++)
            {
                if (s1.ElementAt(i) != s2.ElementAt(i)){
                    mistake++;
                }
            }
            if (mistake <= 1)

                return true;
            return false;
        }

        private Bitmap ResizeImage(Bitmap mg, Size newSize)
        {
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;

            Bitmap bp;


            if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
            Convert.ToDouble(newSize.Height)))
                ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
            else
                ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
            myThumbHeight = Math.Ceiling(mg.Height / ratio);
            myThumbWidth = Math.Ceiling(mg.Width / ratio);

            Size thumbSize = new Size((int)myThumbWidth, (int)myThumbHeight);
            bp = new Bitmap(newSize.Width, newSize.Height);
            x = (newSize.Width - thumbSize.Width) / 2;
            y = (newSize.Height - thumbSize.Height);
            // Had to add System.Drawing class in front of Graphics ---
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x, y, thumbSize.Width, thumbSize.Height);
            g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);

            return bp;
        }


        private void button2_click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(openFileDialog.FileName);
                Size newSize = new Size(1398, 1082);

                img = ResizeImage(img, newSize);

                //bp.Save("c:\\inetpub\\wwwroot\\warmfrag\\pics\\myi mage.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //img.Save("page2try.jpg");

                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                path = Path.Combine(path, "tessdata");
                path = path.Replace("file:\\", "");
            }
        }

        


        private void btOCR_Click(object sender, EventArgs e)
        {
            txtResult.Text = "Loading ...";
            this.Refresh();
            if (img == null)
            {
                MessageBox.Show("You must upload image");
                return;
            }
            
            
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //var img = new Bitmap(openFileDialog.FileName);

            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            //path = Path.Combine(path, "tessdata");
            //path = path.Replace("file:\\", "");

            var ocr = new TesseractEngine(path, "heb", EngineMode.Default);
            Tesseract.PageIteratorLevel myLevel = PageIteratorLevel.Word;
            Dictionary<string, int> wordsMap = new Dictionary<string, int>();
            List<Word> info_words = new List<Word>();
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output");


            using (var page = ocr.Process(img))

            //using (StreamWriter outputFile = new StreamWriter("Coordinates.csv", false, Encoding.UTF8))
            {

                //    outputFile.WriteLine("word" + "," + "X1" + "," + "Y1" + "," + "X2" + "," + "Y2" + "," + "Height" + "," + "Width");

                using (var iter = page.GetIterator())
                {
                    iter.Begin();

                    do
                    {
                        if (iter.TryGetBoundingBox(myLevel, out var rect))
                        {


                            var curText = iter.GetText(myLevel);

                            //var word = Regex.Replace(curText, "[/,.':!\"]", String.Empty);
                            var word = Regex.Replace(curText, "[^א-ת0-9-]", String.Empty);

                            if (!wordsMap.ContainsKey(word))
                            {
                                if (word == " " || word == "")
                                    continue;
                                wordsMap.Add(word, 0);
                            }
                            
                            info_words.Add(new Word(word, rect.X1, rect.Y1, rect.X2, rect.Y2, rect.Height, rect.Width));
                            
                          
                        }
                    } while (iter.Next(myLevel));

                    //thread !!!!!!!!!!!!!!!
                    Thread t = new Thread(() => frequency_word(wordsMap,info_words,this.name_image));
                    t.Start();

                    List<int> limits = new List<int>();
                    var upper_bound = int.MaxValue;
                    var lower_bound = 0;

                    for (int f = 0; f <= info_words.Count - 1; f++)
                    {
                        if (f == info_words.Count - 1)
                        {
                            limits.Add(upper_bound);
                            limits.Add(lower_bound);
                            break;
                        }
                        if (Math.Abs(info_words[f].Y1 - info_words[f + 1].Y1) <= 6) // same line
                        {
                            if (info_words[f].Y1 < upper_bound)
                            {
                                upper_bound = info_words[f].Y1;
                            }
                            if (info_words[f].Y2 > lower_bound)
                            {
                                lower_bound = info_words[f].Y2;
                            }
                        }
                        else
                        {
                            limits.Add(upper_bound);
                            limits.Add(lower_bound);
                            upper_bound = int.MaxValue;
                            lower_bound = 0;
                        }
                    }
                    List<int> gaps = new List<int>();
                    var avg_gap = 0;
                    var lower_current = 0;
                    var upper_next = 0;
                    var is_first = 0;

                    //gaps.Add(0);
                    for (int index = 1; index <= limits.Count - 1; index++)
                    {
                        if (index == limits.Count - 1)
                        {
                            gaps.Add(limits[index] + avg_gap);
                            break;
                        }
                        avg_gap = Math.Abs(limits[index] - limits[index + 1]) / 2;
                        if (is_first == 0)
                        {
                            gaps.Add(limits[0] - avg_gap);
                            is_first = 1;
                        }
                        lower_current = limits[index] + (avg_gap - 1);
                        upper_next = limits[index + 1] - avg_gap;

                        gaps.Add(lower_current);
                        gaps.Add(upper_next);
                        index++;

                    }



                    var gap_num = 0;
                    var first_in_line = 0;
                    var last_in_line = 0;
                    var first_line = 1;
                    var last_line = 0;
                    for (int i = 0; i <= info_words.Count - 1; i++)
                    {
                      

                        if ((i != info_words.Count - 1) && (Math.Abs(info_words[i].Y1 - info_words[i + 1].Y1) <= 6)) // same line
                        {

                            var diff = info_words[i].X1 - info_words[i + 1].X2;
                            var gap_for_padding = diff / 2;
                            info_words[i].X1 -= gap_for_padding;
                            info_words[i + 1].X2 += (gap_for_padding - 1);
                            info_words[i].Y1 = gaps[gap_num];



                        }
                        else // this word is the last in this line
                        {
                            info_words[i].Y1 = gaps[gap_num];
                            //add the right left padding 
                            info_words[i].X1 -= this.RL_padding;
                            info_words[first_in_line].X2 += this.RL_padding;
                            last_in_line = i;
                            gap_num++;
                            if (i == info_words.Count - 1)
                                last_line = 1;
                            for (int j = first_in_line; j <= last_in_line; j++)
                            {
                                if (first_line == 1)
                                    info_words[j].Y1 -= this.UD_padding;
                                
                                info_words[j].Y2 = gaps[gap_num];

                                if (last_line == 1)
                                    info_words[j].Y2 += this.UD_padding;
                            }
                            first_line = 0;
                            first_in_line = i + 1;
                            gap_num++;
                        }
                    }

                    create_padding_file(info_words,this.name_image);
                    //using (StreamWriter afterPadding = new StreamWriter("Padding.csv", false, Encoding.UTF8))
                    //{
                    //    afterPadding.WriteLine("word" + "," + "X1" + "," + "Y1" + "," + "X2" + "," + "Y2" + "," + "Height" + "," + "Width");
                    //    foreach (Word word in info_words)
                    //    {
                    //        afterPadding.WriteLine(word.name + "," + word.X1 + "," + word.Y1 + "," + word.X2 + "," + word.Y2 + "," + word.Height + "," + word.Width + ",");
                    //    }
                    //}

                    Dictionary<int, string> first_per_id = new Dictionary<int, string>();
                    var num = 0;
                    foreach (List<string> values in sentences_list.Values)
                    {
                        first_per_id.Add(num, values[0]);
                        num++;
                    }



                    //var sen_number = 0;
                    var in_sentence = 0;
                    var x2 = 0;
                    var y2 = 0;
                    var x1_prev = 0;
                    var y1_prev = 0;
                    var index_word = 1;
                    var part = 1;
                    //var inner_index = 0;
                    Dictionary<string, List<string>> candide = new Dictionary<string, List<string>>();
                    List<string> chosen_values = new List<string>();
                    var chosen_key = "";
                    var last_key = "";

                    if (sentences_list != null)
                    {

                        foreach (Word word in info_words)
                        {
                            if (in_sentence == 0)
                            {
                                candide = new Dictionary<string, List<string>>();
                                //inner_index = 0;
                                foreach (KeyValuePair<string, List<string>> entry in sentences_list)
                                {

                                    if ((word.name == entry.Value[0]) || similar_words(word.name, entry.Value[0]))
                                    {
                                        //sen_number = inner_index;
                                        part = 1;
                                        in_sentence = 1;
                                        x2 = word.X2;
                                        y2 = word.Y2;
                                        x1_prev = word.X1;
                                        y1_prev = word.Y1;
                                        index_word = 1;
                                        //candide.Add(sentences_list[sentences_list.Keys.ElementAt(sen_number)]);
                                        candide.Add(entry.Key, entry.Value);
                                    }
                                    //inner_index++;
                                }
                            }
                            else
                            {
                                int i = 0;
                                int match = 0;
                                foreach (KeyValuePair<string, List<string>> entry in candide)
                                {
                                    if (index_word > entry.Value.Count - 1)
                                        continue;
                                    
                                    if (entry.Value[index_word] == word.name || similar_words(entry.Value[index_word], word.name))
                                    {
                                        //sen_number = i;
                                        chosen_values = entry.Value;
                                        chosen_key = entry.Key;
                                        match += 1;
                                    }
                                    i++;
                                }
                                if (match == 0)
                                {

                                    in_sentence = 0;
                                    index_word = 1;
                                    if (part > 1)
                                    {
                                        special_AOI.Remove(last_key);
                                        part = 1;
                                    }
                                    continue;
                                }

                                //if this in the same Y1 this is same line => continue
                                //else: create AOI until this word
                                if (word.Y1 == y1_prev)
                                {

                                    if (index_word == chosen_values.Count - 1) // the end 
                                    {
                                        
                                        in_sentence = 0;
                                        index_word = 1;
                                        last_key = chosen_key + "_" + (part).ToString();
                                        special_sentences_recognized.Add(chosen_key);
                                        create_special_AOI(part, 0, chosen_key, special_AOI, word, word.X1, word.Y1, x2, y2);
                                        //first_per_id.Remove(sen_number);
                                        continue;
                                    }
                                    x1_prev = word.X1;
                                    y1_prev = word.Y1;
                               

                                }
                                else
                                {
                                    last_key = chosen_key + "_" + (part).ToString();
                                    create_special_AOI(part, 1, chosen_key, special_AOI, word, x1_prev, y1_prev, x2, y2);

                                    part += 1;
                                    x2 = word.X2;
                                    y2 = word.Y2;
                                    x1_prev = word.X1;
                                    y1_prev = word.Y1;


                                    if (index_word == chosen_values.Count - 1) // the end 
                                    {
                                        in_sentence = 0;
                                        index_word = 1;
                                        create_special_AOI(part, 1, chosen_key, special_AOI, word, word.X1, word.Y1, word.X2, word.Y2);

                                    }
                                }

                                index_word++;
                            }
                        }
                    }

                    if (separate_checkBox.Checked == false)
                    {
                        System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\For SMI");
                        string path = @"\Output\For SMI\" + this.name_image + ".xml";
                        using (StreamWriter xmlFile = new StreamWriter(Environment.CurrentDirectory+path, false, Encoding.UTF8))
                        {
                            var id_aoi = 1;
                            xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                            xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                            foreach (Word word in info_words)
                            {
                                var x1_value = word.X1;
                                var y1_value = word.Y1;
                                var x2_value = word.X2;
                                var y2_value = word.Y2;
                                create_dynamicAOI_xml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, word.name, Constants.NO_SENTENCE);
                                id_aoi += 1;
                            }
                            foreach (string sentence_key in special_AOI.Keys)
                            {
                                var x1_value = special_AOI[sentence_key][0];
                                var y1_value = special_AOI[sentence_key][1];
                                var x2_value = special_AOI[sentence_key][2];
                                var y2_value = special_AOI[sentence_key][3];
                                create_dynamicAOI_xml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, sentence_key, Constants.SENTENCE);
                                id_aoi++;
                            }
                            xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                            xmlFile.Close();
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\For SMI");
                        string path = @"\Output\For SMI\" + this.name_image + "_allWords.xml";
                        using (StreamWriter xmlFile = new StreamWriter(Environment.CurrentDirectory + path, false, Encoding.UTF8))
                        {
                            var id_aoi = 1;
                            xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                            xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                            foreach (Word word in info_words)
                            {
                                var x1_value = word.X1;
                                var y1_value = word.Y1;
                                var x2_value = word.X2;
                                var y2_value = word.Y2;
                                create_dynamicAOI_xml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, word.name, Constants.NO_SENTENCE);
                                id_aoi += 1;
                            }
                            xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                            xmlFile.Close();
                        }

                        string path_special = @"\Output\For SMI\" + this.name_image + "_special.xml";
                        using (StreamWriter xmlFile = new StreamWriter(Environment.CurrentDirectory + path_special, false, Encoding.UTF8))
                        {
                            var id_aoi = 1;
                            xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                            xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                            foreach (string sentence_key in special_AOI.Keys)
                            {
                                var x1_value = special_AOI[sentence_key][0];
                                var y1_value = special_AOI[sentence_key][1];
                                var x2_value = special_AOI[sentence_key][2];
                                var y2_value = special_AOI[sentence_key][3];
                                create_dynamicAOI_xml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, sentence_key, Constants.SENTENCE);
                                id_aoi++;
                            }
                            xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                            xmlFile.Close();
                        }

                    }

                    t.Join();
                    txtResult.Text = "Finised !";
                    //txtResult.Text = page.GetText();
                }








                //var page = ocr.Process(img);
                //txtResult.Text = page.GetText();
            }
        }

        

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Controls.Add(button1);
            Form2 form2 = new Form2();
            
            //button1.BackColor = Color.Red;
            form2.Show();
            
        }
        //public void button1_paint()
        //{
            
        //    this.button1.BackColor = Color.Black;
           
        //    //this.button1.BackColor = Color.FromArgb(192, 0, 0);
        //    //button1.BackColor = new SolidColorBrush(Color.Red);
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

  
    }
}

        //private static async Task startCrawlerAsync()
        //{
        //    var url = "https://tallinzen.net/frequency/?q=dad&type=word&format=web";
        //    var httpClient = new HttpClient();
        //    var html = await httpClient.GetStringAsync(url);
        //    var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        //    htmlDoc.LoadHtml(html);
        //    Console.Write(htmlDoc);


        //}