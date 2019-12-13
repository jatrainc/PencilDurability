using System;

namespace PencilDurability.Erase
{
    public class Eraser
    {
        public string Erase(string text, string textToErase)
        {
            var result = text.Remove(text.IndexOf(textToErase), textToErase.Length);
            return result;
        }
    }
}
