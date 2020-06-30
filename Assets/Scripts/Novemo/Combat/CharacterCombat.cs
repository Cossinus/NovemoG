using System;
using System.Collections;
using Novemo.Characters.Player;
using UnityEngine;

namespace Novemo.Combat
{
    [RequireComponent(typeof(Characters.Character))]
    public class CharacterCombat : MonoBehaviour
    {
        private GameObject player;
        
        private Characters.Character _myStats;

        private float _attackCooldown;

        public GameObject enemyCurrentlyFightingWith;

        public bool Attacked { get; set; }

        public float attackDelay = 1f;

        public event Action OnAttack;

        private void Start()
        {
            player = PlayerManager.Instance.player;
            _myStats = GetComponent<Characters.Character>();
        }

        private void FixedUpdate()
        {
            _attackCooldown -= Time.deltaTime;
        }
    
        public void Attack(Characters.Character targetStats)
        {
            if (targetStats.CanAttack && _attackCooldown <= 0f)
            {
                Attacked = true;

                enemyCurrentlyFightingWith = targetStats.GetComponent<GameObject>();

                StartCoroutine(DealBasicAttackDamage(targetStats, attackDelay));
                
                OnAttack?.Invoke();

                _attackCooldown = 1f / _myStats.stats[5].GetValue();
            }
        }

        //TODO BasicAttack method and SpellDamage method
        
        private void DealDamage(Characters.Character stats, DamageType type, float physicalDamageAmount, float magicDamageAmount, bool reduced)
        {
            if (reduced)
            {
                physicalDamageAmount *= 1 - stats.DamageReducePercentage / 100;
                magicDamageAmount *= 1 - stats.DamageReducePercentage / 100;
            }
            
            switch (type)
            {
                case DamageType.Physical:
                    stats.TakeDamage(_myStats, physicalDamageAmount, 0);
                    break;
                case DamageType.PhysicalLethal:
                    stats.TakeLethalDamage(_myStats, physicalDamageAmount, 0);
                    break;
                case DamageType.Magic:
                    stats.TakeDamage(_myStats, 0, magicDamageAmount);
                    break;
                case DamageType.PhysicalMagic:
                    stats.TakeLethalDamage(_myStats, 0, magicDamageAmount);
                    break;
                case DamageType.Mixed:
                    stats.TakeDamage(_myStats, physicalDamageAmount, magicDamageAmount);
                    break;
                case DamageType.MixedLethal:
                    stats.TakeLethalDamage(_myStats, physicalDamageAmount, magicDamageAmount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private IEnumerator DealBasicAttackDamage(Characters.Character stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            stats.TakeDamage(_myStats, _myStats.stats[2].GetValue(), _myStats.stats[9].GetValue());
        }
        
        /*private IEnumerator DealBasicAttackLethalDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            stats.TakeLethalDamage(_myStats.stats[10].GetValue(), _myStats.stats[9].GetValue(), false, false);
        }*/
    }

    public enum DamageType
    {
        Physical,
        PhysicalLethal,
        Magic,
        PhysicalMagic,
        Mixed,
        MixedLethal,
        Spell,
        BasicAttack
    }
}
