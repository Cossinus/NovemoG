using System;

namespace Novemo.StatusEffects.Actives
{
	public class Purge : ActiveEffect
	{
		public override void Use()
		{
			if (UseTime.AddSeconds(EffectDuration) < DateTime.UtcNow)
			{
				UseTime = DateTime.UtcNow;

				TargetStats.RemoveNegativeEffects();
			}
		}
	}
}