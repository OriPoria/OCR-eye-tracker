using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_OCR
{
    // regular name of AOI is its aoi name (number)
    // if the AOI is targeted, there is also a special name
    public class AOI : BlockInfo
    {
        public string SpecialName;
        public string Group;
        public bool IsSpecial;

    }
}
