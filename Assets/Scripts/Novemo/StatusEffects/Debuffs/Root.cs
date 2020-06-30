namespace Novemo.StatusEffects.Debuffs
{
	public class Root : StatusEffect
	{
		public override void ApplyEffect()
		{
			base.ApplyEffect();

			TargetStats.CanMove = false;
			
			TargetStats.TakeDamage(SourceStats, EffectPower, EffectMagicDamage);
		}

		public override void RemoveEffect()
		{
			TargetStats.CanMove = true;
			
			base.RemoveEffect();
		}
	}
}