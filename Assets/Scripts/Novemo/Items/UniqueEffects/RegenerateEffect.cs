using System;
using System.Collections;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu (menuName = "EquipmentEffects/PassiveEffects/RegenerateEffect")]
    public class RegenerateEffect : PassiveEffect
    {
        public int regenPower;
        public float regenRate;

        public RegenerateType rType;
        
        private CharacterStats _targetStats;

        public override IEnumerator Passive()
        {
            IsRegenerating = true;
            
            _targetStats = PlayerManager.Instance.player.GetComponent<CharacterStats>();

            if (rType == RegenerateType.Health)
            {
                if (_targetStats.CurrentHealth < _targetStats.stats[0].GetValue())
                {
                    _targetStats.CurrentHealth += regenPower;
                    Debug.Log("Regenerated: " + regenPower + "\nHealth: " + _targetStats.CurrentHealth);
                    yield return new WaitForSecondsRealtime(regenRate);
                }
            }
            else
            {
                if (_targetStats.CurrentHealth < _targetStats.stats[0].GetValue())
                {
                    _targetStats.CurrentMana += regenPower;
                    Debug.Log("Regenerated: " + regenPower + "\nMana: " + _targetStats.CurrentMana);
                    yield return new WaitForSecondsRealtime(regenRate);
                }
            }

            IsRegenerating = false;
        }

        public enum RegenerateType
        {
            Health, Mana
        }
    }
}
