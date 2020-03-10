using System;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Status_Effects
{
	public abstract class StatusEffect : IEquatable<StatusEffect>
	{
		public string EffectName { get; set; }
		public float EffectDuration { get; set; }
		public float EffectPower { get; set; }
		public float EffectRate { get; set; }
		public bool IsDecaying { get; set; }
		public int StatIndex { get; set; }

		public CharacterStats CharacterStats { get; set; }
		
		private float timeElapsed;

		public virtual void ApplyEffect()
		{
			CharacterStats.statusEffects.Add(this);
		}

		public virtual void RemoveEffect()
		{
			CharacterStats.RemoveStatusEffect(this);
			timeElapsed = 0;
		}
		
		public virtual void UpdateEffect()
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed >= EffectDuration)
			{
				RemoveEffect();
			}
		}
		
		#region Override Equals
		
		public override bool Equals(object obj)
		{
			return Equals(obj as StatusEffect);
		}
		
		public bool Equals(StatusEffect other)
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

			return EffectName == other.EffectName && CharacterStats == other.CharacterStats;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = EffectName != null ? EffectName.GetHashCode() : 0;
				hashCode = (hashCode * 397) ^ EffectDuration.GetHashCode();
				hashCode = (hashCode * 397) ^ EffectPower.GetHashCode();
				hashCode = (hashCode * 397) ^ EffectRate.GetHashCode();
				hashCode = (hashCode * 397) ^ IsDecaying.GetHashCode();
				hashCode = (hashCode * 397) ^ StatIndex;
				hashCode = (hashCode * 397) ^ (CharacterStats != null ? CharacterStats.GetHashCode() : 0);
				return hashCode;
			}
		}

		#endregion
	}
}