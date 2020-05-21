namespace Novemo.Status_Effects.Buffs
{
	public class StatBuff : StatusEffect
	{
		public override void ApplyEffect()
		{
			base.ApplyEffect();

			switch (StatIndex)
			{
				case 98:
					TargetStats.DamageReducePercentage += EffectPower;
					break;
				case 99:
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
				case 98:
					TargetStats.DamageReducePercentage -= EffectPower;
					break;
				case 99:
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