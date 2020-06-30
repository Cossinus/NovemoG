using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Novemo.Characters;
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
        protected Character targetStats;
        protected Character enemyStats;

        protected Pathfinding pathfinding;

        private Rigidbody2D _rb2d;

        private Vector3 prevLocation = Vector3.zero;

        private bool calculatePath = true;
        private Tilemap _tilemap;
        private List<Pathfinding.Node> _nodes = new List<Pathfinding.Node>();

        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            target = PlayerManager.Instance.player.transform;
            targetStats = target.GetComponent<Player>();
            combat = GetComponent<CharacterCombat>();
            enemyStats = GetComponent<Character>();
            pathfinding = GetComponent<Pathfinding>();
        }
        
        //TODO make enemy stop if stopping distance <= distance between player and enemy and stop calculating the path
        //TODO repair movement (check for walls and just move 
        private float elapsed;
        private void Update()
        {
            elapsed += Time.deltaTime;

            if (elapsed > 1f && calculatePath)
            {
                var playerPos = _tilemap.WorldToCell(target.position);
                var myPos = _tilemap.WorldToCell(transform.position);
                var targetNode = new Pathfinding.Node { X = playerPos.x, Y = playerPos.y};
                var startNode = new Pathfinding.Node {X = myPos.x, Y = myPos.y};

                _nodes = pathfinding.Search(startNode, targetNode);
                
                elapsed = 0;
            }
            
            if (_nodes.Count > 0)
            {
                var position = transform.position;
                var currentTile = _tilemap.WorldToCell(position);
                var nextTile = new Vector3Int(_nodes.First().X, _nodes.First().Y, 0);

                var smoothedDelta = Vector3.MoveTowards(position, _tilemap.GetCellCenterWorld(nextTile),
                    enemyStats.stats[6].GetValue() * Time.deltaTime);
                _rb2d.MovePosition(smoothedDelta);

                if (Metrics.EqualFloats(position.x, _tilemap.GetCellCenterWorld(nextTile).x, 0.01f) &&
                    Metrics.EqualFloats(position.y, _tilemap.GetCellCenterWorld(nextTile).y, 0.01f))
                {
                    _nodes.Remove(_nodes.First());
                }

                if (Vector2.Distance(transform.position, target.position) < attackRadius)
                {
                    _nodes.Clear();
                    pathfinding.pathLine.positionCount = 0;
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
