using System;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Status_Effects.Buffs
{
	public abstract class Buff : IEquatable<Buff>
	{
		public string BuffName { get; set; }
		public float BuffDuration { get; set; }
		public float BuffPower { get; set; }
		public float BuffRate { get; set; }
		public bool IsDecaying { get; set; }
		public int StatIndex { get; set; }

		public CharacterStats CharacterStats { get; set; }
		
		private float timeElapsed;

		public virtual void ApplyBuff()
		{
			CharacterStats.buffs.Add(this);
		}

		protected virtual void RemoveBuff()
		{
			CharacterStats.RemoveBuff(this);
			timeElapsed = 0;
		}
		
		public virtual void Update()
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed >= BuffDuration)
			{
				RemoveBuff();
			}
		}
		
		#region Override Equals
		
		public override bool Equals(object obj)
		{
			return Equals(obj as Buff);
		}
		
		public bool Equals(Buff other)
		{
			if (ReferenceEquals(other, null)) {
				return false;
			}
			
			if (ReferenceEquals(this, other)) {
				return true;
			}
			
			if (GetType() != other.GetType()) {
				return false;
			}

			return BuffName == other.BuffName && CharacterStats == other.CharacterStats;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (BuffName != null ? BuffName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ BuffDuration.GetHashCode();
				hashCode = (hashCode * 397) ^ BuffPower.GetHashCode();
				hashCode = (hashCode * 397) ^ BuffRate.GetHashCode();
				hashCode = (hashCode * 397) ^ IsDecaying.GetHashCode();
				hashCode = (hashCode * 397) ^ StatIndex;
				hashCode = (hashCode * 397) ^ (CharacterStats != null ? CharacterStats.GetHashCode() : 0);
				return hashCode;
			}
		}

		#endregion
	}
}