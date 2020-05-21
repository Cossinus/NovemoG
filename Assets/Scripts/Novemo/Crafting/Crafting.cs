using System.Collections;
using System.Linq;
using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Inventories;
using Novemo.Inventory;
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

        public bool IsOpen { get; set; }
        
        public TextMeshProUGUI craftSkillText;

        private InventoryManager _inventoryManager;

        private CanvasGroup _canvasGroup;
        
        private Player _playerStats;
        
        private bool _fadingIn;
        private bool _fadingOut;

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

                StartCoroutine(nameof(FadeIn));
                IsOpen = true;
            }
            else
            {
                StartCoroutine(nameof(FadeOut));
                InventoryManager.Instance.selectStackSize.SetActive(false);
                IsOpen = false;

                if (!_inventoryManager.MovingSlot.IsEmpty)
                {
                    Inventories.Inventory.Instance.DropItems(_inventoryManager.MovingSlot.Items.ToList(), _playerStats.transform);
                    
                    _inventoryManager.MovingSlot.ClearSlot();
                    Destroy(GameObject.Find("Hover"));
                }
            }
        }
        
        private IEnumerator FadeOut()
        {
            if (_fadingOut) yield break;
            
            _fadingOut = true;
            _fadingIn = false;
            StopCoroutine(nameof(FadeIn));

            var startAlpha = _canvasGroup.alpha;
            var rate = 1.0f / 0.7f;
            var progress = 0.0f;
                
            while (progress < 1.0)
            {
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);

                progress += rate * Time.deltaTime;
                yield return null;
            }
            _canvasGroup.blocksRaycasts = false;

            _canvasGroup.alpha = 0;
            _fadingOut = false;
        }

        private IEnumerator FadeIn()
        {
            if (_fadingIn) yield break;
            
            _fadingOut = false;
            _fadingIn = true;
            StopCoroutine(nameof(FadeOut));

            var startAlpha = _canvasGroup.alpha;
            var rate = 1.0f / 0.7f;
            var progress = 0.0f;
                
            _canvasGroup.blocksRaycasts = true;
            while (progress < 1.0)
            {
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);

                progress += rate * Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 1;
            _fadingIn = false;
        }
    }
}