using System;
using Novemo.Characters.Player;
using Novemo.StatusEffects.Buffs;
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

		private Player playerStats;

		public override void SetDescription()
		{
			itemDescription =
				$"Potion Type: {potionType}{Environment.NewLine}Potion Power: {potionPower}" +
				$"{Environment.NewLine}Potion Rate: {potionRate}" +
				$"{Environment.NewLine}Potion Time: {potionTime}";
		}

		public override bool Use()
		{
			base.Use();
			playerStats = PlayerManager.Instance.player.GetComponent<Player>();
			
			var potionBuff = new StatBuff {
				EffectName = potionType.ToString(),
				EffectDuration = potionTime, 
				EffectPower = potionPower, 
				Icon = itemIcon,
				TargetStats = playerStats
			};

			switch (potionType)
			{
				case PotionType.Vitality:
					potionBuff.EffectName = PotionType.Vitality.ToString();
					potionBuff.StatIndex = 0;
					break;
				case PotionType.Resistance:
					potionBuff.EffectName = PotionType.Resistance.ToString();
					potionBuff.StatIndex = 31;
					break;
				case PotionType.Strength:
					potionBuff.EffectName = PotionType.Strength.ToString();
					potionBuff.StatIndex = 32;
					break;
				case PotionType.Luck:
					potionBuff.EffectName = PotionType.Luck.ToString();
					potionBuff.StatIndex = 27;
					break;
				case PotionType.Speed:
					potionBuff.EffectName = PotionType.Speed.ToString();
					potionBuff.StatIndex = 6;
					break;
				case PotionType.Regeneration:
					var regenerationBuff = new RegenerateBuff {
						EffectName = potionType.ToString(),
						EffectDuration = potionTime,
						EffectPower = potionPower,
						EffectRate = potionRate,
						StatIndex = 28,
						TargetStats = playerStats
					};

					playerStats.ApplyStatusEffect(regenerationBuff);
					return true;
				case PotionType.Invisibility:
					//make player not visible to enemies and change sprite's alpha
					break;
				case PotionType.Heal:
					if (playerStats.CurrentHealth < playerStats.stats[0].GetValue())
					{
						playerStats.ModifyHealth(potionPower);
						return true;
					}
					else
					{
						return false;
					}
			}

			playerStats.ApplyStatusEffect(potionBuff);

			return true;
		}

		public override bool Equals(Item other)
		{
			if (ReferenceEquals(other, null)) {
				return false;
			}
			
			if (ReferenceEquals(this, other)) {
				return true;
			}
			
			if (GetType() != other.GetType()) {
				return false;
			}
			
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