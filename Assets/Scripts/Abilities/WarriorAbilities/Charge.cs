using System;
using System.Collections;
using System.Collections.Generic;
using Novemo;
using UnityEngine;

namespace Novemo
{
    public class Charge : Ability
    {
        public float lookRadius = 7f;
        
        private Camera cam;

        public override void Start()
        {
            base.Start();
            cam = Camera.main;
            abilityName = "Charge!";
            abilityDescription = "";
            cost = 5f;
            cooldown = 0.2f;
            castTime = 0.1f;
        }

        void Update()
        {
            Delay -= Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                Use(cost, cooldown);
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
            
            Plane playerPlane = new Plane(Vector3.up, transform.position);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            float hitdist;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 100f * Time.deltaTime);

                float distance = Vector3.Distance(targetPoint, transform.position);
                
                if (distance <= lookRadius)
                {
                    // Charge to the hitpoint
                    // Stun every enemy hit and deal damage to them
                    // focus nearest target and attack him once
                }
                else
                {
                    // Try going to the edge of lookRadius and then Charge
                }
            }

            yield return new WaitForSeconds(cd);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}