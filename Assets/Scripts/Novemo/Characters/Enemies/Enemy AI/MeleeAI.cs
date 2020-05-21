using Novemo.Controllers;
using UnityEngine;

namespace Novemo.Characters.Enemies.Enemy_AI
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

                if (distance >= attackRadius)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position,
                        enemyStats.stats[6].GetValue() * Time.deltaTime);
                }
                else if (distance <= attackRadius)
                {
                    var targetStats = target.GetComponent<Character>();
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