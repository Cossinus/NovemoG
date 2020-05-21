using System;
using Novemo.Character;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Abilities
{
	[RequireComponent(typeof(Characters.Character))]
	public abstract class Ability : MonoBehaviour
	{
		[NonSerialized] public AbilityObject ability;

		[NonSerialized] public Characters.Character playerStats;

		[NonSerialized] public int abilityLevel = 0;

		public bool isActive;

		protected float abilityDelay;

		protected DateTime abilityUseTime;
		
		protected GameObject Target { get; set; }

		public virtual void Passive(Characters.Character characterStats) { }

		public virtual void Disable() { }

		protected virtual void Active()
		{
			if (!playerStats.CanUseSpells) return;
		}

		protected void LevelUp() => abilityLevel++;

		private void Start()
		{
			abilityDelay = 0;
			playerStats = GetComponent<Characters.Character>();
		}

		public enum AbilityType
		{
			Melee,
			Ranged,
			AoEMelee,
			AoERanged,
			DoT,
			Passive
		}
	}
	
	public enum TargetType
	{
		Player,
		Enemy,
		Monster,
		EpicMonster,
		Boss,
		None
	}
}