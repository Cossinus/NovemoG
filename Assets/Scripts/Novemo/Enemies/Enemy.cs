using Novemo.Controllers;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Enemies
{
    [RequireComponent(typeof(CharacterStats))]
    public class Enemy : Interactable
    {
        private PlayerManager playerManager;
        private CharacterStats myStats;
    
        void Start()
        {
            playerManager = PlayerManager.Instance;
            myStats = GetComponent<CharacterStats>();
        }
    
        public override void Interact()
        {
            base.Interact();
            CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
            if (playerCombat != null)
            {
                playerCombat.Attack(myStats);
            }
        }
    }
}
