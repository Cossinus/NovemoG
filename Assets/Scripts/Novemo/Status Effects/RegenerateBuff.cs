using UnityEngine;

namespace Novemo.Status_Effects
{
	public class RegenerateBuff : StatusEffect
	{
		private float elapsed;
		
		public override void UpdateEffect()
		{
			elapsed += Time.deltaTime;

			if (elapsed > EffectDuration / EffectRate)
			{
				if (StatIndex == 0)
					CharacterStats.CurrentHealth += EffectPower;
				else
					CharacterStats.CurrentMana += EffectPower;

				Debug.Log(CharacterStats.CurrentHealth);
				elapsed = 0;
			}
			
			base.UpdateEffect();
		}
	}
}