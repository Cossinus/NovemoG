using System;
using System.Collections;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Combat
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        private GameObject player;
        
        private CharacterStats _myStats;

        private float _attackCooldown;

        public GameObject enemyCurrentlyFightingWith;

        public bool Attacked { get; set; }

        public float attackDelay = 1f;

        public event Action<bool> OnAttack;

        private void Start()
        {
            player = PlayerManager.Instance.player;
            _myStats = GetComponent<CharacterStats>();
        }

        private void FixedUpdate()
        {
            _attackCooldown -= Time.deltaTime;
        }
    
        public void Attack(CharacterStats targetStats)
        {
            if (targetStats.CanAttack && _attackCooldown <= 0f)
            {
                Attacked = true;

                enemyCurrentlyFightingWith = targetStats.GetComponent<GameObject>();

                StartCoroutine(DealBasicAttackDamage(targetStats, attackDelay));
                
                OnAttack?.Invoke(true);

                _attackCooldown = 1f / _myStats.stats[5].GetValue();
            }
        }

        private void DealDamage(CharacterStats stats, DamageType type, float physicalDamageAmount, float magicDamageAmount, bool reduced)
        {
            if (reduced)
            {
                physicalDamageAmount *= 1 - stats.DamageReducePercentage / 100;
                magicDamageAmount *= 1 - stats.DamageReducePercentage / 100;
            }
            
            switch (type)
            {
                case DamageType.Physical:
                    stats.TakeDamage(physicalDamageAmount, 0, _myStats.stats[23].GetValue(), 0, false);
                    break;
                case DamageType.PhysicalLethal:
                    stats.TakeLethalDamage(physicalDamageAmount, 0);
                    break;
                case DamageType.Magic:
                    stats.TakeDamage(0, magicDamageAmount, 0, _myStats.stats[24].GetValue(), false);
                    break;
                case DamageType.PhysicalMagic:
                    stats.TakeLethalDamage(0, magicDamageAmount);
                    break;
                case DamageType.Mixed:
                    stats.TakeDamage(physicalDamageAmount, magicDamageAmount, _myStats.stats[23].GetValue(), _myStats.stats[24].GetValue(), false);
                    break;
                case DamageType.MixedLethal:
                    stats.TakeLethalDamage(physicalDamageAmount, magicDamageAmount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private IEnumerator DealBasicAttackDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            stats.TakeDamage(_myStats.stats[2].GetValue(), _myStats.stats[9].GetValue(), 1, 1, false);
            // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
        }
        
        /*private IEnumerator DealBasicAttackLethalDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            stats.TakeLethalDamage(_myStats.stats[10].GetValue(), _myStats.stats[9].GetValue(), false, false);
            // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
        }*/
    }

    public enum DamageType
    {
        Physical,
        PhysicalLethal,
        Magic,
        PhysicalMagic,
        Mixed,
        MixedLethal
    }
}
