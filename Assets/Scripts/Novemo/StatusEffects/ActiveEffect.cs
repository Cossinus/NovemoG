using System;

namespace Novemo.StatusEffects
{
	public abstract class ActiveEffect : StatusEffect
	{
		public DateTime UseTime { get; set; }
		
		public float Cooldown { get; set; }

		public abstract void Use();
	}
}