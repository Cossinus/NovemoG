using System.Collections;
using Novemo.Stats;

namespace Novemo.Items.UniqueEffects
{
    public abstract class PassiveEffect : UniqueEffect
    {
        public PassiveTypes passiveType;

        protected bool IsRegenerating { get; set; }
        protected bool Thorned { get; set; }
        protected bool Blazed { get; set; }
        protected bool Bolted { get; set; }
        protected bool Mitigated { get; set; }
        
        public abstract IEnumerator Passive(PlayerStats playerStats);
    }

    public enum PassiveTypes
    {
        Regenerate,
        Thorns,
        Blaze,
        Bolt,
        MitigateDamage,
        StatBoost
    }
}