using System;
using Novemo.Status_Effects;
using Novemo.Status_Effects.Buffs;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
    public class ThickSkin : Ability
    {
        private StatusEffect _mitigateEffect;

        private void Awake()
        {
            ability = Resources.Load<AbilityObject>("Abilities/Warrior/ThickSkin");
        }
        
        private void Update()
        {
            abilityDelay -= Time.deltaTime;

            if (!playerStats.CanUseSpells) Disable();

            if (abilityUseTime.AddSeconds(ability.abilityDuration[abilityLevel]) < DateTime.UtcNow && isActive) Disable();
            
            if (Input.GetButtonDown("Spell2") && abilityDelay <= 0f && playerStats.CurrentMana > ability.abilityCost[abilityLevel] && !isActive && playerStats.CanUseSpells)
            {
                _mitigateEffect = new MitigateEffect
                {
                    EffectPower = 40f,
                    EffectName = $"{ability.abilityName}Mitigate",
                    EffectRate = ability.abilityStack[abilityLevel],
                    EffectDuration = ability.abilityDuration[abilityLevel],
                    TargetStats = playerStats
                };
                
                abilityUseTime = DateTime.UtcNow;
                
                playerStats.ApplyStatusEffect(_mitigateEffect);
                
                playerStats.ModifyMana(-ability.abilityCost[abilityLevel]);

                isActive = true;
            }
        }

        public override void Disable()
        {
            _mitigateEffect.RemoveEffect();
            isActive = false;
        }

        public override void Passive(Characters.Character characterStats)
        {
            // Some passive effect
        }
    }
}