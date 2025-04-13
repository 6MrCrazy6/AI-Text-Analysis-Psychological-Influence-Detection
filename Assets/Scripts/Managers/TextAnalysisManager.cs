using System;
using UnityEngine;
using AnalysisScripts;
using TMPro;
using System.Text.RegularExpressions;
using AIScripts;

namespace Managers
{
    public class TextAnalysisManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI totalWords;

        private SentimentAnalyzer sentiment;
        private SubjectivityAnalyzer subjectivity;
        private TextClassifier classifier;

        private string label;
        private string conclusion;
        private double sentimentScore;
        private double manipulativeRatio;
        private double lexicalDiversity;
        private double subjectivityScore;

        void Start()
        {
            sentiment = new SentimentAnalyzer();
            subjectivity = new SubjectivityAnalyzer();
            classifier = new TextClassifier();
        }

        public void AnalyzeText()
        {
            Debug.Log("AnalyzeText() called!");

            string text = inputField.text.ToLower();
            int wordCount = CountWords(text);
            totalWords.text = $"Total words: {wordCount}";

            manipulativeRatio = ManipulativeWordAnalysis.CalculateManipulativeWordRatio(text);
            lexicalDiversity = LexicalDiversityAnalysis.CalculateLexicalDiversity(text);

            var result = sentiment.CalculateSentimentScore(text, manipulativeRatio);
            sentimentScore = result.FinalSentiment;
            subjectivityScore = subjectivity.CalculateSubjectivity(text);

            (label, conclusion) = classifier.PredictText(sentimentScore, manipulativeRatio, lexicalDiversity, subjectivityScore);
        }

        private int CountWords(string text)
        {
            string cleanedText = Regex.Replace(text, @"[^\w\s-]", "");
            string[] words = cleanedText.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        public string GetLabel()
        {
            return label;
        }

        public string GetConclusion()
        {
            return conclusion;
        }

        public float GetSentimentScore()
        {
            return (float)sentimentScore;
        }

        public float GetManipulativeRatio()
        {
            return (float)manipulativeRatio;
        }

        public float GetLexicalDiversity()
        {
            return (float)lexicalDiversity;
        }

        public float GetSubjectivityScore()
        {
            return (float)subjectivityScore;
        }
    }
}
