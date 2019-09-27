using System.Collections;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    [CreateAssetMenu(menuName = "EquipmentEffects/PassiveEffects/StatBoostEffect")]
    public class StatBoostEffect : PassiveEffect
    {
        public string statBoostName;
        public int statIndex;
        public float statModifier;

        public override IEnumerator Passive(PlayerStats playerStats)
        {
            playerStats.StartCoroutine(playerStats.ScaleValues(statBoostName, statIndex, statModifier));
            yield return new WaitForSeconds(1f);
        }
    }
}