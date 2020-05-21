namespace Novemo.Status_Effects.Buffs
{
    public class ThornsEffect : StatusEffect
    {
        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            TargetStats.OnDamageTaken += ReflectDamage;
        }

        public override void RemoveEffect()
        {
            TargetStats.OnDamageTaken -= ReflectDamage;
            
            base.RemoveEffect();
        }

        private void ReflectDamage(Characters.Character source, Characters.Character target, float damage)
        {
            if (source == target) return;
            
            damage *= EffectPower;
            source.TakeDamage(TargetStats, damage, 0);
        }
    }
}