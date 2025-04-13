using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace AnalysisScripts
{
    public class ManipulativeWordAnalysis : MonoBehaviour
    {
        private static HashSet<string> manipulativeWords = new HashSet<string>();
        private static List<string> manipulativePhrases = new List<string>();

        private string filePath;

        async void Start()
        {
            filePath = Path.Combine(Application.dataPath, "Resources/manipulative_words.json");
            await LoadManipulativeWordsAsync();
        }

        private async Task LoadManipulativeWordsAsync()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                string jsonContent = await File.ReadAllTextAsync(filePath);
                var wordData = JsonConvert.DeserializeObject<ManipulativeWordsWrapper>(jsonContent);

                manipulativeWords.Clear();
                manipulativePhrases.Clear();

                AddWordsFromCategory(wordData.generalization);
                AddWordsFromCategory(wordData.call_to_action);
                AddWordsFromCategory(wordData.emotional_triggers);
                AddWordsFromCategory(wordData.fear_based);
                AddWordsFromCategory(wordData.manipulative_contrasts);
                AddWordsFromCategory(wordData.false_analogies);
                AddWordsFromCategory(wordData.concept_substitution);
                AddWordsFromCategory(wordData.appeal_to_patriotism);
                AddWordsFromCategory(wordData.doubt_and_uncertainty);
                AddWordsFromCategory(wordData.urgency_and_alarmism);

                Debug.Log($"Loaded: {manipulativeWords.Count} words, {manipulativePhrases.Count} phrases.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading manipulative words: {ex.Message}");
            }
        }

        private void AddWordsFromCategory(List<string> words)
        {
            if (words == null) return;

            foreach (var phrase in words)
            {
                if (phrase.Contains(" "))
                    manipulativePhrases.Add(phrase.ToLower());
                else
                    manipulativeWords.Add(phrase.ToLower());
            }
        }

        public static double CalculateManipulativeWordRatio(string text)
        {
            if (manipulativeWords.Count == 0 && manipulativePhrases.Count == 0)
            {
                return 0;
            }

            string cleanedText = Regex.Replace(text, @"[^\w\s-]", "");
            string[] words = cleanedText.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            int manipulativeCount = 0;
            HashSet<string> countedWords = new HashSet<string>();
            HashSet<string> countedPhrases = new HashSet<string>();

            foreach (var word in words)
            {
                if (word.Length > 3 && manipulativeWords.Contains(word) && !countedWords.Contains(word))
                {
                    manipulativeCount++;
                    countedWords.Add(word);
                }
            }

            foreach (var phrase in manipulativePhrases)
            {
                string[] phraseWords = phrase.Split(' ');

                int matchCount = phraseWords.Count(w => words.Contains(w));
                double matchRatio = (double)matchCount / phraseWords.Length;

                if (matchRatio >= 0.65 && !countedPhrases.Contains(phrase))
                {
                    manipulativeCount++;
                    countedPhrases.Add(phrase);
                }
            }

            Debug.Log($"Total words: {words.Length}, Unique manipulative words/phrases found: {manipulativeCount}");

            return words.Length > 0 ? Math.Round((double)manipulativeCount / words.Length, 2) : 0;
        }
    }

    [Serializable]
    public class ManipulativeWordsWrapper
    {
        public List<string> generalization;
        public List<string> call_to_action;
        public List<string> emotional_triggers;
        public List<string> fear_based;
        public List<string> manipulative_contrasts;
        public List<string> false_analogies;
        public List<string> concept_substitution;
        public List<string> appeal_to_patriotism;
        public List<string> doubt_and_uncertainty;
        public List<string> urgency_and_alarmism;
    }
}
