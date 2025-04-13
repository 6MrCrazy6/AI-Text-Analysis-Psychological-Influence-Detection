using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LoadingManager : MonoBehaviour
    {
        public Image[] whiteCircles; 
        public Image[] darkCircles;   
        public float animationSpeed = 0.5f; 

        private Coroutine loadingCoroutine;

        public void StartLoading()
        {
            gameObject.SetActive(true);
            if (loadingCoroutine == null)
            {
                loadingCoroutine = StartCoroutine(AnimateSpinner());
            }
        }

        public void StopLoading()
        {
            if (loadingCoroutine != null)
            {
                StopCoroutine(loadingCoroutine);
                loadingCoroutine = null;
            }

            ResetCircles();
            gameObject.SetActive(false);
        }

        private IEnumerator AnimateSpinner()
        {
            int index = 0;
            while (true)
            {
                ResetCircles();

                Color whiteColor = whiteCircles[index].color;
                whiteColor.a = 1f;
                whiteCircles[index].color = whiteColor;

                Color darkColor = darkCircles[index].color;
                darkColor.a = 1f;
                darkCircles[index].color = darkColor;

                yield return new WaitForSeconds(animationSpeed);

                index = (index + 1) % whiteCircles.Length;
            }
        }

        private void ResetCircles()
        {
            foreach (Image circle in whiteCircles)
            {
                Color color = circle.color;
                color.a = 0.3f;
                circle.color = color;
            }

            foreach (Image circle in darkCircles)
            {
                Color color = circle.color;
                color.a = 0.3f;
                circle.color = color;
            }
        }
    }
}

