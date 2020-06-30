using UnityEngine;

namespace Novemo.StatusEffects.Buffs
{
	public class RegenerateBuff : StatusEffect
	{
		private float elapsed;
		
		public override void UpdateEffect()
		{
			elapsed += Time.deltaTime;
			
			if (elapsed > EffectDuration / EffectRate)
			{
				if (StatIndex == 28)
				{
					TargetStats.ModifyHealth(EffectPower);
				}
				else
				{
					TargetStats.ModifyMana(EffectPower);
				}

				elapsed = 0;
			}
			
			base.UpdateEffect();
		}
	}
}