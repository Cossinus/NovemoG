using Novemo.Stats;
using UnityEngine;

namespace Novemo.Status_Effects.Debuffs
{
	public abstract class Debuff
	{
		public float DebuffDuration { get; set; }
		public float DebuffPower { get; set; }
		public float DebuffRate { get; set; }
		public int StatIndex { get; set; }
		
		public CharacterStats CharacterStats { get; set; }

		private float timeElapsed;

		public virtual void ApplyDebuff()
		{
			CharacterStats.debuffs.Add(this);
		}

		protected virtual void RemoveDebuff()
		{
			CharacterStats.RemoveDebuff(this);
			timeElapsed = 0;
		}
		
		public virtual void Update()
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed >= DebuffDuration)
			{
				RemoveDebuff();
			}
		}
	}
}