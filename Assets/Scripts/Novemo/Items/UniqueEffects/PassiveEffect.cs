using System.Collections;
using Novemo.Stats;

namespace Novemo.Items.UniqueEffects
{
    public abstract class PassiveEffect : UniqueEffect
    {
        public PassiveTypes passiveType;

        protected bool Regenerating { get; set; }
        protected bool Blazed { get; set; }
        protected bool Bolted { get; set; }
        protected bool Mitigated { get; set; }

        public abstract bool EffectReady(CharacterStats targetStats);
        
        public abstract IEnumerator Passive(CharacterStats targetStats);

        public abstract void OnEnable();
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