using System;
using PencilDurability.Utility;
using PencilDurability.Pencils;
using PencilDurability.Exceptions;
namespace PencilDurability
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            int pointValue;
            do
            {
                Console.WriteLine("Set pencil point value");
                var point = Console.ReadLine();
                //if point isn't numeric prompt to re-enter
                if (int.TryParse(point, out pointValue)) break;
            } while (true);
            int lengthValue;
            do
            {
                Console.WriteLine("Set pencil length value");
                //if length isn't numeric prompt to re-enter
                var length = Console.ReadLine();
                if (int.TryParse(length, out lengthValue)) break;
            } while (true);
            int durabilityValue;
            do
            {
                Console.WriteLine("Set pencil eraser durability");
                //if durability isn't numeric prompt to re-enter
                var durability = Console.ReadLine();
                if (int.TryParse(durability, out durabilityValue)) break;
            } while (true);
            Pencil pencil = new Pencil(pointValue, lengthValue, durabilityValue);
            WriterUtility writerUtility = new WriterUtility();
            var sheetOfPaper = writerUtility.GetASheetOfPaper();

            do
            {
                try
                {
                    Console.WriteLine("Please enter 'w' to write text, 'e' to erase text or 'd' to edit text. Then enter text to write, erase or edit.  Enter 'x' to exit.");
                    command = Console.ReadLine();
                    if (command == "w")
                    {
                        var text = Console.ReadLine();
                        var result = pencil.WriteToSheetOfPaper(text, sheetOfPaper);
                        sheetOfPaper.Text = result;
                        Console.WriteLine(result);
                    }
                    else if (command == "e")
                    {
                        try
                        {
                            var textToErase = Console.ReadLine();
                            var result = pencil.Eraser.Erase(sheetOfPaper.Text, textToErase);
                            sheetOfPaper.Text = result;
                            Console.WriteLine(result);
                        } catch (TextToEraseDoesNotExist t)
                        {
                            Console.WriteLine("Text to erase does not exist.");
                        } catch (CannotEraseDurabilityIsZeroException c)
                        {
                            Console.WriteLine("Cannot erase. Eraser durability is zero. Press any key to exit.");
                            Console.ReadLine();
                            break;
                        }
                    }
                    else if (command == "d")
                    {
                        var textToInsert = Console.ReadLine();
                         pencil.Edit(sheetOfPaper, textToInsert);
                        Console.WriteLine(sheetOfPaper.Text);
                    }
                    else if (command == "x")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter 'w' to write text, 'e' to erase text or 'd' to edit text. Then enter text to write, erase or edit.  Enter 'x' to exit.");
                    }
                } catch(PointHasDegradedToZeroException p)
                {
                    Console.WriteLine("Press 's' to sharpen pencil.");
                    command = Console.ReadLine();
                    if (command == "s")
                    {
                        try
                        {
                            pencil.Sharpen(pointValue);
                            Console.WriteLine($"Sheet of paper text is:  {sheetOfPaper.Text}");
                        }
                        catch (CannotSharpenPencilLengthZeroException e)
                        {
                            Console.WriteLine("Cannot sharpen pencil because it's length is zero. Press any key to exit.");
                            Console.ReadLine();
                            break;
                        }
                    }
                }
            } while (true);
        }
    }
}
