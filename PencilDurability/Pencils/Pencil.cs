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
            Point = pointValue;
            Length = lengthValue;
            eraser = new Eraser(eraserDurability);
        }
        public int Point { get; set; }
        public int Length { get; set; }
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
                if (Point == 0) 
                {
                    throw new PointHasDegradedToZeroException();
                } else
                {
                    sheetOfPaper.Text += c.ToString();
                }
                if (upperCaseLetters.Contains(c))
                {
                    Point -= upperCaseLetterDegredationValue;
                } 
                else if (lowerCaseLetters.Contains(c))
                {
                    Point -= lowerCaseLetterDegredationValue;
                } 
                else if (spacesAndNewLines.Contains(c))
                {
                    //no degredation
                }
            }
        }

        public void Sharpen(int pointValue)
        {
            if (Length == 0) throw new CannotSharpenPencilLengthZeroException();
            Point = pointValue;
            Length -= 1;
        }

        public void Erase(string text, string textToErase)
        {
            eraser.Erase(text, textToErase);
        }

        public void Edit(SheetOfPaper sheetOfPaper, string textToInsert)
        {
            if (Point == 0) throw new PointHasDegradedToZeroException(); 
            var firstOccurrenceOfTwoSpaces = sheetOfPaper.Text.IndexOf("  ");
            if (firstOccurrenceOfTwoSpaces < 1) {
                sheetOfPaper.Text = sheetOfPaper.Text + textToInsert;
                return;
            }
            var textToAlterWithInsert = sheetOfPaper.Text.Substring(firstOccurrenceOfTwoSpaces, textToInsert.Length + 1);
            var textBeforeInsert = sheetOfPaper.Text.Substring(0, firstOccurrenceOfTwoSpaces);
            var fromEndOfInsertToEndOfString = sheetOfPaper.Text.Length - (firstOccurrenceOfTwoSpaces + 1 + textToInsert.Length);
            var textAfterInsert = sheetOfPaper.Text.Substring(firstOccurrenceOfTwoSpaces + 1 + textToInsert.Length, fromEndOfInsertToEndOfString);

            var texToAlterLoopCounter = 0;
            bool firstPass = true;
            var textToInsertCharray = textToInsert.ToCharArray();
            sheetOfPaper.Text = textBeforeInsert;
            foreach (var ch in textToAlterWithInsert)
            {
                if (Point == 0)
                {
                    var remainderOfTextToInsert = textToAlterWithInsert.Substring(texToAlterLoopCounter, (textToAlterWithInsert.Length - texToAlterLoopCounter));
                    sheetOfPaper.Text += remainderOfTextToInsert;
                    sheetOfPaper.Text += textAfterInsert;
                    throw new PointHasDegradedToZeroException();
                }
                if (ch == ' ' && firstPass)
                {
                    sheetOfPaper.Text += ch.ToString();
                    firstPass = false;
                    //adding a space so no need to degrade point value
                    continue;
                }
                else if (ch == ' ')
                {
                    sheetOfPaper.Text += textToInsertCharray[texToAlterLoopCounter].ToString();
                    Point--;
                }
                else
                {
                    sheetOfPaper.Text += "@";
                    Point--;
                }
                texToAlterLoopCounter++;
            }
            sheetOfPaper.Text += textAfterInsert;
            return;
        }
    }
}
