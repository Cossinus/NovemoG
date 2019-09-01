using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Novemo
{
    public class Ability : MonoBehaviour
    {
        #region Singleton

        public static Ability Instance;

        void Awake()
        {
            Instance = this;
        }

        #endregion
        
        public string abilityName;
        public string abilityDescription;
        
        public int attacksCount;

        protected float Delay;
        public float cooldown;
        public float castTime;
        public float cost;

        public bool isEnabled;
        public bool canCastOnSelf;

        public CharacterCombat playerCombat;
        public PlayerManager playerManager;
        public CharacterStats myStats;

        public virtual void Start()
        {
            playerManager = PlayerManager.Instance;
            myStats = playerManager.player.GetComponent<CharacterStats>();
            playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        }
        
        protected virtual void Use(float cost, float cd) { }

        public virtual IEnumerator AAbility(float cd, string statName)
        {
            yield return new WaitForSecondsRealtime(cd);
            
            // do stuff
        } 
    }
}