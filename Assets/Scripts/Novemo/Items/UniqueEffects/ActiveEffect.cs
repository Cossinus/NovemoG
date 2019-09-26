using System.Collections;

namespace Novemo.Items.UniqueEffects
{
    public abstract class ActiveEffect : UniqueEffect
    {
        public ActiveTypes activeType;

        protected bool CanCastOnSelf { get; set; }

        public abstract void Cooldown();
        public abstract IEnumerator Active();
    }
    
    public enum ActiveTypes
    {
        DealDamage,
        Heal,
        SpeedUp,
        Dash,
        DamageBoost
    }
}