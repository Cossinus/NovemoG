using System.Collections.Generic;
using Novemo.Combat;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    public class EffectController : MonoBehaviour
    {
        private CharacterStats _targetStats;
        private CharacterCombat _targetCombat;

        private void Start()
        {
            _targetStats = PlayerManager.Instance.player.GetComponent<CharacterStats>(); //change it for every target
            _targetCombat = GetComponent<CharacterCombat>();
            EquipmentManager.Instance.onEquipmentChanged += InitializePassiveEffects;
        }

        //TODO swap all hard coded values in effects with 'effectStrength' and 'effectRate'
        private void InitializePassiveEffects(Equipment newItem, Equipment oldItem)
        {
            // move this to item's namespace
            List<Coroutine> coroutines = new List<Coroutine>();
            
            if (newItem == null && oldItem.effects.Count > 0)
            {
                for (var i = 0; i < oldItem.effects.Count; i++)
                {
                    var passiveEffect = (PassiveEffect) oldItem.effects[i];
                    if (passiveEffect.effectType != EffectType.Passive) continue;

                    if (passiveEffect.passiveType != PassiveTypes.StatBoost) continue;

                    var statBoost = (StatBoostEffect) passiveEffect;
                    statBoost.RemoveModifierValue();
                    
                    //stop item's coroutines
                }
            }

            if (newItem == null || newItem.effects.Count == 0) return;

            for (var i = 0; i < newItem.effects.Count; i++)
            {
                var passiveEffect = (PassiveEffect) newItem.effects[i];
                if (passiveEffect.effectType != EffectType.Passive) continue;
                    
                if (passiveEffect.passiveType != PassiveTypes.StatBoost) continue;

                if (passiveEffect.EffectReady(_targetStats))
                { 
                    coroutines.Add(_targetStats.StartCoroutine(passiveEffect.Passive(_targetStats)));
                }
            }
        }
    }
}