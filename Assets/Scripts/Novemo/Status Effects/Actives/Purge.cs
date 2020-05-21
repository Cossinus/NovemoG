using System;

namespace Novemo.Status_Effects.Actives
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