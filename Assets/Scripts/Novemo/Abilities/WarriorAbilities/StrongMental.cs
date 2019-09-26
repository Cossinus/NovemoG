using System.Collections;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
    public class StrongMental : Ability
    {
        public override void Start()
        {
            base.Start();
            
            abilityName = "Strong Mental";
            abilityDescription = "";
            
            abilityCost = myStats.Scale(0, .01f);
            cooldown = .7f;
            castTime = .05f;
        }

        void Update()
        {
            Delay -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.E))
                Use(abilityCost, cooldown);
            
            if (playerCombat.hasAttacked & isEnabled && myStats.CurrentHealth > abilityCost + 1)
                StartCoroutine(AttackCost(playerCombat.attackDelay));
        }

        protected override void Use(float cost, float cd)
        {
            if (Delay <= 0f && myStats.CurrentHealth > cost + 1)
            {
                StartCoroutine(AAbility(cd, "DamageThirdModifier"));

                Delay = cd - cd * myStats.stats[11].GetValue() / 100;
            }
        }

        public override IEnumerator AAbility(float cd, string statName)
        {
            yield return new WaitForSeconds(castTime);
            
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                myStats.scaleValues[statName] = myStats.Scale(2, .2f);
                myStats.stats[2].modifiers[statName] = myStats.scaleValues[statName];
            } 
            else
            {
                if (myStats.stats[2].modifiers.ContainsKey(statName))
                    myStats.stats[2].modifiers[statName] -= myStats.scaleValues[statName];
            }
            
            yield return new WaitForSeconds(cd);
        }

        private IEnumerator AttackCost(float attackDelay)
        {
            playerCombat.hasAttacked = false;
            yield return new WaitForSeconds(attackDelay);
            myStats.TakeDamage(0, 0, abilityCost, 0);
        }
    }
}