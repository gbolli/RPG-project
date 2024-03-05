using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;

        private void Awake()   // Awake to grab reference before Start
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        private IEnumerator Fade(float target, float time) {
            // prevent conflict of 2 fades at the same time
            if (currentActiveFade != null) StopCoroutine(currentActiveFade);

            currentActiveFade = StartCoroutine(FadeRoutine(target, time));

            // waits until finished
            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time) {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;  // wait for 1 frame
            }
        }
    }
}

