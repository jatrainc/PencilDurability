using System;

namespace PencilDurability.Erase
{
    public class Eraser
    {
        public Eraser(int durabilityValue)
        {
            this.durablility = durabilityValue;
        }
        public int durablility { get; set; }
        public string Erase(string text, string textToErase)
        {
            var result = text.Remove(text.LastIndexOf(textToErase), textToErase.Length);
            durablility -= textToErase.Length;
            return result;
        }
    }
}
