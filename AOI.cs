using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    // regular name of AOI is its aoi name (number)
    // if the AOI is targeted, there is also a special name
    public class AOI
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsTarget { get; set; }
        public string TargetName { get; set; }


    }
}
