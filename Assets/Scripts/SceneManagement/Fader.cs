using System.Collections;
using UnityEngine;

namespace RPG.Scenemanagement
{
	public class Fader : MonoBehaviour
	{
		CanvasGroup canvasGroup;
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 2f;

		private void Start() {
			canvasGroup = GetComponent<CanvasGroup>();
           
		}

		public void FadeOutImmediate() {
			canvasGroup.alpha = 1;
		}

		public IEnumerator FadeOut() {
			while (canvasGroup.alpha < 1) {
				canvasGroup.alpha += Mathf.Clamp01(Time.deltaTime / fadeOutTime);
				yield return null;
			}
		}

        public IEnumerator FadeIn()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeInTime;
                yield return null;
            }
		}
	}
}