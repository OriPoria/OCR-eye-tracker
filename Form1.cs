using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Tesseract;
using Tesseract_OCR.Services;


namespace Tesseract_OCR
{
    public partial class Form1 : Form
    {

        // phrase recognized keep the name of the target phrase
        public static Dictionary<string, List<string>> targetSentences = new Dictionary<string, List<string>>();
        public static List<string> targetSentencesRecognized = new List<string>();

        public static Dictionary<string, TargetShortPhrase> targetShortPhrases = new Dictionary<string, TargetShortPhrase>();
        public static List<string> targetShortPhrasesRecognized = new List<string>();

        public static List<List<string>> offTextSentenecs = new List<List<string>>();
        public static List<List<string>> offTextRecognized = new List<List<string>>();

        Dictionary<int, List<WordUnit>> infoWordsByPage = new Dictionary<int, List<WordUnit>>();
        List<WordUnit> infoWordsAllText = new List<WordUnit>();
        Dictionary<int,List<AOI>> AOIdic = new Dictionary<int, List<AOI>>();


        public string[] imagesPaths;
        public Bitmap[] imgs;
        public string path = "";
        private TextFile textFile = new TextFile();
        private string[] wordsFromFile;
        public Color ButtonColor = new Color();
        int RL_padding, UD_padding;
        int minium_phrase_len;
        string imageName;

        public static string filesPath = Environment.CurrentDirectory;
        public Form1()
        {
            InitializeComponent();

            this.imageName = text_name_tb.Text;
            this.RL_padding = Int32.Parse(RLinch.Text);
            this.UD_padding = Int32.Parse(UDinch.Text);
        }

        private void nameImage_Click(object sender, EventArgs e)
        {
            this.imageName = text_name_tb.Text;
        }
        private void LRinch_Click(object sender, EventArgs e)
        {
            this.RL_padding = Int32.Parse(RLinch.Text);
        }
        private void UDinch_Click(object sender, EventArgs e)
        {
            this.UD_padding = Int32.Parse(UDinch.Text);            
        }
        private void mini_phase_Click(object sender, EventArgs e)
        {
            this.minium_phrase_len= Int32.Parse(mini_phase.Text);
        }
        private void text_name_tb_Click(object sender, EventArgs e)
        {
            this.text_name_tb.Text = "";
        }
        private void openFileDialog_FileOk(object sender, CancelEventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }

        // every click create a new list of target sentences
        private void uploadExcel_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> longUnits = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> shortUnits = new Dictionary<string, List<string>>();
            List<List<string>> offText = new List<List<string>>();

