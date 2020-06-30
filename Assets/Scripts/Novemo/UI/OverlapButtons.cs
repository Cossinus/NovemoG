using System;
using Novemo.Inventories;
using UnityEngine;

namespace Novemo.UI
{
	public class OverlapButtons : MonoBehaviour
	{
		public CanvasGroup statsObject;
		public CanvasGroup craftObject;
		public CanvasGroup questObject;

		private static InventoryManager _inventoryManager;
		
		private void Start()
		{
			_inventoryManager = InventoryManager.Instance;
		}

		public void OpenStatsObject()
		{
			StartCoroutine(_inventoryManager.FadeIn(statsObject));
			StartCoroutine(_inventoryManager.FadeOut(craftObject));
			StartCoroutine(_inventoryManager.FadeOut(questObject));
		}

		public void OpenCraftObject()
		{
			StartCoroutine(_inventoryManager.FadeOut(statsObject));
			StartCoroutine(_inventoryManager.FadeIn(craftObject));
			StartCoroutine(_inventoryManager.FadeOut(questObject));
		}

		public void OpenQuestObject()
		{
			StartCoroutine(_inventoryManager.FadeOut(statsObject));
			StartCoroutine(_inventoryManager.FadeOut(craftObject));
			StartCoroutine(_inventoryManager.FadeIn(questObject));
		}
	}
}
