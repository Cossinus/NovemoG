using System.Collections;
using System.Collections.Generic;
using Novemo;
using UnityEngine;

namespace Novemo
{
    public class ThickSkin : Ability
    {
        public override void Start()
        {
            base.Start();
            abilityName = "Thick Skin";
            abilityDescription = "";
            cooldown = 19f;
        }

        void Update()
        {
            Delay -= Time.deltaTime;
        }
    }
}