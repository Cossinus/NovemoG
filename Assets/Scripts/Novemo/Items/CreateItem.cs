using System;
using Novemo.Controllers;
using Novemo.Items.UniqueEffects;
using Novemo.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Novemo.Items
{
	public class CreateItem : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				CreateNewPotion(Random.Range(1,5), false);
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				CreateNewPotion(Random.Range(6,10), false);
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				CreateNewPotion(Random.Range(11,15), false);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				CreateNewPotion(Random.Range(16,20), false);
			}
		}

		public static void CreateNewPotion(int baseLevel, bool guaranteedQuality)
		{
			var potion = ScriptableObject.CreateInstance<Potion>();

			potion.potionType = (PotionType) Random.Range(0, Enum.GetValues(typeof(PotionType)).Length);
			potion.itemType = ItemType.Potion;
			potion.itemSubType = ItemSubType.Potion;
			potion.itemName = $"{potion.potionType} Potion";
			potion.stackLimit = 3;

			CalculateRarity(potion, baseLevel, guaranteedQuality);

			//TODO test this while gameplay (on paper it is hard to predict whether it is balanced or not)
			var basePotionPower = potion.level;
			var minOffset = (float) basePotionPower / 10;
			var maxOffset = (float) basePotionPower / 5;
			var power = Random.Range((float) basePotionPower / 3 + minOffset, (float) basePotionPower / 3 + maxOffset) * 2;

			switch (potion.potionType)
			{
				case PotionType.Vitality:
					power /= 15;
					potion.itemIcon = Resources.Load<Sprite>("");
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
					power /= 15;
					potion.potionRate = (float)Math.Round(Random.Range(power / 2, power), 2);
					break;
				case PotionType.Invisibility:
					break;
				case PotionType.Heal:
					power *= 2;
					power += potion.potionTime / 2;
					potion.potionTime = 0;
					break;
			}

			potion.potionPower = (float)Math.Round(CalculatePowerWithRarity(power, potion.itemRarity), 2);
			
			potion.potionTime = potion.level * minOffset / maxOffset * 10;

			potion.value = (int) Math.Round(potion.potionTime + potion.potionPower + potion.potionRate * 10 * (1 + potion.level / 7) * (1 + (float)potion.itemRarity / 4));

			potion.SetDescription();
			PlayerManager.Instance.player.GetComponent<PlayerController>().inventory.AddItem(potion);
		}

		public static void CreateNewScroll()
		{
			var scroll = ScriptableObject.CreateInstance<Scroll>();
		}

		public static void CreateNewUniqueEffect()
		{
			var effectType = (EffectType) Random.Range(0, Enum.GetValues(typeof(EffectType)).Length);

			if (effectType == EffectType.Passive)
			{
				var uniqueEffect = ScriptableObject.CreateInstance<PassiveEffect>();
				
				uniqueEffect.passiveType = (PassiveTypes) Random.Range(0, Enum.GetValues(typeof(PassiveTypes)).Length);

				/*switch (uniqueEffect.passiveType)
				{
					case PassiveTypes.Regenerate:
						break;
					case PassiveTypes.Thorns:
						break;
					case PassiveTypes.Blaze:
						break;
					case PassiveTypes.Bolt:
						break;
					case PassiveTypes.MitigateDamage:
						break;
					case PassiveTypes.StatBoost:
						break;
				}*/
			}
			else
			{
				var uniqueEffect = ScriptableObject.CreateInstance<ActiveEffect>();
				
				uniqueEffect.activeType = (ActiveTypes) Random.Range(0, Enum.GetValues(typeof(ActiveTypes)).Length);

				/*switch (uniqueEffect.activeType)
				{
					case ActiveTypes.DealDamage:
						break;
					case ActiveTypes.Heal:
						break;
					case ActiveTypes.SpeedUp:
						break;
					case ActiveTypes.Dash:
						break;
					case ActiveTypes.DamageBoost:
						break;
				}*/
			}
		}

		public static void CreateNewEquipment()
		{
			var equipment = ScriptableObject.CreateInstance<Equipment>();
		}

		private static float CalculatePowerWithRarity(float power, Rarity rarity)
		{
			switch (rarity)
			{
				case Rarity.Common:
					power *= 0.5f;
					break;
				case Rarity.Normal:
					power *= 0.7f;
					break;
				case Rarity.Uncommon:
					power *= 0.85f;
					break;
				case Rarity.Rare:
					power *= 0.95f;
					break;
				case Rarity.VeryRare:
					power *= 1.05f;
					break;
				case Rarity.Epic:
					power *= 1.13f;
					break;
				case Rarity.Legendary:
					power *= 1.21f;
					break;
				case Rarity.Mystical:
					power *= 1.32f;
					break;
				case Rarity.Artifact:
					power *= 1.5f;
					break;
			}
			return power;
		}
		
		private static void CalculateRarity(Item item, int baseLevel, bool guaranteedQuality)
		{
			var choose = new ChooseRarity();

			if (guaranteedQuality) {
				item.itemRarity = choose.Choose(true);
				item.level = Random.Range(baseLevel, baseLevel + 2);
			} else {
				item.itemRarity = choose.Choose(false);
				item.level = Random.Range(baseLevel - 2, baseLevel + 1);
			}
			
			if (item.level < 1)
			{
				item.level = 1;
			}
		}
	}
}