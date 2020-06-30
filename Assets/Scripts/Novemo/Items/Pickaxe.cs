using System;
using UnityEngine;

namespace Novemo.Items
{
	[CreateAssetMenu(fileName = "New Pickaxe", menuName = "Items/Pickaxe")]
	public class Pickaxe : Item
	{
		public int durability;
		public int currentDurability;

		public Item head;
		public Item handle;

		public Hardness hardness;

		[Multiline] public string pickDescription;

		public void OnValidate()
		{
			stackLimit = 1;
			
			SetDescription();
		}
		
		public override void SetDescription()
		{
			itemDescription =
				$"{pickDescription}{Environment.NewLine}<color=#00FFFF>Hardness: {hardness}{Environment.NewLine}Durability: {currentDurability}/{durability}</color>";
		}
	}
}