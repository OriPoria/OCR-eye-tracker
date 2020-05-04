using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    public class Word
    {
        public string name;
        public int X1;
        public int Y1;
        public int X2;
        public int Y2;
        public int Height;
        public int Width;

        public Word(string name_word,int x1, int y1,int x2,int y2,int h, int w)
        {
            name = name_word;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Height = h;
            Width = w;
        }

        //public int X1
        //{
        //    set
        //}
    }
}
