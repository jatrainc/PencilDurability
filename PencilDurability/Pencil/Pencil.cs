using System;
using PencilDurability.Paper;
using System.Collections.Generic;
namespace PencilDurability.Pencil
{
    public class Pencil
    {
        private const int lowerCaseLetterDegredationValue = 1;
        private const int upperCaseLetterDegredationValue = 2;
        private List<char> upperCaseLetters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private List<char> lowerCaseLetters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private List<char> spacesAndNewLines = new List<char> { ' ', '\n' };

        public Pencil(int pointValue, int lengthValue)
        {
            this.point = pointValue;
            this.length = lengthValue;
        }
        public int point { get; set; }
        public int length { get; set; }

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
                if (upperCaseLetters.Contains(c))
                {
                    point -= upperCaseLetterDegredationValue;
                } 
                else if (lowerCaseLetters.Contains(c))
                {
                    point -= lowerCaseLetterDegredationValue;
                } 
                else if (spacesAndNewLines.Contains(c))
                {
                    //no degredation
                }
            }
        }

        public void Sharpen(int pointValue)
        {
            point = pointValue;
        }

    }
}
