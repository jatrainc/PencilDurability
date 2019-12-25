using System.Text;
using PencilDurability.Exceptions;

namespace PencilDurability.Erase
{
    public class Eraser
    {
        public Eraser(int durabilityValue)
        {
            this.durability = durabilityValue;
        }
        public int durability { get; set; }
        public string Erase(string text, string textToErase)
        {
            if (durability == 0) throw new CannotEraseDurabilityIsZeroException();
            if (text.LastIndexOf(textToErase) < 0) throw new TextToEraseDoesNotExist();
            var length = textToErase.Length > durability ? durability : textToErase.Length;
            var lastOccurrenceIndex = text.LastIndexOf(textToErase);
            var firstPartOfResult = text.Substring(0, lastOccurrenceIndex);
            var lastPartOfResultLength = text.Length - lastOccurrenceIndex - textToErase.Length;
            var lastPartOfResult = text.Substring(lastOccurrenceIndex + textToErase.Length, lastPartOfResultLength);
            StringBuilder textErasedPart = new StringBuilder();
            var charray = textToErase.ToCharArray();
            int numberOfCharsThatCantBeErased = textToErase.Length - this.durability;
            //add any characters that can't be erased due to insufficient eraser durability
            for(int j = 0; j < numberOfCharsThatCantBeErased; j++)
            {
                textErasedPart.Append(charray[j].ToString());
            }
            //add the number of spaces represented by length with is durability if textToErase is greater than the durability
            for (int i = 0; i < length; i++)
            {
                textErasedPart.Append(" ");
            }
            var result = firstPartOfResult + textErasedPart.ToString() + lastPartOfResult;
            durability -= length;
            return result;
        }
    }
}
