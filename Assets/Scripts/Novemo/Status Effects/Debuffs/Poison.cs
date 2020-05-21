using UnityEngine;

namespace Novemo.Status_Effects.Debuffs
{
	public class Poison : StatusEffect
	{
		private float elapsed;
		
		public override void ApplyEffect()
		{
			base.ApplyEffect();
			
			TargetStats.Poisoned = true;
			TargetStats.DamageReducePercentage -= 20f;
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
			TargetStats.DamageReducePercentage += 20f;
			TargetStats.Poisoned = false;
			
			base.RemoveEffect();
		}
	}
}