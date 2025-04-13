using UnityEngine;
using UnityEngine.SceneManagement;
using Ricimi;
using DG.Tweening;
using System.Collections;

namespace UI
{
    public class ButtonAnimation : MonoBehaviour
    {
        private bool isClicked = false;
        private CleanButton button;

        public CanvasGroup mainMenuCanvas;
        public CanvasGroup aboutCanvas;
        public GameObject closeButton;

        private void Start()
        {
            button = GetComponent<CleanButton>();

            aboutCanvas.alpha = 0;
        }

        public void OnButtonClick()
        {
            if (isClicked) return;
            isClicked = true;

            string buttonName = gameObject.name;
            button.interactable = false;

            transform.DOScale(0.9f, 0.1f).OnComplete(() =>
            {
                transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    if (buttonName == "StartButton")
                    {
                        StartCoroutine(LoadSceneAsync("Program"));
                    }
                    else if (buttonName == "ExitButton")
                    {
                        Application.Quit();
                    }
                    else if (buttonName == "AboutButton")
                    {
                        ShowCanvas(aboutCanvas, closeButton);
                    }
                    else if (buttonName == "CloseButton")
                    {
                        HideCanvas(aboutCanvas, closeButton);
                    }

                    isClicked = false;
                    button.interactable = true;
                });
            });
        }

        private void ShowCanvas(CanvasGroup canvas, GameObject button)
        {
            if (mainMenuCanvas != null)
            {
                mainMenuCanvas.DOFade(0, 0.2f).OnComplete(() =>
                {
                    mainMenuCanvas.interactable = false;
                    mainMenuCanvas.blocksRaycasts = false;
                    mainMenuCanvas.gameObject.SetActive(false);
                });
            }

            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
                canvas.DOFade(1f, 0.3f).OnComplete(() =>
                {
                    canvas.interactable = true;
                    canvas.blocksRaycasts = true;
                });
            }

            if (button != null) button.SetActive(true);
        }

        private void HideCanvas(CanvasGroup canvas, GameObject button)
        {
            if (canvas != null)
            {
                canvas.DOFade(0f, 0.2f).OnComplete(() =>
                {
                    canvas.gameObject.SetActive(false);
                    canvas.interactable = false;
                    canvas.blocksRaycasts = false;

                    if (mainMenuCanvas != null)
                    {
                        mainMenuCanvas.gameObject.SetActive(true);
                        mainMenuCanvas.DOFade(1f, 0.3f).OnComplete(() =>
                        {
                            mainMenuCanvas.interactable = true;
                            mainMenuCanvas.blocksRaycasts = true;
                        });
                    }
                });
            }

            if (button != null) button.SetActive(false);
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            asyncLoad.allowSceneActivation = true;
        }

    }
}
