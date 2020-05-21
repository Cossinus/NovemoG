using Novemo.UI;
using UnityEngine;

namespace Novemo.Status_Effects.Debuffs
{
	public class Ignite : StatusEffect
	{
		private float elapsed;

		private GameObject uiCanvas;
		
		public override void ApplyEffect()
		{
			base.ApplyEffect();

			TargetStats.Ignited = true;
			TargetStats.OnHealthModified += OnHealthModified;
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
			TargetStats.OnHealthModified -= OnHealthModified;
			TargetStats.Ignited = false;
			
			base.RemoveEffect();
		}

		private void OnHealthModified(float healthModified)
		{
			TargetStats.TakeDamage(SourceStats, 0, healthModified * 0.33f);
		}
	}
}