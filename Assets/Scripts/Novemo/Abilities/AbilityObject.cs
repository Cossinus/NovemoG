using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novemo.Abilities
{
	[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability")]
	public class AbilityObject : ScriptableObject
	{
		public string abilityName;
		public string abilityDescription;
		
		public List<float> abilityCooldown;
		public List<float> abilityDuration;
		public float abilityCastTime;

		public List<ListOfLists> abilityBaseDamage;
		public List<ListOfLists> abilityBonusDamage;
		public List<ListOfLists> abilityEffectDuration;

		public List<float> abilityCost;

		public List<float> abilityRadius;

		public List<int> abilityStack;

		public bool requiresTarget;

		public Ability.AbilityType abilityType;
		public TargetType targetType;

		public Sprite abilityIcon;
	
		[Serializable]
		public class ListOfLists
		{
			public List<float> list;
		}
	}
}