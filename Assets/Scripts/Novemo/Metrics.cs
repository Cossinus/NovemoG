using System;
using System.Linq;
using Novemo.Items;
using UnityEngine;

namespace Novemo
{
	public static class Metrics
	{
		public static readonly int[] RarityProbabilities = {
			750, 1300, 1775, 2075, 2175, 2225, 2245, 2249, 2250
		};
		
		public static bool NextBoolean(System.Random random)
		{
			return random.Next() > int.MaxValue / 2;
		}

		public static float GetCurrentFraction(bool health, Characters.Character target)
		{
			if (health)
				return target.CurrentHealth / target.stats[0].GetValue();
			
			return target.CurrentMana / target.stats[1].GetValue();
		}

		public static float PhysicalDamage(Characters.Character source, Characters.Character target, float damage, float armor, float penetration)
		{
			damage *= 1 - (armor - penetration) / (damage - armor - penetration);

			damage = ReduceDamage(target, damage);
			damage = BoostDamage(source, damage);

			if (CritChance(source.stats[30].GetValue()))
			{
				damage *= 2;
			}
			
			return (float) Math.Round(Mathf.Clamp(damage, 0, float.MaxValue), 2);
		}

		public static float MagicDamage(Characters.Character source, Characters.Character target, float damage, float magicResist, float penetration)
		{
			damage *= 1 - (magicResist - penetration) / (damage - magicResist - penetration);

			damage = ReduceDamage(target, damage);
			damage = BoostDamage(source, damage);
			
			if (CritChance(source.stats[30].GetValue()))
			{
				damage *= 2;
			}

			return (float) Math.Round(Mathf.Clamp(damage, 0, float.MaxValue), 2);
		}

		public static float ReduceDamage(Characters.Character stats, float damage)
		{
			if (stats.DamageReducePercentage > 0)
			{
				return damage * (1 - stats.DamageReducePercentage / 100);
			}

			return damage;
		}

		public static float BoostDamage(Characters.Character stats, float damage)
		{
			if (stats.DamageBoostPercentage > 0)
			{
				return damage * (1 + stats.DamageBoostPercentage / 100);
			}

			return damage;
		}

		public static bool EqualFloats(double value1, double value2, double acceptableDifference)
		{
			return Math.Abs(value1 - value2) < acceptableDifference;
		}

		public static bool CritChance(float critChance)
		{
			return UnityEngine.Random.Range(0, 100) <= critChance;
		}
		
		public static Vector3 GetRandomPosition(Transform target)
		{
			var angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

			return target.position - new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
		}
		
		public static Rarity CalculateRarity(bool guaranteedQuality)
		{
			var choose = new Randomizer();

			var probabilities = RarityProbabilities;
			
			if (guaranteedQuality) {
				return (Rarity) choose.ChooseInt(probabilities, probabilities.Last(), true);
			}

			return (Rarity) choose.ChooseInt(probabilities, probabilities.Last(), false);
		}

		public static string GetStringWithRarity(Rarity rarity, string stringToModify)
		{
			switch (rarity)
			{
				case Rarity.Common:
					return $"<color=#696969><size=40>{stringToModify}</size></color>"; //dark gray
				case Rarity.Normal:
					return $"<color=yellow><size=40>{stringToModify}</size></color>";
				case Rarity.Uncommon:
					return $"<color=#bfff00><size=40>{stringToModify}</size></color>"; //lime-ish
				case Rarity.Rare:
					return $"<color=#bc3c21><size=40>{stringToModify}</size></color>"; //brick-ish
				case Rarity.VeryRare:
					return $"<color=#00CED1><size=40>{stringToModify}</size></color>"; //cyan-ish
				case Rarity.Epic:
					return $"<color=orange><size=40><b>{stringToModify}</b></size></color>";
				case Rarity.Legendary:
					return $"<color=#ff00ff><size=40><b>{stringToModify}</b></size></color>"; //purple-ish
				case Rarity.Mystical:
					return $"<color=red><size=40><b>{stringToModify}</b></size></color>";
				case Rarity.Artifact:
					return $"<color=white><size=40><b>{stringToModify}</b></size></color>";
				default:
					return $"<color=#696969><size=40>{stringToModify}</size></color>";
			}
		}
	}
}
