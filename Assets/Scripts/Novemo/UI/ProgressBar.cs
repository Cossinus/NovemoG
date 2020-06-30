using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.UI
{
	public class ProgressBar : MonoBehaviour
	{
		public Image progressBar;
		public Image fillImage;

		private Coroutine _coroutine;
		
		public void FillSlider(float duration)
		{
			if (_coroutine != null) StopCoroutine(_coroutine);
			_coroutine = StartCoroutine(FillSliderCoroutine(duration));
		}

		public void DisableSlider()
		{
			progressBar.gameObject.SetActive(false);
			StopCoroutine(_coroutine);
		}
		
		private IEnumerator FillSliderCoroutine(float duration)
		{
			progressBar.gameObject.SetActive(true);
			fillImage.fillAmount = 0;
			
			var rate = 1.0f / duration;
			var progress = 0.0f;
			
			while (progress < 1.0f)
			{
				fillImage.fillAmount += rate * Time.deltaTime;

				progress += rate * Time.deltaTime;
				yield return null;
			}
			
			progressBar.gameObject.SetActive(false);
		}
	}
}