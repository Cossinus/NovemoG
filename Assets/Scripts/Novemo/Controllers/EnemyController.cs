using System.Collections;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Novemo.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius = 10f;
        public float stoppingDistance = 1f;

        public bool isRanged;
        public bool IsChasing { get; set; }

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
            float distance = Vector3.Distance(target.position, transform.position);

            if (!isRanged)
            {
                if (distance <= lookRadius)
                {
                    IsChasing = true;
                    
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
            }
            else
            {
                //Check for distance, stopping distance is range
            }

            if (!IsChasing)
            {
                StartCoroutine(Patrol(lookRadius));
            }
        }

        private void FaceTarget()
        {
            
        }

        private IEnumerator Patrol(float delay)
        {
            var randomXPosition = Random.Range(-lookRadius, lookRadius);
            var randomYPosition = Random.Range(-lookRadius, lookRadius);
            var randomPosition = new Vector2(randomXPosition, randomYPosition);
            
            yield return new WaitForSeconds(delay);
                
            transform.position = Vector2.MoveTowards(transform.position, randomPosition,
                    enemyStats.stats[6].GetValue() * Time.deltaTime);
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}
