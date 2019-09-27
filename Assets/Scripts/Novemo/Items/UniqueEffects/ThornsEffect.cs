using System.Collections;
using Novemo.Controllers;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu(menuName = "EquipmentEffects/PassiveEffects/ThornsEffect")]
    public class ThornsEffect : PassiveEffect
    {
        private CharacterStats _targetStats;

        public float thornsPercentage;

        public IEnumerator GetEffect(CharacterCombat playerCombat)
        {
            _targetStats = playerCombat.enemyCurrentlyFightingWith.GetComponent<CharacterStats>();
            yield return new WaitForSeconds(0.1f);
        }

        public override IEnumerator Passive(PlayerStats playerStats)
        {
            if (_targetStats != null)
            {
                var thorn = playerStats.GetLastDamage * thornsPercentage;
                _targetStats.TakeLethalDamage(thorn, 0);
                _targetStats.OnHealthChangeInvoke();
                playerStats.GetLastDamage = 0;
                yield return null;
            }
        }
    }
}