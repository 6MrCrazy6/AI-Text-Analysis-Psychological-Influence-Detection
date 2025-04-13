using DG.Tweening;
using Managers;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ProgramUI : MonoBehaviour
    {
        public GameObject UIProgramCanvas;
        public GameObject LoadingCanvas;
        public GameObject ResultsCanvas;

        public TextMeshProUGUI resultText;

        public TextAnalysisManager textAnalysisManager;

        private string label = null;
        private string conclusion = null;
        private float sentiment = 0;
        private float manipulative = 0;
        private float lexical = 0;
        private float subjectivity = 0;

        public void StartAnalysis()
        {
            UIProgramCanvas.SetActive(false);
            LoadingCanvas.SetActive(true);
            LoadingCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

            StartCoroutine(WaitForAnalysisResults());
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }


        public void SaveResults()
        {
            string savePath = Path.Combine(Application.dataPath, "SavedResults");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string fileName = $"AnalysisResult_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string filePath = Path.Combine(savePath, fileName);

            string resultString = $"Text Mood: {label}\n\n" +
                                  $"Conclusion: {conclusion}\n\n" +
                                  $"Algorithm Results:\n" +
                                  $"- SentimentScore: {sentiment:F2}\n" +
                                  $"- ManipulativeWordRatio: {manipulative:F2}\n" +
                                  $"- LexicalDiversity: {lexical:F2}\n" +
                                  $"- SubjectivityScore: {subjectivity:F2}";

            File.WriteAllText(filePath, resultString);

            Debug.Log($"Results saved to {filePath}");
        }

        IEnumerator WaitForAnalysisResults()
        {
            textAnalysisManager.AnalyzeText();

            while (!IsDataValid())
            {
                label = textAnalysisManager.GetLabel();
                conclusion = textAnalysisManager.GetConclusion();
                sentiment = textAnalysisManager.GetSentimentScore();
                manipulative = textAnalysisManager.GetManipulativeRatio();
                lexical = textAnalysisManager.GetLexicalDiversity();
                subjectivity = textAnalysisManager.GetSubjectivityScore();

                yield return null;  
            }

            yield return new WaitForSeconds(5);

            SwitchToResults();
        }

        void SwitchToResults()
        {
            LoadingCanvas.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
            {
                LoadingCanvas.SetActive(false);

                ResultsCanvas.SetActive(true);
                ResultsCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

                string resultString = $"<b>Text Mood:</b>\n {label}\n\n" +
                                      $"<b>Conclusion:</b>\n {conclusion}\n\n" +
                                      $"<b>Algorithm Results:</b>\n" +
                                      $"- SentimentScore: {sentiment:F2}\n" +
                                      $"- ManipulativeWordRatio: {manipulative:F2}\n" +
                                      $"- LexicalDiversity: {lexical:F2}\n" +
                                      $"- SubjectivityScore: {subjectivity:F2}";

                resultText.text = resultString;
            });
        }
        bool IsDataValid()
        {
            return !string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(conclusion);
        }
    }

}
