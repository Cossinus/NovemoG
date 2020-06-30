using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Novemo.StatusEffects;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.UI
{
	public class EffectUI : MonoBehaviour
	{
		#region Singleton

		public static EffectUI Instance;
		
		private void Awake()
		{
			Instance = this;
		}

		#endregion

		public Transform buffParent;
		public Transform debuffParent;
		public Transform passiveParent;
		
		public GameObject effectPrefab;

		private List<EffectObject> effectObjects = new List<EffectObject>();

		public event Action<EffectObject> OnEffectLog;
		public event Action<EffectObject> OnPassiveLog;

		public void RaiseEffectLog(EffectObject effect) => OnEffectLog?.Invoke(effect);
		public void RaisePassiveLog(EffectObject effect) => OnPassiveLog?.Invoke(effect);
		
		private void Start()
		{
			OnEffectLog += OnEffectLogged;
			OnPassiveLog += OnPassiveLogged;
		}

		private void OnEffectLogged(EffectObject eObject)
		{
			eObject.effectObject = Instantiate(effectPrefab, eObject.effect.IsDebuff ? debuffParent : buffParent);
			eObject.effectObject.GetComponent<EffectContainer>().effectObject = eObject;

			SetNewObject(eObject);

			StartCoroutine(Fill(eObject, eObject.effect.EffectDuration));
		}

		//TODO add fill amount like in lol
		
		private void OnPassiveLogged(EffectObject eObject)
		{
			eObject.effectObject = Instantiate(effectPrefab, passiveParent);
			
			SetNewObject(eObject);
		}

		private void SetNewObject(EffectObject eObject)
		{
			effectObjects.Add(eObject);

			eObject.effectObject.transform.Find("Effect").GetComponent<Image>().sprite = eObject.effect.Icon;

			eObject.effectObject.transform.Find("BackgroundColor").GetComponent<Image>().color =
				eObject.effect.IsDebuff ? new Color(226, 0, 0) : new Color(226, 188, 0);

			effectObjects.OrderBy(e => e.effect.EffectDuration).ToList().Reverse();

			foreach (var effect in effectObjects)
			{
				effect.effectObject.transform.SetAsFirstSibling();
			}
		}

		private IEnumerator Fill(EffectObject eObject, float visibleTime)
		{
			var rate = 0f;
			try
			{
				rate = 1.0f / visibleTime;
			} catch { /*Ignored*/ }
			
			var progress = 0.0f;
			
			var duration = eObject.effectObject.transform.Find("Duration").GetComponent<Image>();
			
			while (progress < 1.0f)
			{
				duration.fillAmount += rate * Time.deltaTime;
				
				progress += rate * Time.deltaTime;

				yield return null;
			}
			
			DisableEffect(eObject);
		}
		
		public void DisableEffect(EffectObject eObject)
		{
			try
			{
				eObject.effectObject.GetComponent<EffectContainer>().HideTooltip();
				effectObjects.Remove(eObject);
				Destroy(eObject.effectObject);
			} catch { /*Ignored*/ }
		}

		public struct EffectObject
		{
			public GameObject effectObject;
			public StatusEffect effect;
		}
	}
}