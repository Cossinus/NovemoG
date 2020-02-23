using Novemo.Player;
using Novemo.Stats;
using Novemo.Status_Effects.Buffs;
using UnityEngine;

namespace Novemo.Items
{
	[CreateAssetMenu(fileName = "New Potion", menuName = "Items/Potion")]
	public class Potion : Item
	{
		public PotionType potionType;
		
		public float potionPower;
		public float potionRate;
		public float potionTime;
		
		private PlayerStats playerStats;

		public override string SetDescription()
		{
			base.SetDescription();
			return itemDescription = $"{base.SetDescription()}Potion Type: {potionType}\nPotion Power: {potionPower}\nPotion Rate: {potionRate}\nPotion Time: {potionTime}\n<color=#C0C0C0>Level: {level}</color>";
		}

		public override bool Use()
		{
			base.Use();
			playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
			
			var potionBuff = new StatBuff {
				BuffName = potionType.ToString(),
				BuffDuration = potionTime, 
				BuffPower = potionPower, 
				CharacterStats = playerStats
			};

			switch (potionType)
			{
				case PotionType.Vitality:
					potionBuff.BuffName = PotionType.Vitality.ToString();
					potionBuff.StatIndex = 0;
					break;
				case PotionType.Resistance:
					potionBuff.BuffName = PotionType.Resistance.ToString();
					potionBuff.StatIndex = 98;
					break;
				case PotionType.Strength:
					potionBuff.BuffName = PotionType.Strength.ToString();
					potionBuff.StatIndex = 99;
					break;
				case PotionType.Luck:
					potionBuff.BuffName = PotionType.Luck.ToString();
					potionBuff.StatIndex = 27;
					break;
				case PotionType.Speed:
					potionBuff.BuffName = PotionType.Speed.ToString();
					potionBuff.StatIndex = 6;
					break;
				case PotionType.Regeneration:
					potionBuff.BuffName = PotionType.Regeneration.ToString();
					potionBuff.StatIndex = 7;
					
					var regenerationRateBuff = new StatBuff
					{
						BuffName = $"{PotionType.Regeneration.ToString()}Rate",
						BuffDuration = potionTime,
						BuffPower = potionRate,
						StatIndex = 28,
						CharacterStats = playerStats
					};

					playerStats.ApplyBuff(regenerationRateBuff);
					break;
				case PotionType.Invisibility:
					//make player not visible to enemies and change sprite's alpha
					break;
				case PotionType.Heal:
					if (playerStats.CurrentHealth < playerStats.stats[0].GetValue())
						playerStats.CurrentHealth += potionPower;
					else
						return false;
					break;
			}
			
			playerStats.ApplyBuff(potionBuff);

			return true;
		}

		public override bool Equals(Item other)
		{
			var otherPotion = (Potion) other;
			return base.Equals(other) && potionType == otherPotion.potionType && potionPower.Equals(otherPotion.potionPower) &&
			       potionRate.Equals(otherPotion.potionRate) && potionTime.Equals(otherPotion.potionTime);
		}
	}
}

public enum PotionType
{
	Vitality,
	Resistance,
	Strength,
	Luck,
	Speed,
	Regeneration,
	Invisibility,
	Heal
}