using System.Collections;
using System.Linq;
using Novemo.Characters.Player;
using Novemo.Inventories;
using Novemo.Inventories.Slots;
using TMPro;
using UnityEngine;

namespace Novemo.Crafting
{
    public class Crafting : MonoBehaviour
    {
        #region Singleton

        public static Crafting Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public TextMeshProUGUI craftSkillText;

        public GameObject craftingParent;

        private InventoryManager _inventoryManager;

        private CanvasGroup _canvasGroup;
        
        private Player _playerStats;

        public bool IsOpen { get; private set; }

        public void Start()
        {
            _inventoryManager = InventoryManager.Instance;
            _playerStats = PlayerManager.Instance.player.GetComponent<Player>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Open()
        {
            if (Metrics.EqualFloats(_canvasGroup.alpha, 0, 0.01f))
            {
                craftSkillText.text = $"Crafting Skill: {_playerStats.CraftSkill.ToString()}";

                StartCoroutine(_inventoryManager.FadeIn(_canvasGroup));
                IsOpen = true;
            }
            else
            {
                StartCoroutine(_inventoryManager.FadeOut(_canvasGroup));

                _inventoryManager.selectStackSize.SetActive(false);
                IsOpen = false;
            }
        }
    }
}