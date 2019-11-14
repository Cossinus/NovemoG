using System.Collections;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Novemo.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius;
        public float attackRadius;
        public float stoppingDistance;

        public bool isRanged;

        private float patrolDelay = 10f;

        private Coroutine patrol;
        
        private Transform target;
        private CharacterCombat combat;
        private CharacterStats enemyStats;
        
        private void Start()
        {
            target = PlayerManager.Instance.player.transform;
            combat = GetComponent<CharacterCombat>();
            enemyStats = GetComponent<CharacterStats>();
        }
        
        private void Update()
        {
            patrolDelay -= Time.deltaTime;
            
            float distance = Vector3.Distance(target.position, transform.position);

            if (!isRanged)
            {
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

                        FaceTarget();
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
            else
            {
                
            }
        }

        private IEnumerator Patrol()
        {
            var enemyPosition = transform.position;
            var randomXPosition = Random.Range(enemyPosition.x - lookRadius, enemyPosition.x + lookRadius);
            var randomYPosition = Random.Range(enemyPosition.y - lookRadius, enemyPosition.y + lookRadius);
            var randomPosition = new Vector2(randomXPosition, randomYPosition);

            const float rate = 1f;
            var progress = 0.0f;
            var distance = Vector2.Distance(randomPosition, enemyPosition);
            
            while (progress < distance)
            {
                transform.position = Vector2.MoveTowards(transform.position, randomPosition,
                    enemyStats.stats[6].GetValue() * Time.deltaTime);
                
                progress += rate * Time.deltaTime;
                yield return null;
            }
        }

        private void FaceTarget()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}
