namespace Novemo.StatusEffects.Debuffs
{
	public class Slow : StatusEffect
	{
		public override void ApplyEffect()
		{
			base.ApplyEffect();
			
			TargetStats.stats[6].AddWholeModifier(EffectPower, TargetStats);
		}

		public override void RemoveEffect()
		{
			TargetStats.stats[6].AddWholeModifier(-EffectPower, TargetStats);

			base.RemoveEffect();
		}
	}
}