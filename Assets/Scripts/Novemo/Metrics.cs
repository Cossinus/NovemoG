using System;
using UnityEngine;

namespace Novemo
{
	public class Metrics
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

		public static bool EqualFloats(double value1, double value2, double acceptableDifference) => Math.Abs(value1 - value2) < acceptableDifference;
		
		public static bool CritChance(float critChance)
		{
			return UnityEngine.Random.Range(0, 100) <= critChance;
		}
		
		public static Vector3 GetRandomPosition(Transform target)
		{
			var angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

			return target.position - new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
		}
	}
}
