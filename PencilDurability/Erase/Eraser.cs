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
            var result = text.Remove(text.LastIndexOf(textToErase), length);
            durability -= length;
            return result;
        }
    }
}
