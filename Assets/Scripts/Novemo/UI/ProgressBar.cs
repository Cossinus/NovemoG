using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.UI
{
	public class ProgressBar : MonoBehaviour
	{
		public Image progressBar;
		public Image fillImage;

		private float _duration;

		private Coroutine _coroutine;
		
		public void FillSlider(float duration)
		{
			if (_coroutine != null) StopCoroutine(_coroutine);
			_duration = duration;
			_coroutine = StartCoroutine(FillSlider());
		}

		public void DisableSlider()
		{
			progressBar.gameObject.SetActive(false);
			StopCoroutine(_coroutine);
		}
		
		private IEnumerator FillSlider()
		{
			progressBar.gameObject.SetActive(true);
			fillImage.fillAmount = 0;
			
			var rate = 1.0f / _duration;
			var progress = 0.0f;
			
			while (progress < 1.0)
			{
				fillImage.fillAmount += rate * Time.deltaTime;

				progress += rate * Time.deltaTime;
				yield return null;
			}
			
			progressBar.gameObject.SetActive(false);
		}
	}
}