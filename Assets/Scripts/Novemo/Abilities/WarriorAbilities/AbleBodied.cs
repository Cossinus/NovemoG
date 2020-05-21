using System;
using Novemo.Combat;
using Novemo.Status_Effects;
using Novemo.Status_Effects.Buffs;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
	public class AbleBodied : Ability
	{
		private StatusEffect _statBuff;
		
		private CharacterCombat _combat;
		private bool _activated;

		private void Update()
		{
			abilityDelay -= Time.deltaTime;

			if (!playerStats.CanUseSpells) Disable();
			
			if (Input.GetButtonDown("Spell3") && abilityDelay <= 0f && !isActive && playerStats.CanUseSpells)
			{
				abilityUseTime = DateTime.UtcNow;
				
				playerStats.GetComponent<CharacterCombat>().OnAttack += Active;
				
				_statBuff = new StatBuff()
				{
					StatIndex = 99,
					EffectPower = 20f,
					EffectName = $"{ability.abilityName}Mitigate",
					TargetStats = playerStats
				};
				
				playerStats.ApplyStatusEffect(_statBuff);
				
				isActive = true;
			}
		}

		protected override void Active()
		{
			base.Active();
			
			if (!isActive) return;
			//change animation to follow up description

			var cost = playerStats.GetScaledValueByMultiplier(0, 0.01f);

			if (playerStats.CurrentHealth > cost + 1)
			{
				playerStats.ModifyHealth(-cost);
			}
			else
			{
				Disable();
			}
		}

		public override void Disable()
		{
			playerStats.GetComponent<CharacterCombat>().OnAttack -= Active;
			isActive = false;
			_statBuff.RemoveEffect();
		}
	}
}