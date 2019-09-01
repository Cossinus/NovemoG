using System.Collections;
using System.Collections.Generic;
using Novemo;
using UnityEngine;

namespace Novemo
{
    public class Charge : Ability
    {
        private Camera cam;
        
        public override void Start()
        {
            base.Start();
            cam = Camera.main;
            abilityName = "Charge!";
            abilityDescription = "";
            cost = 5f;
            cooldown = 0.2f;
            castTime = 0.5f;
        }

        void Update()
        {
            Delay -= Time.deltaTime;
            
            if (Input.GetKey/*Down*/(KeyCode.Q))
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

            FaceDirection();

            yield return new WaitForSeconds(cd);
        }
        
        private void FaceDirection()
        {
            Vector2 mousePos = new Vector2();

            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;
            
            Vector3 direction = (-cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane))  - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 25f);
        }
    }
}