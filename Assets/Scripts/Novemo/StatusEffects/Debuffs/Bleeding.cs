using UnityEngine;

namespace Novemo.StatusEffects.Debuffs
{
	public class Bleeding : StatusEffect
	{
		private float elapsed;
		
		public override void ApplyEffect()
		{
			base.ApplyEffect();
			
			TargetStats.Bleeding = true;
		}
		
		public override void UpdateEffect()
		{
			elapsed += Time.deltaTime;

			if (elapsed > EffectDuration / EffectRate)
			{
				TargetStats.TakeLethalDamage(SourceStats, EffectPower, 0);
				
				elapsed = 0;
			}
			
			base.UpdateEffect();
		}

		public override void RemoveEffect()
		{
			TargetStats.Bleeding = false;
			
			base.RemoveEffect();
		}
	}
}