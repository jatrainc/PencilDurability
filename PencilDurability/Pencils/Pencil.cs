using System;
using System.Text;
using PencilDurability.Paper;
using System.Collections.Generic;
using PencilDurability.Exceptions;
using PencilDurability.Erase;

namespace PencilDurability.Pencils
{
    public class Pencil
    {
        private const int lowerCaseLetterDegredationValue = 1;
        private const int upperCaseLetterDegredationValue = 2;
        private List<char> upperCaseLetters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private List<char> lowerCaseLetters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private List<char> spacesAndNewLines = new List<char> { ' ', '\n' };

        public Pencil(int pointValue, int lengthValue, int eraserDurability)
        {
            this.point = pointValue;
            this.length = lengthValue;
            eraser = new Eraser(eraserDurability);
        }
        public int point { get; set; }
        public int length { get; set; }
        public Eraser eraser { get; set; }

        public string WriteToSheetOfPaper(string textToWrite, SheetOfPaper sheetOfPaper)
        {
            DegradePointValue(sheetOfPaper, textToWrite);
            return sheetOfPaper.Text;
        }

        private void DegradePointValue(SheetOfPaper sheetOfPaper, String text)
        {
            var carray = text.ToCharArray();
            foreach (var c in carray)
            {
                if (point == 0) 
                {
                    throw new PointHasDegradedToZeroException();
                } else
                {
                    sheetOfPaper.Text += c.ToString();
                }
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
            if (length == 0) throw new CannotSharpenPencilLengthZeroException();
            point = pointValue;
            length -= 1;
        }

        public void Erase(string text, string textToErase)
        {
            eraser.Erase(text, textToErase);
        }

        public string Edit(string text, string textToInsert)
        {
            var firstOccurrenceOfTwoSpaces = text.IndexOf("  ");
            if (firstOccurrenceOfTwoSpaces < 1) return text + textToInsert;
            var textToAlterWithInsert = text.Substring(firstOccurrenceOfTwoSpaces, textToInsert.Length + 1);
            var textBeforeInsert = text.Substring(0, firstOccurrenceOfTwoSpaces);
            var fromEndOfInsertToEndOfString = text.Length - (firstOccurrenceOfTwoSpaces + 1 + textToInsert.Length);
            var textAfterInsert = text.Substring(firstOccurrenceOfTwoSpaces + 1 + textToInsert.Length, fromEndOfInsertToEndOfString);

            var textToAlterResult = new StringBuilder();
            var texToAlterLoopCounter = 0;
            bool firstPass = true;
            var textToInsertCharray = textToInsert.ToCharArray();
            foreach (var ch in textToAlterWithInsert)
            {
                if (ch == ' ' && firstPass)
                {
                    textToAlterResult.Append(" ");
                    firstPass = false;
                    continue;
                } 
                else if (ch == ' ')
                {
                    textToAlterResult.Append(textToInsertCharray[texToAlterLoopCounter].ToString());
                } 
                else
                {
                    textToAlterResult.Append("@");
                }
                texToAlterLoopCounter++;
            }
            var result = textBeforeInsert + textToAlterResult.ToString() + textAfterInsert;
            
            return result;
        }
    }
}
