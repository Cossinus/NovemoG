using System;

namespace Novemo.Items
{
    [Serializable]
    public class UniqueEffect
    {
        public string effectName;
        public string effectDescription;
    }

    public enum EffectAspect
    {
        DealDamage,
        Heal,
        SpeedUp,
        Dash,
        Regenerate,
        DamageBoost,
        StatsBoost,
        Blaze,
        Bolt,
        
    }
    
    public enum EffectType
    {
        Passive, Active
    }
}
