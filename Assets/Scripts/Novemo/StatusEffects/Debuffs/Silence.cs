namespace Novemo.StatusEffects.Debuffs
{
	public class Silence : StatusEffect
	{
		public override void ApplyEffect()
		{
			base.ApplyEffect();

			TargetStats.CanUseSpells = false;
			
			TargetStats.TakeDamage(SourceStats, 0, EffectPower);
		}

		public override void RemoveEffect()
		{
			TargetStats.CanUseSpells = true;
			
			base.RemoveEffect();
		}
	}
}