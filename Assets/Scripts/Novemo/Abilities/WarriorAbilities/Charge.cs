using System.Collections;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
	public class Charge : Ability
    {
	    private const string Name = "Charge!";
	    private const string Description = "Player charges to a specified destination and stuns any enemy hit.";
	    private static readonly Sprite Icon;// = (Sprite) Resources.Load("Abilities/Charge");
	    
	    private bool _isCharging;

	    private CharacterStats PlayerStats { get; set; }

	    private void Awake()
	    {
		    PlayerStats = PlayerManager.Instance.player.GetComponent<CharacterStats>();
		    AbilityRadius = 3f;
		    CastTime = 0.05f;
		    Cooldown = 7f - 7f * PlayerManager.Instance.player.GetComponent<PlayerStats>().stats[11].GetValue() / 100;
	    }

	    private void FixedUpdate()
	    {
		    Delay -= Time.deltaTime;
		    
		    if (Input.GetKeyDown(KeyCode.Q))
		    {
			    if (Delay <= 0f && PlayerStats.CurrentHealth > Cost + 1 && !_isCharging)
			    {
				    PlayerStats.CurrentMana -= Cost;

				    Delay = Cooldown;
				    
				    StartCoroutine(Active());
			    }
		    }
		}

	    public override IEnumerator Active()
		{
			_isCharging = true;
			PlayerStats.CanAttack = false;
			var activeMoveSpeed = PlayerStats.stats[6].GetValue() + 0.5f;
			StartCoroutine(PlayerStats.StopMoving(CastTime + activeMoveSpeed * Time.deltaTime + 1f));
			
			yield return new WaitForSeconds(CastTime);
			
			var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			const float rate = 1f;
			var progress = 0.0f;
			while (progress < 1.0)
			{
				transform.position = Vector2.MoveTowards(transform.position, mousePos, activeMoveSpeed * Time.deltaTime);

				progress += rate * Time.deltaTime;
				yield return null;
			}
			
			yield return new WaitForSeconds(Cooldown);
			_isCharging = false;
			PlayerStats.CanAttack = true;
		}

	    private void OnTriggerEnter2D(Collider2D other)
		{
			if (_isCharging)
			{
				if (other.CompareTag("Enemy"))
				{
					Target = other.gameObject;
					Target.GetComponent<CharacterStats>().ApplyDebuff("Stun", PlayerStats.Scale(2, 0.02f), 2.4f);
					Target.GetComponent<CharacterStats>().ApplyDebuff("Slow", 0, 1f);
				}
			}
		}
    }
}