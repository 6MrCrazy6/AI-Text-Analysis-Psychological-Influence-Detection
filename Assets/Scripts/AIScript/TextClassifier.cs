using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Python.Runtime;
using UnityEngine;

namespace AIScripts
{
    public class TextClassifier
    {
        private PyObject predictFunction;

        public TextClassifier()
        {
            try
            {
                string pythonCodePath = Path.Combine(Application.streamingAssetsPath, "PythonCode");
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
                    sysPath.InvokeMethod("append", new PyString(pythonCodePath));

                    PyObject aiModule = Py.Import("ai_model");
                    predictFunction = aiModule.GetAttr("predict_text");

                    Debug.Log("Python function predict_text() loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error initializing Python: {ex.Message}");
            }
        }

        public (string, string) PredictText(double sentiment, double manipulative, double lexical, double subjectivity)
        {
            using (Py.GIL())
            {
                try
                {
                    if (predictFunction == null)
                    {
                        Debug.LogError("predictFunction is NULL. Python module might not be loaded.");
                        return ("Error", "Failed to process");
                    }

                    string jsonFilePath = Path.Combine(Application.persistentDataPath, "AnalysisResults.json");

                    var inputData = new
                    {
                        SentimentScore = sentiment,
                        ManipulativeWordRatio = manipulative,
                        LexicalDiversity = lexical,
                        SubjectivityScore = subjectivity
                    };

                    string jsonData = JsonConvert.SerializeObject(inputData, Formatting.Indented);

                    File.WriteAllText(jsonFilePath, jsonData, Encoding.UTF8);
                    Debug.Log($"JSON saved at: {jsonFilePath}");
                    Debug.Log($"JSON content: {jsonData}");

                    PyObject result = predictFunction.Invoke(new PyString(jsonFilePath));

                    if (result == null)
                    {
                        Debug.LogError("Python function returned NULL.");
                        return ("Error", "Python function failed");
                    }

                    string label = result[0].As<string>();
                    string conclusion = result[1].As<string>();

                    Debug.Log($"Label: {label}");
                    Debug.Log($"Conclusion: {conclusion}");

                    return (label, conclusion);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error when calling Python function: {ex.Message}");
                    return ("Error", "Failed to process");
                }
            }
        }
    }
}

