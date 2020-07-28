using System;
using System.Collections.Generic;

namespace ZacksLibrary
{
    public class ExtraMethods
    {
        public static string TitleString(string phrase)
        {
            string[] words = phrase.Split();
            List<string> newPhrase = new List<string>();

            foreach (string word in words)
            {
                newPhrase.Add(char.ToUpper(word[0]) + word.Substring(1));
            }

            string titleString = string.Join(" ", newPhrase);

            return titleString;
        }
    }
}
