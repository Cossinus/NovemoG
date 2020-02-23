using System.Collections;
using Novemo.Combat;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu(menuName = "EquipmentEffects/PassiveEffects/ThornsEffect")]
    public class ThornsEffect : PassiveEffect
    {
        private CharacterStats _targetStats;

        public override bool EffectReady(CharacterStats targetStats)
        {
            var enemy = PlayerManager.Instance.player.GetComponent<CharacterCombat>().enemyCurrentlyFightingWith;
            if (enemy != null)
                _targetStats = enemy.GetComponent<CharacterStats>();
            return true;
        }

        /*public IEnumerator GetEffect(CharacterCombat playerCombat)
        {
            _targetStats = playerCombat.enemyCurrentlyFightingWith.GetComponent<CharacterStats>();
            yield return new WaitForSeconds(0.1f);
        }*/

        public override IEnumerator Passive(CharacterStats targetStats)
        {
            if (_targetStats != null)
            {
                var thorn = targetStats.GetLastDamage * effectPower;
                _targetStats.TakeLethalDamage(thorn, 0);
                _targetStats.OnHealthChangeInvoke();
                targetStats.GetLastDamage = 0;
                yield return null;
            }
        }

        public override void OnEnable() { }
    }
}