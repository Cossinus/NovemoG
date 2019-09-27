using System.Collections;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu (menuName = "EquipmentEffects/PassiveEffects/RegenerateEffect")]
    public class RegenerateEffect : PassiveEffect
    {
        public int regenPower;
        public float regenRate;

        public RegenerateType regenType;

        private void OnEnable()
        {
            IsRegenerating = false;
            Mitigated = false;
            Bolted = false;
            Thorned = false;
            Blazed = false;
        }

        public bool CheckForEffect(PlayerStats playerStats)
        {
            return (playerStats.CurrentHealth < playerStats.stats[0].GetValue() ||
                    playerStats.CurrentMana < playerStats.stats[1].GetValue()) && !IsRegenerating;
        }

        public override IEnumerator Passive(PlayerStats playerStats)
        {
            var intRegen = (int) regenType;
            if (intRegen == 0 && playerStats.CurrentHealth < playerStats.stats[0].GetValue())
            {
                playerStats.CurrentHealth += regenPower;
                playerStats.OnHealthChangeInvoke();
                IsRegenerating = true;
                yield return new WaitForSeconds(regenRate);
            }
            if (intRegen == 1 && playerStats.CurrentMana < playerStats.stats[1].GetValue())
            {
                playerStats.CurrentMana += regenPower;
                playerStats.OnManaChangeInvoke();
                IsRegenerating = true;
                yield return new WaitForSeconds(regenRate);
            }

            IsRegenerating = false;
        }

        public enum RegenerateType
        {
            Health, Mana
        }
    }
}
