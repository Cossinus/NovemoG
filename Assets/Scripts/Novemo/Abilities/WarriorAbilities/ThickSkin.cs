using UnityEngine;

namespace Novemo.Abilities.WarriorAbilities
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