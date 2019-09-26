using System.Collections;
using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
{
    public class Charge : Ability
    {
        public float lookRadius = 4f;

        private float distance;

        private bool isCharging;

        private Vector3 targetPoint;
        
        private Camera cam;

        public override void Start()
        {
            base.Start();
            cam = Camera.main;

            abilityName = "Charge!";
            abilityDescription = "";
            
            abilityCost = 5f;
            cooldown = 0.2f;
            castTime = 0.1f;
        }

        void Update()
        {
            Delay -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Q))
                Use(abilityCost, cooldown);

            CCharge();
        }

        protected override void Use(float cost, float cd)
        {
            if (Delay <= 0f && myStats.CurrentHealth > cost + 1)
            {
                StartCoroutine(AAbility(cd, " "));

                myStats.CurrentMana -= cost;

                Delay = cd - cd * myStats.stats[11].GetValue() / 100;
            }
        }
        
        public override IEnumerator AAbility(float cd, string statName)
        {
            yield return new WaitForSeconds(castTime);
            
            /*Plane playerPlane = new Plane(Vector3.up, transform.position);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (playerPlane.Raycast(ray, out var hitdist))
            {
                targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 100f * Time.deltaTime);

                distance = Vector3.Distance(targetPoint, transform.position);
                
                isCharging = true;
            }*/

            yield return new WaitForSeconds(cd);
        }

        void CCharge()
        {
            /*if (distance <= lookRadius)
            {
                while (Vector3.Distance(transform.position, targetPoint) > 0.001f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPoint, 0.01f * Time.deltaTime);
                    // Charge to the hitpoint
                    // Stun every enemy hit and deal damage to them
                    // focus nearest target and attack him once
                    // Check if player hits any obstacle and stop the Charge
                }

                isCharging = false;
            }
            else
            {
                isCharging = false;
            }
            //playerCombat.Attack(myStats);*/
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}