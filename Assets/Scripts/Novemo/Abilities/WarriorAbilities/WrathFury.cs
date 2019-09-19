using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
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