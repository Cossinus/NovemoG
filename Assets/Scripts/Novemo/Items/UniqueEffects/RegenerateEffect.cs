using System.Collections;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu (menuName = "EquipmentEffects/PassiveEffects/RegenerateEffect")]
    public class RegenerateEffect : PassiveEffect
    {
        public RegenerateType regenType;

        public override bool EffectReady(CharacterStats targetStats)
        {
            return (targetStats.CurrentHealth < targetStats.stats[0].GetValue() && !Regenerating ||
                    targetStats.CurrentMana < targetStats.stats[1].GetValue()) && !Regenerating;
        }

        public override IEnumerator Passive(CharacterStats targetStats)
        {
            var intRegen = (int) regenType;
            if (intRegen == 0 && targetStats.CurrentHealth < targetStats.stats[0].GetValue())
            {
                targetStats.CurrentHealth += effectPower;
                targetStats.OnHealthChangeInvoke();
                Regenerating = true;
                yield return new WaitForSeconds(effectRate);
            }
            if (intRegen == 1 && targetStats.CurrentMana < targetStats.stats[1].GetValue())
            {
                targetStats.CurrentMana += effectPower;
                targetStats.OnManaChangeInvoke();
                Regenerating = true;
                yield return new WaitForSeconds(effectRate);
            }

            Regenerating = false;
        }

        public override void OnEnable()
        {
            Regenerating = false;
        }

        public enum RegenerateType
        {
            Health, Mana
        }
    }
}
