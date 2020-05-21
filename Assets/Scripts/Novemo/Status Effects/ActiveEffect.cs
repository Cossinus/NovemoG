using System;

namespace Novemo.Status_Effects
{
	public abstract class ActiveEffect : StatusEffect
	{
		public DateTime UseTime { get; set; }

		public virtual void Use() { }
	}
}