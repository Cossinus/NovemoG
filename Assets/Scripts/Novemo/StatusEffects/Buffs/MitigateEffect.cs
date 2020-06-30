namespace Novemo.StatusEffects.Buffs
{
    public class MitigateEffect : StatusEffect
    {
        private bool _activated;

        public override void ApplyEffect()
        {
            //TODO add Icon assignment
            
            base.ApplyEffect();
            
            TargetStats.DamageReducePercentage += EffectPower;

            TargetStats.OnDamageTaken += MitigateDamage;
        }

        public override void RemoveEffect()
        {
            TargetStats.OnDamageTaken -= MitigateDamage;
            
            TargetStats.DamageReducePercentage -= EffectPower;

            base.RemoveEffect();
        }

        private void MitigateDamage(Characters.Character source, Characters.Character target, float damage)
        {
            if (source != target && !(EffectRate < 1)) EffectRate--;
            
            if (EffectRate < 1) RemoveEffect();
        }
    }
}