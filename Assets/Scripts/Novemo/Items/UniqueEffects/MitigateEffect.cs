using System.Collections;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu (menuName = "EquipmentEffects/PassiveEffects/MitigateEffect")]
    public class MitigateEffect : PassiveEffect
    {
        public override bool EffectReady(CharacterStats targetStats)
        {
            return !Mitigated;
        }

        public override IEnumerator Passive(CharacterStats targetStats)
        {
            if (targetStats.GetLastDamage > 0)
            {
                var mitigate = targetStats.GetLastDamage * effectPower;
                targetStats.CurrentHealth += mitigate;
                targetStats.OnHealthChangeInvoke();
                targetStats.GetLastDamage = 0;
                
                Mitigated = true;
                yield return new WaitForSeconds(0.755f);
            }
            Mitigated = false;
        }

        public override void OnEnable()
        {
            Mitigated = false;
        }
    }
}