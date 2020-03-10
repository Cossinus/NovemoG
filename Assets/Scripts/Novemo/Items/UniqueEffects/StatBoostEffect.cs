using System.Collections;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu(menuName = "EquipmentEffects/PassiveEffects/StatBoostEffect")]
    public class StatBoostEffect : PassiveEffect
    {
        private CharacterStats _targetStats;
        
        public override bool EffectReady(CharacterStats targetStats)
        {
            _targetStats = targetStats;
            return true;
        }

        public override IEnumerator Passive(CharacterStats targetStats)
        {
            _targetStats.stats[statIndex].wholeModifierValue += effectPower;
            yield return null;
        }

        public void RemoveModifierValue()
        {
            _targetStats.stats[statIndex].wholeModifierValue -= effectPower;
        }

        public override void OnEnable()
        {
            Bolted = false;
            Blazed = false;
        }
    }
}