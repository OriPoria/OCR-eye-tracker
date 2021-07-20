using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    public abstract class BlockInfo
    {
        public string Name;
        public int X1;
        public int Y1;
        public int X2;
        public int Y2;
        public int Height;
        public int Width;
        public bool IsTarget;
        public string TargetName;

    }
}
