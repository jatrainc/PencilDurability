using System;
using PencilDurability.Paper;
namespace PencilDurability.Pencil
{
    public class Pencil
    {
        public int point { get; set; }

        public string WriteToSheetOfPaper(string textToWrite, SheetOfPaper sheetOfPaper)
        {
            sheetOfPaper.Text = sheetOfPaper.Text + textToWrite;
            return sheetOfPaper.Text;
        }
    }
}
