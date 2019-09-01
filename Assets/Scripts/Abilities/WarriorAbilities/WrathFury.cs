using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novemo
{
    public class WrathFury : Ability
    {
        public override void Start()
        {
            base.Start();
            abilityName = "Wrath Fury";
            abilityDescription = "";
            cooldown = 45f;
        }

        void Update()
        {
            Delay -= Time.deltaTime;
        }
    }
}