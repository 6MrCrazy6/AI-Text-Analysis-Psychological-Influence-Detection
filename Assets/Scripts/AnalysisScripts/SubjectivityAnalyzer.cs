using System;
using System.IO;
using System.Text.RegularExpressions;
using Python.Runtime;
using UnityEngine;

namespace AnalysisScripts
{
    public class SubjectivityAnalyzer
    {
        private PyObject textBlob;

        public SubjectivityAnalyzer()
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

        public double CalculateSubjectivity(string text)
        {
            text = Regex.Replace(text, @"[^\w\s.!?]", "").ToLower().Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                Debug.LogError("Input text is empty!");
                return 0.0;
            }

            double totalSubjectivity = 0.0;
            int sentenceCount = 0;

            string[] sentences = text.Split(new char[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            using (Py.GIL())
            {
                PyObject TextBlob = textBlob.GetAttr("TextBlob");

                foreach (string sentence in sentences)
                {
                    if (sentence.Trim().Length > 1)
                    {
                        PyObject blob = TextBlob.Invoke(new PyString(sentence));
                        PyObject sentiment = blob.GetAttr("sentiment");
                        double subjectivity = sentiment.GetAttr("subjectivity").As<double>();

                        subjectivity = double.IsNaN(subjectivity) ? 0.0 : subjectivity;

                        totalSubjectivity += subjectivity;
                        sentenceCount++;
                    }
                }
            }

            double finalSubjectivity = sentenceCount > 0 ? totalSubjectivity / sentenceCount : 0.0;
            finalSubjectivity = Math.Max(0, Math.Min(finalSubjectivity, 1));

            Debug.Log($"Subjectivity Score: {finalSubjectivity}");

            return Math.Round(finalSubjectivity, 2);
        }
    }

}
