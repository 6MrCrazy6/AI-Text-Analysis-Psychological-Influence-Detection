using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AnalysisScripts
{
    public class LexicalDiversityAnalysis : MonoBehaviour
    {
        public static double CalculateLexicalDiversity(string text)
        {
            text = Regex.Replace(text, @"[^\w\s]", "").ToLower();

            string[] words = text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> uniqueWords = new HashSet<string>(words);

            if (words.Length > 0)
            {
                return Math.Round((double)uniqueWords.Count / words.Length, 2);
            }
            else
            {
                return 0;
            }
        }
    }
}
