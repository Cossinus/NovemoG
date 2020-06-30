using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static void CreateNewPotion(int baseLevel, bool guaranteedQuality)
		{
			var potion = ScriptableObject.CreateInstance<Potion>();

			potion.potionType = (PotionType) Random.Range(0, Enum.GetValues(typeof(PotionType)).Length);
			potion.itemType = ItemType.Potion;
			potion.itemSubType = ItemSubType.Potion;
			potion.itemName = $"{potion.potionType} Potion";
			potion.itemIcon = Resources.Load<Sprite>($"Sprites/Items/Potions/{potion.potionType}_Potion");
			potion.itemRarity = Metrics.CalculateRarity(guaranteedQuality);

			potion.level = Random.Range(baseLevel - 3, baseLevel + 3);
			potion.level = Mathf.Clamp(potion.level, 0, 999);
			
			potion.stackLimit = 3;

			//TODO test this while gameplay (on paper it is hard to predict whether it is balanced or not)
			var basePotionPower = potion.level;
			var minOffset = (float) basePotionPower / 10;
			var maxOffset = (float) basePotionPower / 5;
			var power = Random.Range((float) basePotionPower / 3 + minOffset, (float) basePotionPower / 3 + maxOffset) * 2;

			switch (potion.potionType)
			{
				case PotionType.Vitality:
					power /= 15;
					break;
				case PotionType.Resistance:
					power /= 3;
					break;
				case PotionType.Strength:
					power /= 3;
					break;
				case PotionType.Luck:
					power /= 15;
					break;
				case PotionType.Speed:
					power /= 15;
					break;
				case PotionType.Regeneration:
					potion.potionRate = (float)Math.Round(Random.Range(power / 1.1f, power), 2);
					break;
				case PotionType.Invisibility:
					break;
				case PotionType.Heal:
					power *= 2;
					power += potion.potionTime / 2;
					potion.potionTime = 0;
					break;
			}

			potion.potionPower = (float)Math.Round(ComputePowerWithRarity(power, potion.itemRarity), 2);
			
			potion.potionTime = potion.level * minOffset / maxOffset * 10;

			potion.value = Mathf.RoundToInt(potion.potionTime + potion.potionPower + potion.potionRate * 10 * (1 + potion.level / 7) * (1 + (float)potion.itemRarity / 4));

			potion.SetDescription();
			_inventory.AddItem(potion);
		}
	}
}