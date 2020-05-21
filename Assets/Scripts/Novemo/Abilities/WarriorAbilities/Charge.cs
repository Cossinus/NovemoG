using System;
using Novemo.Character;
using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Stats;
using Novemo.Status_Effects;
using Novemo.Status_Effects.Debuffs;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
	public class Charge : Ability
	{
		private float _activeMoveSpeed;

		private Rigidbody2D _rb2d;
		
		private StatusEffect _stun;
	    private StatusEffect _slow;
	    private Camera _camera;

	    private void Awake()
	    {
		    _camera = Camera.main;
		    _rb2d = PlayerManager.Instance.player.GetComponent<Rigidbody2D>();
		    ability = Resources.Load<AbilityObject>("Abilities/Warrior/Charge");
	    }

	    private void Update()
	    {
		    abilityDelay -= Time.deltaTime;

		    if (Input.GetButtonDown("Spell1") && abilityDelay <= 0f && playerStats.CurrentHealth > ability.abilityCost[abilityLevel] + 1 && !isActive && playerStats.CanUseSpells)
		    {
			    playerStats.ModifyHealth(-ability.abilityCost[abilityLevel]);

			    abilityUseTime = DateTime.UtcNow;
			    playerStats.CanAttack = false;
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
		    base.Active();
		    
		    Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
		    _activeMoveSpeed = playerStats.stats[6].GetValue() + 0.5f;

		    var smoothedDelta = Vector3.MoveTowards(transform.position, mousePos, Time.fixedDeltaTime * _activeMoveSpeed);
		    _rb2d.MovePosition(smoothedDelta);

		    if (abilityUseTime.AddSeconds(ability.abilityDuration[abilityLevel]) < DateTime.UtcNow && isActive)
		    {
			    isActive = false;
			    playerStats.CanAttack = true;

			    abilityDelay = ability.abilityCooldown[abilityLevel];
		    }
	    }
	    
	    private void OnTriggerEnter2D(Collider2D other)
		{
			if (!isActive || !other.CompareTag("Enemy")) return;
			
			var targetStats = other.gameObject.GetComponent<Characters.Character>();
			
			_stun = new Stun
			{
				EffectName = "Stun",
				EffectPower = ability.abilityBaseDamage[0].list[abilityLevel] + ability.abilityBonusDamage[0].list[abilityLevel],
				EffectDuration = ability.abilityEffectDuration[0].list[abilityLevel],
				TargetStats = targetStats
			};
			targetStats.GetComponent<Characters.Character>().ApplyStatusEffect(_stun);
			
			_slow = new Slow
			{
				EffectName = "Slow",
				EffectPower = ability.abilityBaseDamage[1].list[abilityLevel] + ability.abilityBonusDamage[1].list[abilityLevel],
				EffectDuration = ability.abilityEffectDuration[1].list[abilityLevel],
				TargetStats = targetStats
			};
			targetStats.GetComponent<Characters.Character>().ApplyStatusEffect(_slow);
		}
	}
}