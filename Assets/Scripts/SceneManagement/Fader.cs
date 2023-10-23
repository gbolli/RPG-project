using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] float fadeOut = 3f;
        [SerializeField] float fadeIn = 2f;

        CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeOutIn()); // testing
        }

        IEnumerator FadeOutIn() // nested coroutine for fade out then fade in
        {
            yield return FadeOut(fadeOut);
            yield return FadeIn(fadeIn);
        }

        IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;

                yield return null;  // wait for 1 frame
            }
        }

        IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;

                yield return null;  // wait for 1 frame
            }
        }
    }
}

