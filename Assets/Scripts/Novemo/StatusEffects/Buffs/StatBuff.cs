namespace Novemo.StatusEffects.Buffs
{
	public class StatBuff : StatusEffect
	{
		public override void ApplyEffect()
		{
			base.ApplyEffect();

			switch (StatIndex)
			{
				case 31:
					TargetStats.DamageReducePercentage += EffectPower;
					break;
				case 32:
					TargetStats.DamageBoostPercentage += EffectPower;
					break;
				default:
					TargetStats.stats[StatIndex].AddWholeModifier(EffectPower, TargetStats);
					break;
			}
		}
		
		public override void RemoveEffect()
		{
			switch (StatIndex)
			{
				case 31:
					TargetStats.DamageReducePercentage -= EffectPower;
					break;
				case 32:
					TargetStats.DamageBoostPercentage -= EffectPower;
					break;
				default:
					TargetStats.stats[StatIndex].AddWholeModifier(-EffectPower, TargetStats);
					break;
			}
			
			base.RemoveEffect();
		}
	}
}