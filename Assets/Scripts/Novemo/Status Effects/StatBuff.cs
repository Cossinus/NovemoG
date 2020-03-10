namespace Novemo.Status_Effects
{
	public class StatBuff : StatusEffect
	{
		private bool _callOnce;

		public override void RemoveEffect()
		{
			switch (StatIndex)
			{
				case 98:
					CharacterStats.DamageReducePercentage -= EffectPower;
					break;
				case 99:
					CharacterStats.DamageBoostPercentage -= EffectPower;
					break;
				default:
					CharacterStats.stats[StatIndex].wholeModifierValue -= EffectPower;
					CharacterStats.stats[StatIndex].Scale();
					break;
			}
			base.RemoveEffect();
			_callOnce = false;
		}

		public override void ApplyEffect()
		{
			base.ApplyEffect();
			if (_callOnce) return;
			switch (StatIndex)
			{
				case 98:
					CharacterStats.DamageReducePercentage += EffectPower;
					break;
				case 99:
					CharacterStats.DamageBoostPercentage += EffectPower;
					break;
				default:
					CharacterStats.stats[StatIndex].wholeModifierValue += EffectPower;
					CharacterStats.stats[StatIndex].Scale();
					break;
			}

			_callOnce = true;
		}
	}
}