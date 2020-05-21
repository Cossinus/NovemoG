using System;
using UnityEngine;

namespace Novemo.Status_Effects
{
	public abstract class StatusEffect : IEquatable<StatusEffect>
	{
		public string EffectName { get; set; }
		public float EffectPower { get; set; }
		public float EffectMagicDamage { get; set; }
		public float EffectDuration { get; set; }
		public float EffectRate { get; set; }
		public bool IsDecaying { get; set; }
		public int StatIndex { get; set; }

		public Sprite Icon { get; set; }

		public Characters.Character SourceStats { get; set; }
		public Characters.Character TargetStats { get; set; }
		
		private float _timeElapsed;

		public virtual void ApplyEffect()
		{
			TargetStats.statusEffects.Add(this);
		}

		public virtual void RemoveEffect()
		{
			TargetStats.RemoveStatusEffect(this);
			_timeElapsed = 0;
		}
		
		public virtual void UpdateEffect()
		{
			_timeElapsed += Time.deltaTime;

			if (_timeElapsed >= EffectDuration)
			{
				RemoveEffect();
			}
		}
		
		#region Override Equals

		public override bool Equals(object obj) => Equals(obj as StatusEffect);

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

			return EffectName == other.EffectName && TargetStats == other.TargetStats &&
			       Metrics.EqualFloats(EffectDuration, other.EffectDuration, 0.001);
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
				hashCode = (hashCode * 397) ^ (TargetStats != null ? TargetStats.GetHashCode() : 0);
				return hashCode;
			}
		}

		#endregion
	}
}