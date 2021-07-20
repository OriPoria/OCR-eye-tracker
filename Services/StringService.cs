using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tesseract_OCR.Services
{
    public class StringService
    {
        // removes \n, \t, \r from a string and reaplace the letter that breaks the נ letter
        public static string CleanString(string inputString)
        {
            string s = null;
            string output = Regex.Replace(inputString, @"\s+", " ");
            output = Regex.Replace(output, "ª", "נ");
            return output;
        }
        // remove punctuations include space except for: 
        // - if in the middle of the word unit 
        // ' sign
        public static string RemovePunctuation(string inputString)
        {
            inputString = Regex.Replace(inputString, "[^א-ת0-9a-zA-Z'-]", String.Empty);
            if (inputString.EndsWith("-"))
                inputString = inputString.Substring(0, inputString.Length - 1);
            return inputString;
        }
        public static string RemovePunctuationPhrase(string inputString)
        {
            string[] units = inputString.Split(' ');
            for (int i = 0; i < units.Length; i++)
                units[i] = RemovePunctuation(units[i]);
            return string.Join(" ", units);
        }
        public static int HammingDist(string str1, string str2)
        {
            int i = 0, count = 0;
            while (i < str1.Length)
            {
                char c1 = str1[i];
                char c2 = str2[i];
                if (str1[i] != str2[i])
                    count++;
                i++;
            }
            return count;
        }
        public static string ConcatList(List<string> list, string delimiter)
        {
            return string.Join(delimiter, list.ToArray());

        }
    }
}
