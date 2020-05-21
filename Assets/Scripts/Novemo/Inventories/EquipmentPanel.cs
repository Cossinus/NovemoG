using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Classes;
using Novemo.Controllers;
using Novemo.Inventories.Slot;
using UnityEngine;

namespace Novemo.Inventory
{
    public class EquipmentPanel : MonoBehaviour
    {
        #region Singleton

        public static EquipmentPanel Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of Inventory found!");
                return;
            }

            Instance = this;
        }

        #endregion
    
        private static Class playerClass;

        public EquipSlot[] allEquipSlots;

        public GameObject weapon;
        public GameObject shield;
        public GameObject dagger;

        void Start()
        {
            playerClass = PlayerManager.Instance.player.GetComponent<PlayerController>().PlayerClass;

            //TODO Do this after choosing class

            if (playerClass == FindObjectOfType<ClassHandler>().classes[0]/* || gameObject.GetComponent<Viking>() || gameObject.GetComponent<Warlock>() || gameObject.GetComponent<Elementar>() || gameObject.GetComponent<Necromancer>() || gameObject.GetComponent<MonsterHunter>()*/)
            {
                shield.SetActive(true);
            }
            /*if (playerClass == gameObject.GetComponent<Thief>() || gameObject.GetComponent<Collector>() || gameObject.GetComponent<Ninja>() || gameObject.GetComponent<HeadHunter>() || gameObject.GetComponent<Elf>() || gameObject.GetComponent<Hunter>())
            {
            dagger.SetActive(true);
            weapon.SetActive(false);
            }*/
        }
    }
}
