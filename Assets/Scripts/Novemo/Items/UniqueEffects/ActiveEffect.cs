using System.Collections;
using Novemo.Stats;

namespace Novemo.Items.UniqueEffects
{
    public abstract class ActiveEffect : UniqueEffect
    {
        public ActiveTypes activeType;

        public float effectCooldown;
        
        protected bool CanCastOnSelf { get; set; }

        public abstract void Cooldown();
        public abstract IEnumerator Active(PlayerStats playerStats);
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