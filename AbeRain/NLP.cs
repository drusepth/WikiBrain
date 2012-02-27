using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AbeRain
{
    class NLP
    {

        public static List<string> GetKeywords(string phrase)
        {
            List<string> keywords = new List<string>();
            string[] sentence_words = StripPunctuation(phrase).Split(' ');

            foreach (string word in sentence_words)
            {
                foreach (string k in Encyclopedia.ParentTopics(word))
                    keywords.Add(k);
            }

            // Remove duplicates
            //

            return keywords;
        }

        public static string StripPunctuation(string phrase)
        {
            return Regex.Replace(phrase, "[!?\\/.,`~()]", "");
        }

    }
}
