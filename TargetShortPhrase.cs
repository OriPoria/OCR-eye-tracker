using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    public class TargetShortPhrase
    {
        public List<string> Phrase { get; set; }
        public int RecognitionCounter { get; set; }
        public TargetShortPhrase(List<string> p)
        {
            Phrase = p;
        }
    }
}
