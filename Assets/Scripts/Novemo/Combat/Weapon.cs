using Novemo.Character;
using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Controllers;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Combat
{
	public class Weapon : MonoBehaviour
	{
		public SpriteRenderer weaponSprite; // Set current weapon sprite
		
		private float _physicalDamage;
		private float _magicDamage;
		private float _physicDamagePen;
		private float _magicDamagePen;
		private float _attackCooldown;

		private bool _isAttacking;

		private Characters.Character _playerStats;

		private void Start()
		{
			_playerStats = PlayerManager.Instance.player.GetComponent<Characters.Character>();
		}

		private void Update()
		{
			if (_playerStats.CanAttack && _attackCooldown <= 0f)
			{
				_isAttacking = true;
				
				//start animation

				_attackCooldown = 1f / _playerStats.stats[5].GetValue();
			}
		}
		
		private void FixedUpdate()
		{
			_attackCooldown -= Time.deltaTime;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (_isAttacking && other.CompareTag("Enemy"))
			{
				_physicalDamage = _playerStats.stats[2].GetValue();
				_magicDamage = _playerStats.stats[9].GetValue();
				_physicDamagePen = _playerStats.stats[23].GetValue();
				_magicDamagePen = _playerStats.stats[24].GetValue();
				other.GetComponent<Characters.Character>().TakeDamage(_playerStats, _physicalDamage, _magicDamage);
				_isAttacking = false;
			}
		}

		public void MoveToDefaultPosition()
		{
			GetComponent<SpriteRenderer>().sortingOrder = -1;
			GetComponent<SpriteRenderer>().sortingLayerName = "Player";
			transform.position = new Vector2(-0.01f, 0.3f);
			transform.eulerAngles = new Vector3(0f, 0f, 16f);
		}
	}
}