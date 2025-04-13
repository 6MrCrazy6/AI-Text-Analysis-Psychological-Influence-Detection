using System;
using System.IO;
using System.Text.RegularExpressions;
using Python.Runtime;
using UnityEngine;

namespace AnalysisScripts
{
    public class SentimentAnalyzer
    {
        private PyObject textBlob;

        public SentimentAnalyzer()
        {
            try
            {
                string pythonHome = Path.Combine(Application.streamingAssetsPath, "Python");
                string pythonLib = Path.Combine(pythonHome, "Lib");
                string pythonSitePackages = Path.Combine(pythonLib, "site-packages");

                Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("PYTHONPATH", pythonLib + ";" + pythonSitePackages, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", Path.Combine(pythonHome, "python311.dll"), EnvironmentVariableTarget.Process);

                PythonEngine.Initialize();
                Debug.Log("Python Engine initialized.");

                using (Py.GIL())
                {
                    PyObject sys = Py.Import("sys");
                    PyObject sysPath = sys.GetAttr("path");
                    sysPath.InvokeMethod("append", new PyString(pythonSitePackages));
                    Debug.Log($"Added path: {pythonSitePackages}");

                    textBlob = Py.Import("textblob");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Initialization error Python: {ex.Message}");
            }
        }

        public (double FinalSentiment, double VaderScore, double TextBlobScore) CalculateSentimentScore(string text, double manipulativeRatio)
        {
            text = Regex.Replace(text, @"[^\w\s.!?]", "").ToLower().Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                Debug.LogError("Input text is empty!");
                return (0.0, 0.0, 0.0);
            }

            double totalVader = 0.0;
            double totalTextBlob = 0.0;
            int sentenceCount = 0;

            string[] sentences = text.Split(new char[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            using (Py.GIL())
            {
                PyObject vader = Py.Import("vaderSentiment.vaderSentiment");
                PyObject SentimentIntensityAnalyzer = vader.GetAttr("SentimentIntensityAnalyzer");
                PyObject analyzer = SentimentIntensityAnalyzer.Invoke();
                PyObject TextBlob = textBlob.GetAttr("TextBlob");

                foreach (string sentence in sentences)
                {
                    if (sentence.Trim().Length > 1)
                    {
                        PyObject scores = analyzer.InvokeMethod("polarity_scores", new PyString(sentence));
                        double compound = scores.GetItem("compound").As<double>();
                        totalVader += compound;

                        PyObject blob = TextBlob.Invoke(new PyString(sentence));
                        PyObject sentiment = blob.GetAttr("sentiment");
                        double textBlobPolarity = sentiment.GetAttr("polarity").As<double>();

                        textBlobPolarity = double.IsNaN(textBlobPolarity) ? 0.0 : textBlobPolarity;

                        totalTextBlob += textBlobPolarity;
                        sentenceCount++;
                    }
                }
            }

            double vaderScore = sentenceCount > 0 ? totalVader / sentenceCount : 0.0;
            double textBlobScore = sentenceCount > 0 ? totalTextBlob / sentenceCount : 0.0;

            vaderScore = Math.Max(-1, Math.Min(vaderScore, 1));
            textBlobScore = Math.Max(-1, Math.Min(textBlobScore, 1));

            double finalSentiment = (vaderScore + textBlobScore) / 2;

            double penalty = manipulativeRatio > 0.5 ? (manipulativeRatio > 0.9 ? 0.15 : manipulativeRatio > 0.7 ? 0.1 : 0.05) : 0;
            finalSentiment -= penalty;

            finalSentiment = Math.Max(-1, Math.Min(finalSentiment, 1));

            Debug.Log($"Final Sentiment: {finalSentiment}, Vader: {vaderScore}, TextBlob: {textBlobScore}, ManipulativePenalty: -{penalty}");

            return (Math.Round(finalSentiment, 2), Math.Round(vaderScore, 2), Math.Round(textBlobScore, 2));
        }
    }
}
