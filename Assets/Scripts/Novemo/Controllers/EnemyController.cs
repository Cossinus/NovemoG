using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Combat;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Novemo.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Pathfinder Values")]
        public float lookRadius;
        public float attackRadius;
        public float patrolDelay = 10f;
        
        public Animator animator;

        protected Transform target;
        protected Coroutine patrol;
        protected CharacterCombat combat;
        protected Characters.Character targetStats;
        protected Characters.Character enemyStats;

        private static Rigidbody2D _rb2d;

        private Vector3 prevLocation = Vector3.zero;
        
        private bool calculatePath = true;
        private static Tilemap tilemap;
        private List<TileController.Node> nodes = new List<TileController.Node>();

        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            target = PlayerManager.Instance.player.transform;
            targetStats = target.GetComponent<Player>();
            combat = GetComponent<CharacterCombat>();
            enemyStats = GetComponent<Characters.Character>();
        }
        
        //TODO make enemy stop if stopping distance <= distance between player and enemy and stop calculating the path
        //TODO repair movement (check for walls and just move 
        private float elapsed;
        private void Update()
        {
            elapsed += Time.deltaTime;

            if (elapsed > 1f && calculatePath)
            {
                var playerPos = tilemap.WorldToCell(target.position);
                var myPos = tilemap.WorldToCell(transform.position);
                var targetNode = new TileController.Node { X = playerPos.x, Y = playerPos.y};
                var startNode = new TileController.Node {X = myPos.x, Y = myPos.y};

                nodes = TileController.Search(startNode, targetNode);
                
                elapsed = 0;
            }
            
            if (nodes.Count > 0)
            {
                var position = transform.position;
                var currentTile = tilemap.WorldToCell(position);
                var nextTile = new Vector3Int(nodes.First().X, nodes.First().Y, 0);

                var smoothedDelta = Vector3.MoveTowards(position, tilemap.GetCellCenterWorld(nextTile),
                    enemyStats.stats[6].GetValue() * Time.deltaTime);
                _rb2d.MovePosition(smoothedDelta);

                if (Metrics.EqualFloats(currentTile.x, nextTile.x, 0.1f) &&
                    Metrics.EqualFloats(currentTile.y, nextTile.y, 0.1f))
                {
                    nodes.Remove(nodes.First());
                }

                if (Vector2.Distance(transform.position, target.position) < attackRadius)
                {
                    nodes.Clear();
                    calculatePath = false;
                    combat.Attack(targetStats);
                }
            }

            if (Vector2.Distance(transform.position, target.position) > attackRadius) calculatePath = true;
            
            SetAnimatorValues();
        }

        private void SetAnimatorValues()
        {
            var curVel = (transform.position - prevLocation) / Time.deltaTime;
            
            if (curVel.y > 0)
            {
                
            }
            else
            {
                
            }
            
            if (curVel.x > 0)
            {
                
            }
            else
            {
                
            }
            
            prevLocation = transform.position;
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
