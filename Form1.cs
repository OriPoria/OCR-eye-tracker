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
        // phrase recognized keep the name of the special phrase
        public static Dictionary<string, List<string>> specialSentences = new Dictionary<string, List<string>>();
        public static List<string> specialSentencesRecognized = new List<string>();
        public static Dictionary<string, List<string>> specialShortPhrases = new Dictionary<string, List<string>>();
        public static List<string> specialShortPhrasesRecognized = new List<string>();
        List<WordUnit> infoWords = new List<WordUnit>();
        Dictionary<string, AOI> AOIdic = new Dictionary<string, AOI>();

        // Deprecated:
        public static List<string> specialWordsRecognized = new List<string>();
        public static Dictionary<string, string> singleWords = new Dictionary<string, string>();
        // End

        public Bitmap[] imgs;
        public string path = "";
        private TextFile textFile = new TextFile();
        private string[] wordsFromFile;
        public Color ButtonColor = new Color();
        int RL_padding, UD_padding;
        int minium_phrase_len;
        string imageName;

        // cancel s as input
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
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
        // every click create a new list of special sentences
        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
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
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] filePaths = Directory.GetFiles(fbd.SelectedPath);
                    Bitmap[] bitmaps = new Bitmap[filePaths.Length];
                    System.Drawing.Size size = new System.Drawing.Size(1398, 1082);
                    var i = 0;
                    foreach (var path in filePaths)
                    {
                        try
                        {
                            using (var bmp = (Bitmap)Image.FromFile(path))
                            {
                                bitmaps[i] = ImageService.ResizeImage(bmp, size);
                                i += 1;
                            }

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Images uploading failed");
                            return;
                        }
                    }
                    textFile.TextImages = bitmaps;
                    imgs = bitmaps;
                }
            }
        }
        private void upload_word_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Word Documents|*.docx", ValidateNames = true, Multiselect = false };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    textFile.WordFile = file;
                    wordsFromFile = textFile.PagesText;
                }
            }
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            // test mode
            /*
            string test1 = "elizabeth with questions no graph";
            string test2 = "elizabeth no graph";
            string currTest = test1;
            using (FileStream file = new FileStream(currTest + ".docx", FileMode.Open, FileAccess.Read))
            {
                textFile.WordFile = file;
                wordsFromFile = textFile.PagesText;
            }
            */
            // end test
            status_lbl.Text = "Loading...";
            this.Refresh();
            if (imgs == null)
            {
                MessageBox.Show("You must upload images");
                status_lbl.Text = "";
                return;
            }
            var ocr = new TesseractEngine(path, "heb", EngineMode.Default);
            Tesseract.PageIteratorLevel myLevel = PageIteratorLevel.Word;
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output");


            // dictionary for the frequncies and temporary for shorst phrase recognition 
            Dictionary<string, int> wordsMap = new Dictionary<string, int>();

            int aoiNumber = 1;
            int aoiGroup = 1;

            for (int indexPage = 1; indexPage <= imgs.Length; indexPage++)
            {
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
                                
                                WordUnit wordUnit = new WordUnit(wordUnitName, rect.X1, rect.Y1, rect.X2, rect.Y2, rect.Height, rect.Width);
                                wordUnit.updateEndSign(curText);


                                // wordMap is for the frequency
                                if (!wordsMap.ContainsKey(wordUnitName))
                                {
                                    if (wordUnitName == " " || wordUnitName == "")
                                        continue;
                                    wordsMap.Add(wordUnitName, 0);
                                }

                                infoWords.Add(wordUnit);

                            }
                        } while (iter.Next(myLevel));

                        //
                        Thread t = new Thread(() => FrequencyWord(wordsMap, infoWords, this.imageName + $"{indexPage}"));
                        if (words_freq_cb.Checked)
                            t.Start();
                        //

                        List<int> limits = new List<int>();
                        var upperBound = int.MaxValue;
                        var lowerBound = 0;

                        for (int f = 0; f <= infoWords.Count - 1; f++)
                        {

                            if (f == infoWords.Count - 1)
                            {
                                limits.Add(upperBound);
                                limits.Add(lowerBound);
                                break;
                            }
                            if (AreSameLine(infoWords[f], infoWords[f+1])) // same line
                            {
                                if (infoWords[f].Y1 < upperBound)
                                {
                                    upperBound = infoWords[f].Y1;
                                }
                                if (infoWords[f].Y2 > lowerBound)
                                {
                                    lowerBound = infoWords[f].Y2;
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
                        for (int i = 0; i <= infoWords.Count - 1; i++)
                        {
                            if ((i != infoWords.Count - 1) && AreSameLine(infoWords[i], infoWords[i+1]))
                            {

                                var diff = infoWords[i].X1 - infoWords[i + 1].X2;
                                var gap_for_padding = diff / 2;
                                infoWords[i].X1 -= gap_for_padding;
                                infoWords[i + 1].X2 += (gap_for_padding - 1);
                                infoWords[i].Y1 = gaps[gap_num];

                            }
                            else
                            {
                                infoWords[i].Y1 = gaps[gap_num];
                                //add the right left padding 
                                infoWords[i].X1 -= this.RL_padding;
                                infoWords[first_in_line].X2 += this.RL_padding;
                                last_in_line = i;
                                gap_num++;
                                if (i == infoWords.Count - 1)
                                    last_line = 1;
                                for (int j = first_in_line; j <= last_in_line; j++)
                                {
                                    if (first_line == 1)
                                        infoWords[j].Y1 -= this.UD_padding;

                                    infoWords[j].Y2 = gaps[gap_num];

                                    if (last_line == 1)
                                        infoWords[j].Y2 += this.UD_padding;
                                }
                                first_line = 0;
                                first_in_line = i + 1;
                                gap_num++;
                            }
                        }

                        FileCreators.createPaddingFile(new WordPaddingBuilder(new List<WordUnit>(infoWords)), this.imageName + $"{indexPage}");


                        //should be deleted
                        Dictionary<int, string> firstPerId = new Dictionary<int, string>();
                        var num = 0;
                        foreach (List<string> values in specialSentences.Values)
                        {
                            firstPerId.Add(num, values[0]);
                            num++;
                        }

                        AoiAlgorithm(ref aoiNumber, ref aoiGroup);


                        foreach (KeyValuePair<string, int> item in wordsMap)
                        {
                            string word = item.Key;
                            foreach (KeyValuePair<string, List<string>> shortPhrase in specialShortPhrases)
                            {
                                // if the first word in the phrase is equal, add to recognition
                                if (word.Contains(shortPhrase.Value[0]))
                                {
                                    specialShortPhrasesRecognized.Add(shortPhrase.Key);
                                }
                            }

                        }

                        FileCreators.createPaddingFile(new AOIPaddingBuilder(new List<AOI>(AOIdic.Values), imageName), $"aois {indexPage}");



                        if (separate_xml_cb.Checked == false)
                        {
                            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\For SMI");
                            string path = @"\Output\For SMI\" + this.imageName + $"{indexPage}" + ".xml";
                            using (StreamWriter xmlFile = new StreamWriter(Environment.CurrentDirectory + path, false, Encoding.UTF8))
                            {
                                var id_aoi = 1;
                                xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                                xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                                foreach (WordUnit word in infoWords)
                                {
                                    var x1_value = word.X1;
                                    var y1_value = word.Y1;
                                    var x2_value = word.X2;
                                    var y2_value = word.Y2;
                                    CreateDynamicAoiXml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, word.Name, word.Name, Constants.NO_SENTENCE, false);
                                    id_aoi += 1;
                                }
                                foreach (string key in AOIdic.Keys)
                                {
                                    var x1_value = AOIdic[key].X1;
                                    var y1_value = AOIdic[key].Y1;
                                    var x2_value = AOIdic[key].X2;
                                    var y2_value = AOIdic[key].Y2;
                                    CreateDynamicAoiXml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, key,
                                        AOIdic[key].Group, Constants.SENTENCE, AOIdic[key].IsSpecial);
                                    id_aoi++;
                                }
                                xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                                xmlFile.Close();
                            }
                        }
                        else
                        {
                            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\Output\For SMI");
                            string path = @"\Output\For SMI\" + this.imageName + $"{indexPage}" + "_allWords.xml";
                            using (StreamWriter xmlFile = new StreamWriter(Environment.CurrentDirectory + path, false, Encoding.UTF8))
                            {
                                var id_aoi = 1;
                                xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                                xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                                foreach (WordUnit word in infoWords)
                                {
                                    var x1_value = word.X1;
                                    var y1_value = word.Y1;
                                    var x2_value = word.X2;
                                    var y2_value = word.Y2;
                                    CreateDynamicAoiXml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, word.Name, word.Name, Constants.NO_SENTENCE, false);
                                    id_aoi += 1;
                                }
                                xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                                xmlFile.Close();
                            }

                            string path_special = @"\Output\For SMI\" + this.imageName + $"{indexPage}" + "_special.xml";
                            using (StreamWriter xmlFile = new StreamWriter(Environment.CurrentDirectory + path_special, false, Encoding.UTF8))
                            {
                                var id_aoi = 1;
                                xmlFile.WriteLine("<?xml version=\"1.0\"?>");
                                xmlFile.WriteLine("<ArrayOfDynamicAOI xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                                foreach (string key in AOIdic.Keys)
                                {
                                    var x1_value = AOIdic[key].X1;
                                    var y1_value = AOIdic[key].Y1;
                                    var x2_value = AOIdic[key].X2;
                                    var y2_value = AOIdic[key].Y2;
                                    CreateDynamicAoiXml(id_aoi, x1_value, y1_value, x2_value, y2_value, xmlFile, key,
                                        AOIdic[key].Group, Constants.SENTENCE, AOIdic[key].IsSpecial);
                                    id_aoi++;
                                }
                                xmlFile.WriteLine("</ArrayOfDynamicAOI>");
                                xmlFile.Close();
                            }

                        }

                        if (words_freq_cb.Checked)
                            t.Join();
                        else
                        {
                            FileCreators.CreateResultsFile(wordsMap, infoWords, this.imageName + $"{indexPage}");
                        }
                        //t.Join();
                        // without the thread: creating the results without tal libzon

                        status_lbl.Text = $"{indexPage} " + "out of " + $"{imgs.Length}";
                        ClearData();
                        //in order to test the first page:
                        //break;
                    }


                }

            }
            MessageBox.Show("Finished");
            status_lbl.Text = "";

        }

        private Dictionary<string, List<string>> StartSpecialSentenceDic(WordUnit word)
        {
            Dictionary<string, List<string>> candide = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, List<string>> entry in specialSentences)
            {
                if (word.Name == entry.Value[0])
                    candide.Add(entry.Key, entry.Value);
            }
            return candide;
        }
        public static void SetWords(Dictionary<string, string> singles)
        {
            singleWords = singles;
        }
        public static void SetSentences(Dictionary<string, List<string>> sentences, Dictionary<string, List<string>> shortUnits)
        {
            specialSentences = sentences;
            specialShortPhrases = shortUnits;
        }
        public static void FrequencyWord(Dictionary<string, int> wordsMap, List<WordUnit> info_words, string name_image)
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
            FileCreators.CreateResultsFile(wordsMap, info_words, name_image);

        }



        public void CreateAOI(int key, int group, bool isSpecial, string specialName, int x1, int y1, int x2, int y2)
        {
            AOI aoi = new AOI();
            aoi.Name = key.ToString();
            aoi.Group = group.ToString();
            aoi.X1 = x1;
            aoi.X2 = x2;
            aoi.Y1 = y1;
            aoi.Y2 = y2;
            aoi.Height = y2 - y1;
            aoi.Width = x2 - x1;
            aoi.IsSpecial = isSpecial;
            if (aoi.IsSpecial)
                aoi.SpecialName = specialName;
            AOIdic.Add(aoi.Name, aoi);
        }
        public void CreateDynamicAoiXml(int id_aoi, int x1_value, int y1_value, int x2_value, int y2_value,
            StreamWriter xmlFile, string name, string group, int with_sentence, bool isSpecial)
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
                if (singleWords.ContainsKey(name))
                {
                    specialWordsRecognized.Add(name);
                    name_aoi = singleWords[name];
                }
            }
            else
            {
                name_aoi = name;
            }

            if (isSpecial)
            {
                xmlFile.WriteLine("    <Color>NamedColor:Coral</Color>");
            }
            else if (!isSpecial)
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

        private void AoiAlgorithm(ref int aoiNumber, ref int aoiGroup) {
            // boolean var, if the word is in the sentences_list
            var inSpecialSentence = false;
            var x2 = 0;
            var y2 = 0;
            var x1_prev = 0;
            var y1_prev = 0;
            var parts = false;
            var phase_counter = 0;
            var custom_key = "";
            Dictionary<string, List<string>> candide = new Dictionary<string, List<string>>();

            for (int i = 0; i < infoWords.Count; i++)
            {   
                WordUnit word = infoWords[i];
                x2 = word.X2;
                y2 = word.Y2;
                candide = StartSpecialSentenceDic(word);

                if (candide.Count > 0)
                {
                    inSpecialSentence = true;
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
                                inSpecialSentence = false;
                                if (parts)
                                {
                                    AOIdic.Remove(aoiNumber.ToString());
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
                                    specialSentencesRecognized.Add(entry.Key);
                                    CreateAOI(aoiNumber, aoiGroup, inSpecialSentence, entry.Key, word.X1, word.Y1, x2, y2);
                                    inSpecialSentence = false;
                                    aoiNumber += 1;
                                    aoiGroup += 1;
                                    sentence_recognized = true;
                                    break;
                                }
                                x1_prev = word.X1;
                                y1_prev = word.Y1;
                            }
                            // if the current word in the special sentence is in a new line, create AOI for the 
                            // sentence until that word
                            else
                            {
                                CreateAOI(aoiNumber, aoiGroup, inSpecialSentence, entry.Key, x1_prev, y1_prev, x2, y2);
                                // new section of the AOI
                                parts = true;
                                aoiNumber += 1;
                                x2 = word.X2;
                                y2 = word.Y2;
                                x1_prev = word.X1;
                                y1_prev = word.Y1;

                                // if the current word is the last in the special sentence
                                if (match == entry.Value.Count) 
                                {
                                    CreateAOI(aoiNumber, aoiGroup, inSpecialSentence, entry.Key, word.X1, word.Y1, word.X2, word.Y2);
                                    specialSentencesRecognized.Add(entry.Key);
                                    sentence_recognized = true;
                                    inSpecialSentence = false;
                                    aoiNumber += 1;
                                    aoiGroup += 1;
                                    break;
                                }
                            }
                            i += 1;
                            if (i == infoWords.Count)
                                break;
                            word = infoWords[i];
                        }
                        if (sentence_recognized)
                            break;
                        else
                        {
                            word = infoWords[start_index];
                            i = start_index;
                            match = 0;
                        }

                    }
                }
                else
                {
                    while (i < infoWords.Count)
                    {
                        word = infoWords[i];
                        x1_prev = word.X1;
                        y1_prev = word.Y1;
                        phase_counter += 1;
                        parts = false;
                        if ((word.EndWithPunctuation && phase_counter >= minium_phrase_len) ||
                            word.EndOfSentence ||
                             (i+1 < infoWords.Count && StartSpecialSentenceDic(infoWords[i + 1]).Count != 0))
                        {
                            phase_counter = 0;
                            break;
                        }

                        // different line with more than one word in the prev line
                        if (i+1 < infoWords.Count && word.Y1 != infoWords[i+1].Y1)
                        {
                            parts = true;
                            break;
                        }
                        i += 1;
                    }
                    CreateAOI(aoiNumber, aoiGroup, inSpecialSentence, null, word.X1, word.Y1, x2, y2);
                    aoiNumber += 1;
                    if (!parts)
                        aoiGroup += 1;
                }

            }
        }
        // TODO: test this method
        private void ClearData()
        {
            specialSentencesRecognized.Clear();
            specialShortPhrasesRecognized.Clear();
            infoWords.Clear();
            AOIdic.Clear();
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