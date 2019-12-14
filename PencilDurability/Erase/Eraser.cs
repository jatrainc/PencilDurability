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
            var length = textToErase.Length > durability ? durability : textToErase.Length;
            var lastOccurrenceIndex = text.LastIndexOf(textToErase);
            var firstPartOfResult = text.Substring(0, lastOccurrenceIndex);
            var lastPartOfResultLength = text.Length - lastOccurrenceIndex - textToErase.Length;
            var lastPartOfResult = text.Substring(lastOccurrenceIndex + textToErase.Length, lastPartOfResultLength);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < textToErase.Length; i++)
            {
                sb.Append(" ");
            }
            var result = firstPartOfResult + sb.ToString() + lastPartOfResult;
            durability -= length;
            return result;
        }
    }
}
