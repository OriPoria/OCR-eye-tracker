﻿
namespace Tesseract_OCR
{
    public class WordUnit : BlockInfo
    {
        public bool EndWithPunctuation = false;
        public bool EndOfSentence = false;
        public int WordIndex;

        public WordUnit(string n, int x1, int y1, int x2, int y2, int h, int w)
        {
            // Name is the string of the word, for indexing there is WordIndex
            Name = n;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Height = h;
            Width = w;
        }

        public void SetEndSign(string OCRstring)
        {
            if (IsEndOfSentence(OCRstring))
                EndOfSentence = true;
            if (IsEndWithPunc(OCRstring))
                EndWithPunctuation = true;
        }

        public static bool IsEndWithPunc(string s)
        {
            if (s.EndsWith(",") || s.EndsWith("-") || s.EndsWith(";"))
                return true;
            return false;
        }
        public static bool IsEndOfSentence(string s)
        {
            if (s.EndsWith(".") || s.EndsWith("!") || s.EndsWith("?"))
                return true;
            return false;
        }
    }
}
