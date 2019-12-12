using System;
using PencilDurability.Paper;
namespace PencilDurability.Pencil
{
    public class Pencil
    {

        public string WriteToSheetOfPaper(string textToWrite, SheetOfPaper sheetOfPaper)
        {
            sheetOfPaper.Text = textToWrite;
            return sheetOfPaper.Text;
        }
    }
}
