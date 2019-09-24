using System.Collections;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    public abstract class PassiveEffect : UniqueEffect
    {
        public PassiveTypes pType;
        
        public bool IsRegenerating { get; protected set; }

        public abstract IEnumerator Passive();
    }

    public enum PassiveTypes
    {
        Regenerate,
        Thorns,
        Blaze,
        Bolt,
        MitigateDamage,
        StatsBoost
    }
}