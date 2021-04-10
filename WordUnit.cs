
namespace Tesseract_OCR
{
    public class WordUnit : BlockInfo
    {
        public bool EndWithPunctuation = false;
        public bool EndOfSentence = false;

        public WordUnit(string name_word, int x1, int y1, int x2, int y2, int h, int w)
        {
            Name = name_word;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Height = h;
            Width = w;
        }

        public void updateEndSign(string OCRstring)
        {
            if (isEndOfSent(OCRstring))
                EndOfSentence = true;
            if (isEndWithPunc(OCRstring))
                EndWithPunctuation = true;
        }

        public static bool isEndWithPunc(string s)
        {
            if (s.EndsWith(",") || s.EndsWith("-") || s.EndsWith(";"))
                return true;
            return false;
        }
        public static bool isEndOfSent(string s)
        {
            if (s.EndsWith(".") || s.EndsWith("!") || s.EndsWith("?"))
                return true;
            return false;
        }
    }
}
