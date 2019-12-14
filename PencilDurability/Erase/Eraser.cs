using System;
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
            var charrayOfSpaces = "   ";
            var result = text.Replace(textToErase, charrayOfSpaces);
            durability -= length;
            return result;
        }
    }
}
