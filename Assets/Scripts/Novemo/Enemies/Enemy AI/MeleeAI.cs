using Novemo.Controllers;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Enemies.Enemy_AI
{
    public class MeleeAI : EnemyController
    {
        private void Update()
        {
            patrolDelay -= Time.deltaTime;
            
            BasicMeleeAI();
        }
        
        public void BasicMeleeAI()
        {
            float distance = Vector3.Distance(target.position, transform.position);
            
            if (distance <= lookRadius)
            {
                if (patrol != null)
                {
                    StopCoroutine(patrol);
                }

                if (distance >= stoppingDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position,
                        enemyStats.stats[6].GetValue() * Time.deltaTime);
                }
                else if (distance <= stoppingDistance)
                {
                    CharacterStats targetStats = target.GetComponent<CharacterStats>();
                    if (targetStats != null)
                    {
                        combat.Attack(targetStats);
                    }
                }
            }
            else
            {
                if (patrolDelay <= 0f)
                {
                    patrol = StartCoroutine(Patrol());

                    patrolDelay = 10f;
                }
            }
        }
    }
}