using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR.ExcelCreators
{
    class WordAOIDetection
    {
        [Description("Target words")]
        public string TargetWord { get; set; }
        [Description("AOI name")]
        public string AOIName { get; set; }
        [Description("Detected")]
        public string Detected { get; set; }
    }
    class PhraseAOIDetection
    {
        [Description("Target phrases")]
        public string TargetPhrase { get; set; }
        [Description("AOI name")]
        public string AOIName { get; set; }
        [Description("Detected")]
        public string Detected { get; set; }
    }
    class OffTextDetection
    {
        [Description("Sentence")]
        public string Sentence { get; set; }
        [Description("Detected")]
        public string Detected { get; set; }

    }
    class WordBoundaries
    {
        [Description("Stimulus")]
        public string Stimulus { get; set; }
        [Description("Index")]
        public int Index { get; set; }
        [Description("Word")]
        public string Word { get; set; }
        [Description("X")]
        public int X { get; set; }
        [Description("Y")]
        public int Y { get; set; }
        [Description("H")]
        public int H{ get; set; }
        [Description("W")]
        public int W { get; set; }
        [Description("Target Name")]
        public string TargetName { get; set; }
        [Description("Length of Word")]
        public int LengthWord { get; set; }
        [Description("Frequency")]
        public int Frequency { get; set; }
    }

    class PhraseBoundaries
    {
        [Description("Stimulus")]
        public string Stimulus { get; set; }
        [Description("Name")]
        public string Name { get; set; }
        [Description("Group")]
        public string Group { get; set; }
        [Description("X")]
        public int X { get; set; }
        [Description("Y")]
        public int Y { get; set; }
        [Description("H")]
        public int H { get; set; }
        [Description("W")]
        public int W { get; set; }
        [Description("Target Name")]
        public string TargetName { get; set; }

    }

}
