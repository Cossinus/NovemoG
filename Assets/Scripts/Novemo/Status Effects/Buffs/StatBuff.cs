namespace Novemo.Status_Effects.Buffs
{
	public class StatBuff : Buff
	{
		private bool _callOnce;

		protected override void RemoveBuff()
		{
			switch (StatIndex)
			{
				case 98:
					CharacterStats.DamageReducePercentage -= BuffPower;
					break;
				case 99:
					CharacterStats.DamageBoostPercentage -= BuffPower;
					break;
				default:
					CharacterStats.stats[StatIndex].modifierValue -= BuffPower;
					CharacterStats.stats[StatIndex].Scale();
					break;
			}
			base.RemoveBuff();
			_callOnce = false;
		}

		public override void ApplyBuff()
		{
			base.ApplyBuff();
			if (_callOnce) return;
			switch (StatIndex)
			{
				case 98:
					CharacterStats.DamageReducePercentage += BuffPower;
					break;
				case 99:
					CharacterStats.DamageBoostPercentage += BuffPower;
					break;
				default:
					CharacterStats.stats[StatIndex].modifierValue += BuffPower;
					CharacterStats.stats[StatIndex].Scale();
					break;
			}

			_callOnce = true;
		}
	}
}