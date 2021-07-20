using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tesseract_OCR.Services;

namespace Tesseract_OCR
{
    public class TextFile
    {
        private FileStream wordFilePath;
        private Bitmap[] images;
        private string[] pagesText;
        int numberOfPages;

        public FileStream WordFile
        {
            set
            {
                wordFilePath = value;
                SetPages();
            }
        }
        public Bitmap[] TextImages { get; set; }

        // Update the number of pages in the word file and initialize the text of pages
        private void SetPages()
        {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordFile = app.Documents.Open(wordFilePath.Name);
            numberOfPages = wordFile.Range().Information[WdInformation.wdNumberOfPagesInDocument];
            Microsoft.Office.Interop.Word.Document[] pages = new Microsoft.Office.Interop.Word.Document[numberOfPages];
            pagesText = new string[numberOfPages];
            int pageStart = 0;

            for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
            {
                var page = wordFile.Range(pageStart);
                if (currentPageIndex < numberOfPages)
                {
                    page.End = page.GoTo(
                        What: WdGoToItem.wdGoToPage,
                        Which: WdGoToDirection.wdGoToAbsolute,
                        Count: currentPageIndex + 1
                    ).Start - 1;
                }
                else
                {
                    page.End = wordFile.Range().End;
                }
                pageStart = page.End + 1;
                page.Copy();
                pages[currentPageIndex - 1] = app.Documents.Add();
                pages[currentPageIndex - 1].Range().Paste();
                // clean string to be without \n \r \t etc.
                pagesText[currentPageIndex - 1] = StringService.CleanString(pages[currentPageIndex - 1].Content.Text);
            }
            
            // in word file, header and footer are the same in all the pages
            List<string> headers = new List<string>();
            List<string> footers = new List<string>();
            foreach (Section aSection in app.ActiveDocument.Sections)
            {
                foreach (HeaderFooter aHeader in aSection.Headers)
                    headers.Add(aHeader.Range.Text);
                foreach (HeaderFooter aFooter in aSection.Footers)
                    footers.Add(aFooter.Range.Text);
            }
            string header = StringService.CleanString(StringService.ConcatList(headers, ""));
            string footer = StringService.CleanString(StringService.ConcatList(footers, ""));
            for (int i = 0; i < numberOfPages; i++)
            {
                pagesText[i] = header + pagesText[i] + footer;
            }
            wordFile.Close(false);
            app.Quit(false);
        }

        public string[] PagesText
        {
            get { return pagesText; }
        }
        public string GetText(int pageNumber)
        {
            if (pagesText.Length > pageNumber)
                return pagesText[pageNumber];
            return null;
        }


    }
}
