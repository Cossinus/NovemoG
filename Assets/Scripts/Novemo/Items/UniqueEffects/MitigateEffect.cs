using System.Collections;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu (menuName = "EquipmentEffects/PassiveEffects/MitigateEffect")]
    public class MitigateEffect : PassiveEffect
    {
        public float mitigatePercentage;

        public bool CheckForEffect()
        {
            return !Mitigated;
        }

        public override IEnumerator Passive(PlayerStats playerStats)
        {
            if (playerStats.GetLastDamage > 0)
            {
                var mitigate = playerStats.GetLastDamage * mitigatePercentage;
                playerStats.CurrentHealth += mitigate;
                playerStats.OnHealthChangeInvoke();
                playerStats.GetLastDamage = 0;
                
                Mitigated = true;
                yield return new WaitForSeconds(0.755f);
            }
            
            Mitigated = false;
        }
    }
}