using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    public class TargetShortPhrase : ICloneable
    {
        public List<string> Phrase { get; set; }
        public int RecognitionCounter { get; set; }
        public TargetShortPhrase(List<string> p)
        {
            Phrase = p;
        }

        public object Clone()
        {
            TargetShortPhrase clone = new TargetShortPhrase(Phrase);
            clone.RecognitionCounter = RecognitionCounter;
            return clone;

        }
    }
}
