using Novemo.Classes;
using Novemo.Controllers;
using Novemo.Inventory.Slot;
using Novemo.Player;
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
    
        private static GameObject _playerRef;

        public EquipSlot[] allEquipSlots;

        public GameObject weapon;
        public GameObject shield;
        public GameObject dagger;

        void Start()
        {
            _playerRef = PlayerManager.Instance.player;

            allEquipSlots = Inventory.Instance.statsUI.GetComponentsInChildren<EquipSlot>();
        
            // Do this after choosing class
            _playerRef.GetComponent<PlayerController>().playerClass = _playerRef.AddComponent<Warrior>();
        
            if (_playerRef.GetComponent<PlayerController>().playerClass == gameObject.GetComponent<Warrior>()/* || gameObject.GetComponent<Viking>() || gameObject.GetComponent<Warlock>() || gameObject.GetComponent<Elementar>() || gameObject.GetComponent<Necromancer>() || gameObject.GetComponent<MonsterHunter>()*/)
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
