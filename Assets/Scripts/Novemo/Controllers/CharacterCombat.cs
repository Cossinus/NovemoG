﻿using System.Collections;
using Novemo.Abilities;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Controllers
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        private CharacterStats myStats;

        private float attackCooldown;

        public bool hasAttacked;

        public float attackDelay = .6f;

        public event System.Action OnAttack;

        void Start()
        {
            myStats = GetComponent<CharacterStats>();
        }

        void Update()
        {
            attackCooldown -= Time.deltaTime;
        }
    
        public void Attack(CharacterStats targetStats)
        {
            if (attackCooldown <= 0f)
            {
                hasAttacked = true;
            
                StartCoroutine(DoDamage(targetStats, attackDelay));

                OnAttack?.Invoke();

                attackCooldown = 1f / myStats.stats[5].GetValue();
            }
        }

        IEnumerator DoDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);
            Ability.Instance.attacksCount++;
            stats.TakeDamage(myStats.stats[2].GetValue(), myStats.stats[9].GetValue(), myStats.stats[10].GetValue(), myStats.stats[9].GetValue());
            // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
        }
    }
}