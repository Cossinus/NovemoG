using System;
using Novemo.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Novemo.UI
{
	public class EffectContainer : MonoBehaviour
	{
		private InventoryManager _inventoryManager;
		
		[HideInInspector]
		public EffectUI.EffectObject effectObject;
		
		private void Start() => _inventoryManager = InventoryManager.Instance;
		
		public void ShowTooltip()
		{
			_inventoryManager.toolTipObject.SetActive(true);

			var e = effectObject.effect;

			//TODO fancy this
			_inventoryManager.sizeTextObject.text =
				$"<sprite=\"{e.GetType().Name}\" index=0>{e.EffectName}{Environment.NewLine}" +
				$"Effect Power: <color=#bc3c21>{e.EffectPower}</color>, <color=#00CED1>{e.EffectMagicDamage}</color>{Environment.NewLine}";
			if (e.EffectRate > 0)
			{
				_inventoryManager.sizeTextObject.text += $"Effect Rate: {e.EffectRate}{Environment.NewLine}";
			}
			_inventoryManager.sizeTextObject.text += $"Effect Source: {e.SourceStats.name}";

			_inventoryManager.visualTextObject.text = _inventoryManager.sizeTextObject.text;
		}

		public void HideTooltip()
		{
			if (EventSystem.current.gameObject.CompareTag("Effect"))
				_inventoryManager.toolTipObject.SetActive(false);
		}
	}
}