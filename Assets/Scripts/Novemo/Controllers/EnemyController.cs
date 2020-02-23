using System.Collections;
using Novemo.Combat;
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
        public float patrolDelay = 10f;
        
        public bool isRanged;

        public Animator animator;

        protected Transform target;
        protected CharacterStats enemyStats;
        protected Coroutine patrol;
        protected CharacterCombat combat;

        private void Start()
        {
            target = PlayerManager.Instance.player.transform;
            combat = GetComponent<CharacterCombat>();
            enemyStats = GetComponent<CharacterStats>();
        }

        protected IEnumerator Patrol()
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}
