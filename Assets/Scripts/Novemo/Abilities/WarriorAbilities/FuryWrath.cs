using System;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
	public class FuryWrath : Ability
	{
		private void Update()
		{
			abilityDelay -= Time.deltaTime;

			if (Input.GetButtonDown("Spell4") && abilityDelay <= 0f && !isActive)
			{
				abilityUseTime = DateTime.UtcNow;

				isActive = true;
			}
		}

		private void FixedUpdate()
		{
			if (isActive)
			{
				Active();
			}
		}

		protected override void Active()
		{
			
		}
	}
}