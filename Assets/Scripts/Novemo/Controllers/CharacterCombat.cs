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

        void Start()
        {
            _myStats = GetComponent<CharacterStats>();
        }

        void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }
    
        public void Attack(CharacterStats targetStats)
        {
            if (_attackCooldown <= 0f)
            {
                HasAttacked = true;

                enemyCurrentlyFightingWith = targetStats.GetComponent<GameObject>();
                
                StartCoroutine(DoDamage(targetStats, attackDelay));

                OnAttack?.Invoke();

                _attackCooldown = 1f / _myStats.stats[5].GetValue();
            }
        }

        IEnumerator DoDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            Ability.Instance.attacksCount++;
            stats.TakeDamage(_myStats.stats[2].GetValue(), _myStats.stats[9].GetValue());
            stats.TakeLethalDamage(_myStats.stats[10].GetValue(), _myStats.stats[9].GetValue());
            // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
        }
    }
}
