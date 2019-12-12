using System;
using PencilDurability.Paper;
namespace PencilDurability.Pencil
{
    public class Pencil
    {
        private const int lowerCaseLetterDegredationValue = 1;
        private const int upperCaseLetterDegredationValue = 2;

        public Pencil(int pointValue)
        {
            this.point = pointValue;
        }
        public int point { get; set; }

        public string WriteToSheetOfPaper(string textToWrite, SheetOfPaper sheetOfPaper)
        {
            sheetOfPaper.Text += textToWrite;
            DegradePointValue(textToWrite);
            return sheetOfPaper.Text;
        }

        private void DegradePointValue(String text)
        {
            var carray = text.ToCharArray();
            foreach (var c in carray)
            {
                point -= lowerCaseLetterDegredationValue;
            }
        }

    }
}