            ExcelService excelService = new ExcelService();
            if (excelService.ExcelFilePath == null)
                return;
            List<IEnumerable<string>> table = excelService.ReadExcelFile<string>();
            foreach (List<string> line in table)
            {
                if (line.Count != 3 || 
                    line[1] == String.Empty || line[1] == null ||
                    line[2] == String.Empty || line[2] == null)
                    continue;
                string text = (StringService.RemovePunctuationPhrase(line[1]));
                List<string> words = text.Split(' ').ToList();
                words.Remove("");
                string key = line[2];
                if (key == "off_text")
                    offText.Add(words);
                else if (words.Count > 1)
                    longUnits.Add(key, words);
                else
                    shortUnits.Add(key, words);
            }
            SetSentences(longUnits, shortUnits, offText);
            MessageBox.Show("Excel uploaded successfully!");
        }
        private void label1_Click(object sender, EventArgs e) { }
        private void link_lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.link_lbl.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://pdftoimage.com/");
        }

        private void upload_images_btn_click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {

                fbd.SelectedPath = Properties.Settings.Default.Folder_Path;
                
                
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if (!String.IsNullOrEmpty(Properties.Settings.Default.Folder_Path))
                        Properties.Settings.Default.Folder_Path = fbd.SelectedPath;
                    Properties.Settings.Default.Folder_Path = fbd.SelectedPath;
                    filesPath = System.IO.Directory.GetParent(fbd.SelectedPath).FullName +
                        $@"\{Path.GetFileNameWithoutExtension(fbd.SelectedPath)} outputs";
                    imagesPaths = Directory.GetFiles(fbd.SelectedPath);
                    text_name_tb.Text = Path.GetFileName(fbd.SelectedPath);
                }
            }
        }
        private int ResizingImages()
        {
            int width = 0;
            int height = 0;
            var resolutionItem = resolutions_lb.SelectedItem;
            if (resolutionItem == null)
            {
                MessageBox.Show("Image resolution is not selected");
                return -1;
            }

            string[] resToks = resolutionItem.ToString().Split('X');
            width = int.Parse(resToks[0]);
            height = int.Parse(resToks[1]);
            Bitmap[] bitmaps = new Bitmap[imagesPaths.Length];
            var i = 0;
            foreach (var path in imagesPaths)
            {
                try
                {
                    using (var bmp = (Bitmap)Image.FromFile(path))
                    {
                        bitmaps[i] = ImageService.ResizeImage(bmp, width, height);
                        bitmaps[i].Save($"image{i + 1}.png");
                        i += 1;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Images uploading failed");
                    return -1;
                }
            }
            textFile.TextImages = bitmaps;
            imgs = bitmaps;
            return 0;
                    
        }
        private void upload_word_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Word Documents|*.docx", ValidateNames = true, Multiselect = false };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    Thread t = new Thread(() => {
                        textFile.WordFile = file;
                        wordsFromFile = textFile.PagesText;
                    });
                    status_lbl.Text = "Uploading word file";
                    t.Start();
                    t.Join();
                    status_lbl.Text = "";

                }
            }
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            status_lbl.Text = "Loading...";
            this.Refresh();
            if (imagesPaths == null)
            {
                MessageBox.Show("You must upload images");
                status_lbl.Text = "";
                return;
            }
            // if ResizingImages not executed successfully
            int resizingImagesStatus = ResizingImages();
            if (resizingImagesStatus != 0)
            {
                status_lbl.Text = "";
                return;
            }

            var ocr = new TesseractEngine(path, "heb", EngineMode.Default);
            Tesseract.PageIteratorLevel myLevel = PageIteratorLevel.Word;


            // dictionary for the frequncies and temporary for shorst phrase recognition 
            Dictionary<string, int> wordsMap = new Dictionary<string, int>();

            int aoiNumber = 1;
            int aoiGroup = 1;
            int wordIndex = 1;

            for (int indexPage = 1; indexPage <= imgs.Length; indexPage++)
            {
                List<WordUnit> pageInfoWords = new List<WordUnit>();

                using (var page = ocr.Process(imgs[indexPage - 1]))
                {
                    var wordsIndex = 0;
                    WordsMatchService wordsMatcher = null;
                    if (wordsFromFile != null && indexPage <= wordsFromFile.Length)
                        wordsMatcher = new WordsMatchService(wordsFromFile[indexPage - 1].Split(' '));
                    using (var iter = page.GetIterator())
                    {
                        iter.Begin();

                        do
                        {
                            if (iter.TryGetBoundingBox(myLevel, out var rect))
                            {
                                var curText = iter.GetText(myLevel);
                                if (wordsMatcher != null)
                                {
                                    curText = wordsMatcher.MatchWords(curText, wordsIndex);
                                    wordsIndex = wordsMatcher.WordListPointer;
                                }

                                var wordUnitName = StringService.RemovePunctuation(curText);

                                WordUnit wordUnit = new WordUnit(wordUnitName, indexPage, rect.X1, rect.Y1, rect.X2, rect.Y2, rect.Height, rect.Width);
                                wordUnit.SetEndSign(curText);


                                // wordMap is for the frequency
                                if (!wordsMap.ContainsKey(wordUnitName))
                                {
                                    if (wordUnitName == " " || wordUnitName == "")
                                        continue;
                                    wordsMap.Add(wordUnitName, 0);
                                }


                                // check if wordUnit start target short phrase
                                wordUnit.WordIndex = wordIndex;
                                wordIndex++;
                                ShortPhraseRecognition(wordUnit);
                                pageInfoWords.Add(wordUnit);

                            }
                        } while (iter.Next(myLevel));

                        // Off text word
                        for (int i = 0; i < pageInfoWords.Count; i++)
                        {
                            int phraseIndex = 0;
                            foreach (var phrase in offTextSentenecs)
                            {
                                var j = 0;
                                var start = i;
                                var phraseLength = phrase.Count;
                                while (j < phraseLength && (i + j) < pageInfoWords.Count && phrase[j] == pageInfoWords[j + i].Name)
                                    j++;
                                if (j == phraseLength)
                                {
                                    pageInfoWords.RemoveRange(start, phraseLength);
                                    offTextRecognized.Add(phrase);
                                }
                                phraseIndex++;
                            }
                        }


                        Thread frequencyWordThread = new Thread(() => FrequencyWord(wordsMap));
                        if (words_freq_cb.Checked)
                            frequencyWordThread.Start();

                        List<int> limits = new List<int>();
                        var upperBound = int.MaxValue;
                        var lowerBound = 0;

                        for (int f = 0; f <= pageInfoWords.Count - 1; f++)
                        {

                            if (f == pageInfoWords.Count - 1)
                            {
                                // if this is single word in the last line
                                if (upperBound == int.MaxValue && lowerBound == 0)
                                {
                                    upperBound = pageInfoWords[f].Y1;
                                    lowerBound = pageInfoWords[f].Y2;
                                }
                                limits.Add(upperBound);
                                limits.Add(lowerBound);
                                break;
                            }
                            if (AreSameLine(pageInfoWords[f], pageInfoWords[f + 1])) // same line
                            {
                                if (pageInfoWords[f].Y1 < upperBound)
                                {
                                    upperBound = pageInfoWords[f].Y1;
                                }
                                if (pageInfoWords[f].Y2 > lowerBound)
                                {
                                    lowerBound = pageInfoWords[f].Y2;
                                }
                            }
                            else
                            {
                                limits.Add(upperBound);
                                limits.Add(lowerBound);
                                upperBound = int.MaxValue;
                                lowerBound = 0;
                            }
                        }
                        List<int> gaps = new List<int>();
                        var avg_gap = 0;
                        var lower_current = 0;
                        var upper_next = 0;
                        var is_first = 0;

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
                                gaps.Add(limits[0] - avg_gap > 0 ? limits[0] - avg_gap : 0);
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
                        for (int i = 0; i <= pageInfoWords.Count - 1; i++)
                        {
                            if ((i != pageInfoWords.Count - 1) && AreSameLine(pageInfoWords[i], pageInfoWords[i + 1]))
                            {

                                var diff = pageInfoWords[i].X1 - pageInfoWords[i + 1].X2;
                                var gap_for_padding = diff / 2;
                                pageInfoWords[i].X1 -= gap_for_padding;
                                pageInfoWords[i + 1].X2 += (gap_for_padding - 1);
                                pageInfoWords[i].Y1 = gaps[gap_num];

                            }
                            else
                            {
                                pageInfoWords[i].Y1 = gaps[gap_num];
                                //add the right left padding 
                                pageInfoWords[i].X1 -= this.RL_padding;
                                pageInfoWords[first_in_line].X2 += this.RL_padding;
                                last_in_line = i;
                                gap_num++;
                                if (i == pageInfoWords.Count - 1)
                                    last_line = 1;
                                for (int j = first_in_line; j <= last_in_line; j++)
                                {
                                    if (first_line == 1 && pageInfoWords[j].Y1 - this.UD_padding > 0)
                                        pageInfoWords[j].Y1 -= this.UD_padding;

                                    pageInfoWords[j].Y2 = gaps[gap_num];

                                    if (last_line == 1)
                                        pageInfoWords[j].Y2 += this.UD_padding;
                                }
                                first_line = 0;
                                first_in_line = i + 1;
                                gap_num++;
                            }
                        }
                        if (words_freq_cb.Checked)
                            frequencyWordThread.Join();
                    }
                }
                infoWordsByPage[indexPage] = pageInfoWords;
            }

            for (int indexPage = 1; indexPage <= imgs.Length; indexPage++)
            {
                string stimulus = $"{imageName}_p{indexPage}";
                FileCreators.CreateBounderiesWordsFile(infoWordsByPage[indexPage], stimulus, words_freq_cb.Checked, wordsMap);
                System.IO.Directory.CreateDirectory(filesPath + @"\AOI templates");
                string path = @"\AOI templates\" + this.imageName + $"_page{indexPage}" + "_w.xml";
                using (StreamWriter xmlFile = new StreamWriter(filesPath + path, false, Encoding.UTF8))
                {
                    var id_aoi = 1;
                    xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                    xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                    foreach (WordUnit word in infoWordsByPage[indexPage])
                    {
                        var x1_value = word.X1;
                        var y1_value = word.Y1;
                        var x2_value = word.X2;
                        var y2_value = word.Y2;
                        CreateDynamicAoiXml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, word.WordIndex.ToString(),
                            word.WordIndex.ToString(), Constants.NO_SENTENCE, false);
                        id_aoi += 1;
                    }
                    xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                    xmlFile.Close();
                }
                infoWordsAllText.AddRange(infoWordsByPage[indexPage]);
            }

            AoiAlgorithm(ref aoiNumber, ref aoiGroup);
            List<AOI> aoiAllText = new List<AOI>();
            for (int indexPage = 1; indexPage <= imgs.Length; indexPage++)
            {
                List<AOI> aois = AOIdic[indexPage];
                string stimulus = $"{imageName}_p{indexPage}";
                FileCreators.CreateBounderiesPhraseFile(aois, stimulus);

                FileCreators.CreateBounderiesWordsFile(infoWordsByPage[indexPage], stimulus, words_freq_cb.Checked, wordsMap);
                string pathTarget = @"\AOI templates\" + this.imageName + $"_page{indexPage}" + "_c.xml";
                using (StreamWriter xmlFile = new StreamWriter(filesPath + pathTarget, false, Encoding.UTF8))
                {
                    var id_aoi = 1;
                    xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                    xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                    foreach (AOI aoi in aois)
                    {
                        var x1_value = aoi.X1;
                        var y1_value = aoi.Y1;
                        var x2_value = aoi.X2;
                        var y2_value = aoi.Y2;
                        CreateDynamicAoiXml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, aoi.Name,
                            aoi.Group, Constants.SENTENCE, aoi.IsTarget);
                        id_aoi++;
                    }
                    xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                    xmlFile.Close();
                }
                aoiAllText.AddRange(aois);
            }
            FileCreators.CreateBounderiesPhraseFile(aoiAllText, imageName);
            FileCreators.CreateBounderiesWordsFile(infoWordsAllText, imageName, words_freq_cb.Checked, wordsMap);

            FileCreators.CreateTargetAOIDetectionFile(
            targetShortPhrases,
            targetShortPhrasesRecognized,
            targetSentences,
            targetSentencesRecognized,
            imageName);


            status_lbl.Text = "";
            MessageBox.Show("Finished");

        }


        private Dictionary<string, List<string>> StartTargetSentenceDic(int index)
        {
            int tempIndex;
            if (index == 529)
            {
                var x = 324;
            }
            Dictionary<string, List<string>> candide = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, List<string>> entry in targetSentences)
            {
                tempIndex = index;
                bool match = true;
                foreach (string word in entry.Value)
                {
                    if (tempIndex >= infoWordsAllText.Count || word != infoWordsAllText[tempIndex].Name)
                    {
                        match = false;
                        break;
                    }
                    tempIndex += 1;
                }
                if (match)
                    candide.Add(entry.Key, entry.Value);
            }
            return candide;
        }

        public static void SetSentences(Dictionary<string, List<string>> sentences, Dictionary<string, List<string>> shortUnits, List<List<string>> offText)
        {
            offTextSentenecs = offText;
            targetSentences = sentences;
            foreach (KeyValuePair<string,List<string>> item in shortUnits)
                targetShortPhrases.Add(item.Key, new TargetShortPhrase(item.Value));
        }
        public static void FrequencyWord(Dictionary<string, int> wordsMap)
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
        }



        public void CreateAOI(int page, int name, int group, bool isTarget, string targetName, int x1, int y1, int x2, int y2)
        {
            AOI aoi = new AOI();
            aoi.Name = name.ToString();
            aoi.Group = group.ToString();
            aoi.X1 = x1;
            aoi.X2 = x2;
            aoi.Y1 = y1;
            aoi.Y2 = y2;
            aoi.Height = y2 - y1;
            aoi.Width = x2 - x1;
            aoi.IsTarget = isTarget;
            if (aoi.IsTarget)
                aoi.TargetName = targetName;
            if (AOIdic.ContainsKey(page))
                AOIdic[page].Add(aoi);
            else
                AOIdic[page] = new List<AOI>() { aoi };
        }
        public void CreateDynamicAoiXml(int id_aoi, int x1_value, int y1_value, int x2_value, int y2_value,
            StreamWriter xmlFile, string name, string group, int with_sentence, bool isTarget)
        {
            var is_tens = 0;
            var is_hunderd = 0;

            if (id_aoi > 9 && id_aoi < 100)
            {
                is_tens = 1;
            }
            else if (id_aoi >= 100)
            {
                is_hunderd = 1;
            }

            var x1_line = "        <X>" + x1_value + "</X>";
            var y1_line = "        <Y>" + y1_value + "</Y>";
            var x2_line = "        <X>" + x2_value + "</X>";
            var y2_line = "        <Y>" + y2_value + "</Y>";

            xmlFile.WriteLine("  <DynamicAOI>");
            var id = "    <ID>" + id_aoi + "</ID>";
            xmlFile.WriteLine(id);
            xmlFile.WriteLine("    <ParentID>1</ParentID>");
            xmlFile.WriteLine("    <TrackingUsage>None</TrackingUsage>");
            var group_aoi = "    <Group>" + group + "</Group>";
            xmlFile.WriteLine(group_aoi);
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

            }
            else
            {
                name_aoi = name;
            }

            if (isTarget)
            {
                xmlFile.WriteLine("    <Color>NamedColor:Coral</Color>");
            }
            else if (!isTarget)
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
        }

        private void AoiAlgorithm(ref int aoiNumber, ref int aoiGroup) {
            // boolean var, if the word is in the sentences_list
            var inTargetSentence = false;
            var x2 = 0;
            var y2 = 0;
            var x1_prev = 0;
            var y1_prev = 0;
            var prev_page = 0;
            var parts = false;
            var phase_counter = 0;
            var custom_key = "";
            Dictionary<string, List<string>> candide = new Dictionary<string, List<string>>();

            for (int i = 0; i < infoWordsAllText.Count; i++)
            {   
                WordUnit word = infoWordsAllText[i];
                x2 = word.X2;
                y2 = word.Y2;
                candide = StartTargetSentenceDic(i);

                if (candide.Count > 0)
                {
                    inTargetSentence = true;
                    int match = 0;
                    var start_index = i;
                    foreach (KeyValuePair<string, List<string>> entry in candide)
                    {
                        var sentence_recognized = false;
                        custom_key = entry.Key;
                        y1_prev = word.Y1;
                        x1_prev = word.X1;
                        while (match < entry.Value.Count)
                        {
                            if (entry.Value[match] == word.Name)
                                match += 1;
                            else
                            {
                                inTargetSentence = false;
                                if (parts)
                                {
                                    // TODO: make remove aoi by from dictionary
                     //               AOIdic.Remove(aoiNumber.ToString());
                                    parts = true;
                                }
                                break;
                            }

                            //if this in the same Y1 this is same line => continue
                            //else: create AOI until this word
                            if (word.Y1 == y1_prev)
                            {
                                if (match == entry.Value.Count) // the end 
                                {
                                    targetSentencesRecognized.Add(entry.Key);
                                    CreateAOI(word.Page, aoiNumber, aoiGroup, inTargetSentence, entry.Key, word.X1, word.Y1, x2, y2);
                                    inTargetSentence = false;
                                    aoiNumber += 1;
                                    aoiGroup += 1;
                                    sentence_recognized = true;
                                    break;
                                }
                                x1_prev = word.X1;
                                y1_prev = word.Y1;
                                prev_page = word.Page;
                            }
                            // if the current word in the target sentence is in a new line, create AOI for the 
                            // sentence until that word
                            else
                            {
                                CreateAOI(prev_page, aoiNumber, aoiGroup, inTargetSentence, entry.Key, x1_prev, y1_prev, x2, y2);
                                // new section of the AOI
                                parts = true;
                                aoiNumber += 1;
                                x2 = word.X2;
                                y2 = word.Y2;
                                x1_prev = word.X1;
                                y1_prev = word.Y1;
                                prev_page = word.Page;


                                // if the current word is the last in the target sentence
                                if (match == entry.Value.Count) 
                                {
                                    CreateAOI(word.Page, aoiNumber, aoiGroup, inTargetSentence, entry.Key, word.X1, word.Y1, word.X2, word.Y2);
                                    targetSentencesRecognized.Add(entry.Key);
                                    sentence_recognized = true;
                                    inTargetSentence = false;
                                    aoiNumber += 1;
                                    aoiGroup += 1;
                                    break;
                                }
                            }
                            i += 1;
                            // if the current word is the last in the page
                            if (i == infoWordsAllText.Count)
                                break;
                            word = infoWordsAllText[i];
                        }
                        if (sentence_recognized)
                            break;
                        else
                        {
                            word = infoWordsAllText[start_index];
                            i = start_index;
                            match = 0;
                        }

                    }
                }
                else
                {
                    while (i < infoWordsAllText.Count)
                    {
                        word = infoWordsAllText[i];
                        x1_prev = word.X1;
                        y1_prev = word.Y1;
                        phase_counter += 1;
                        parts = false;
                        if ((word.EndWithPunctuation && phase_counter >= minium_phrase_len) ||
                            word.EndOfSentence ||
                             (i+1 < infoWordsAllText.Count && StartTargetSentenceDic(i + 1).Count != 0))
                        {
                            phase_counter = 0;
                            break;
                        }

                        // different line with more than one word in the prev line
                        if (i+1 < infoWordsAllText.Count && word.Y1 != infoWordsAllText[i+1].Y1)
                        {
                            parts = true;
                            break;
                        }
                        i += 1;
                    }
                    CreateAOI(word.Page, aoiNumber, aoiGroup, inTargetSentence, null, word.X1, word.Y1, x2, y2);
                    aoiNumber += 1;
                    if (!parts)
                        aoiGroup += 1;
                }

            }
        }


        private void ShortPhraseRecognition(WordUnit word)
        {
            foreach (KeyValuePair<string, TargetShortPhrase> item in targetShortPhrases)
            {
                TargetShortPhrase shortPhrase = item.Value;
                string key = item.Key;
                // if the first word in the phrase is equal, add to recognition
                // TODO: work on the recognition

                string pattern = @"[ה|ש|מה|ול|ב|ל|כשה|מ]" + shortPhrase.Phrase[0] + "$";
                Regex rg = new Regex(pattern);
                if (Regex.Match(word.Name, pattern).Success || shortPhrase.Phrase[0] == word.Name)
                {
                    targetShortPhrasesRecognized.Add(key);
                    shortPhrase.RecognitionCounter += 1;
                    word.IsTarget = true;
                    word.TargetName = key;
                }
            }
        }



        private bool AreSameLine(WordUnit word1, WordUnit word2)
        {
            if ((Math.Abs(word1.Y1 - word2.Y1) <= 6) ||
                (word2.Name == "-" && word2.Y2 < word1.Y2) ||
                (word1.Name == "-" && word2.Y2 > word1.Y2))
                return true;
            return false;
        }



  
    }
}