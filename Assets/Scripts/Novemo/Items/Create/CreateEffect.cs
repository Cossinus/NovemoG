using System;
using Novemo.StatusEffects;
using UnityEngine;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static StatusEffect CreateNewEffect<T>(float baseLevel, int statIndex, StatusEffect.EffectType effectType) where T : StatusEffect
		{
			/*var statusEffects = typeof(StatusEffect).Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(T)) && !t.BaseType.IsSubclassOf(typeof(T)) && !t.IsAbstract)
				.Select(t => (StatusEffect) Activator.CreateInstance(t)).ToList();

			var statusEffect = statusEffects[UnityEngine.Random.Range(0, statusEffects.Count)];*/
			
			var statusEffect = (T) Activator.CreateInstance(typeof(T));
			
			statusEffect.EType = effectType;
			statusEffect.EffectName = statusEffect.GetType().Name;
			statusEffect.StatIndex = statIndex > -1 ? statIndex : UnityEngine.Random.Range(0, 33);
			statusEffect.Icon = Resources.Load<Sprite>($"Sprites/Effects/{statusEffect.GetType().Name}");
			statusEffect.IsDebuff = statusEffect.GetType().FullName.Contains("Debuffs");

			switch (effectType)
			{
				case StatusEffect.EffectType.Passive:
					statusEffect.EffectPower = baseLevel + 10 * (1 + baseLevel / 10) / 100;
					statusEffect.EffectDuration = int.MaxValue;
					break;
				case StatusEffect.EffectType.Temporary:
					var powerTemp = baseLevel + 10 * (1 + baseLevel / 10);
					statusEffect.EffectDuration = 9f;
					statusEffect.EffectRate = UnityEngine.Random.Range(3, 5);
					SetMagicDamage(statusEffect, powerTemp);
					break;
				case StatusEffect.EffectType.Active:
					var powerActive = baseLevel + 30 * (1 + baseLevel / 10);
					statusEffect.EffectDuration = 90f;
					statusEffect.EffectRate = UnityEngine.Random.Range(3, 5);
					SetMagicDamage(statusEffect, powerActive);
					break;
				default:
					statusEffect.EffectDuration = 1f;
					break;
			}

			return statusEffect;
		}

		private static void SetMagicDamage(StatusEffect statusEffect, float power)
		{
			var isMagic = Metrics.NextBoolean(new System.Random());
			
			if (isMagic)
			{
				statusEffect.EffectPower = power / 3f;
				statusEffect.EffectMagicDamage = power;
			}
			else
			{
				statusEffect.EffectPower = power * 1.1f;
			}
		}
	}
}