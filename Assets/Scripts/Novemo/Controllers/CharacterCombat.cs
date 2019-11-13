using System.Collections;
using Novemo.Abilities;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Controllers
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        private CharacterStats _myStats;

        private float _attackCooldown;

        public GameObject enemyCurrentlyFightingWith;

        public bool HasAttacked { get; set; }

        public float attackDelay = 1f;

        public event System.Action OnAttack;

        private void Start()
        {
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
                HasAttacked = true;

                enemyCurrentlyFightingWith = targetStats.GetComponent<GameObject>();
                
                StartCoroutine(DoBasicAttackDamage(targetStats, attackDelay));

                OnAttack?.Invoke();

                _attackCooldown = 1f / _myStats.stats[5].GetValue();
            }
        }

        private IEnumerator DoBasicAttackDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            stats.TakeDamage(_myStats.stats[2].GetValue(), _myStats.stats[9].GetValue());
            // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
        }
        
        private IEnumerator DoBasicAttackLethalDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            stats.TakeLethalDamage(_myStats.stats[10].GetValue(), _myStats.stats[9].GetValue());
            // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
        }

        public float DoPhysicalSpellDamage() { return 0; }
        
        public float DoMagicSpellDamage() { return 0; }
        
        public float DoMixedSpellDamage() { return 0; }
        
        public float DoLethalSpellDamage() { return 0; }
    }
}
