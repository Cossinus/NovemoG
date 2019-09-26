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
        }

        public bool CheckForEffect(CharacterStats characterStats)
        {
            if ((int) regenType == 0 && characterStats.CurrentHealth < characterStats.stats[0].GetValue() && !IsRegenerating)
                return true;
            if ((int) regenType == 1 && characterStats.CurrentMana < characterStats.stats[1].GetValue() && !IsRegenerating)
                return true;

            return false;
        }

        public override IEnumerator Passive(CharacterStats characterStats)
        {
            if ((int) regenType == 0 && characterStats.CurrentHealth < characterStats.stats[0].GetValue() - 1)
            {
                characterStats.CurrentHealth += regenPower;
                characterStats.OnHealthChangeInvoke();
                IsRegenerating = true;
                yield return new WaitForSeconds(regenRate);
            }
            if ((int) regenType == 1 && characterStats.CurrentMana < characterStats.stats[1].GetValue() - 1)
            {
                characterStats.CurrentMana += regenPower;
                characterStats.OnManaChangeInvoke();
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
