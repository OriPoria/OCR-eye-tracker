using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tesseract_OCR.Services
{
    public class WordsMatchService
    {
        private string[] wordsList;
        private int wordListPointer;
        private string correctWord;
        public WordsMatchService(string [] list)
        {
            wordsList = list;
            wordListPointer = 0;
        }
        public int WordListPointer
        {
            get { return wordListPointer; }
        }
        public string MatchWords(string ocrWord, int index)
        {
            if (index == 130)
            {
                var x = 3;
            }
            correctWord = ocrWord;
            wordListPointer = index;
            string listWord = index < wordsList.Length ? wordsList[index] : "";
            if (listWord != ocrWord)
            {
                if (AreSimilar(ocrWord, listWord))
                    correctWord = listWord;
                
                else if (Regex.IsMatch(listWord, "^[a-zA-Z0-9]*$") && listWord.Length > 0)
                    correctWord = listWord;

                else if (AreDifferent(ocrWord, listWord))
                {
                    string newWord;
                    if (index > 0)
                    {
                        newWord = wordsList[index - 1];
                        if (AreSimilar(ocrWord, newWord))
                        {
                            correctWord = newWord;
                            wordListPointer -= 1;
                        }
                            
                    }
                    if (index < wordsList.Length - 1)
                    {
                        newWord = wordsList[index + 1];
                        if (AreSimilar(ocrWord, newWord))
                        {
                            correctWord = newWord;
                            wordListPointer += 1;
                        }
                    }

                }
            }
            wordListPointer += 1;
            return correctWord;
        }
        private bool AreDifferent(string str1, string str2)
        {
            int minimumLen = Math.Min(str1.Length, str2.Length);
            double similarCharCounter = 0.0;
            for (int i = 0; i < minimumLen; i++)
            {
                if (str1[i] == str2[i])
                    similarCharCounter += 1;
            }
            if (similarCharCounter / minimumLen < 0.35)
                return true;
            return false;
        }
        private bool AreSimilar(string str1, string str2)
        {
            double similarCharCounter = 0.0;

            if (str1.Length == str2.Length)
            {
                similarCharCounter = str1.Length - StringService.HammingDist(str1, str2);
                if (str1.Length > 4 && (similarCharCounter / str1.Length) > 0.5)
                    return true;
                else if (str1.Length <= 4 && (similarCharCounter / str1.Length) >= 0.25)
                    return true;
            }
            else if (Math.Abs(str1.Length - str2.Length) == 1)
            {
                string shorterString = str1.Length > str2.Length ? str2 : str1;
                string longerString = str1.Length > str2.Length ? str1 : str2;
                for (int i = 0; i < 2; i++)
                {
                    string newLonger = longerString.Substring(i, shorterString.Length);
                    similarCharCounter = newLonger.Length - StringService.HammingDist(shorterString, newLonger);
                    if (similarCharCounter / newLonger.Length >= 0.55)
                        return true;
                }

                if (shorterString.Length > 3)
                {
                    if (shorterString[0] == longerString[0] && shorterString[1] == longerString[1]
                        && shorterString[shorterString.Length - 1] == longerString[longerString.Length - 1]
                        && shorterString[shorterString.Length - 2] == longerString[longerString.Length - 2])
                        return true;
                }
            }

            return false;
        }


    }
}
