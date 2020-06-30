using System;
using System.Collections;
using System.Collections.Generic;
using Novemo.Characters.Player;
using TMPro;
using UnityEngine;

namespace Novemo.Inventories
{
    public class StatsPanel : MonoBehaviour
    {
        #region Singleton
        
        public static StatsPanel Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        #endregion
        
        public List<TextMeshProUGUI> statsText = new List<TextMeshProUGUI>();
        public TextMeshProUGUI visualText;
        public TextMeshProUGUI sizeText;

        public GameObject statsTooltip;
        public GameObject placeholder;

        private Characters.Character playerStats;

        private void Start()
        {
            playerStats = PlayerManager.Instance.player.GetComponent<Characters.Character>();
        }

        public void ShowStatTooltip(int statIndex)
        {
            var statValue = playerStats.stats[statIndex].GetValue();
            var baseStatValue = playerStats.stats[statIndex].baseValue;
            var additionalStatValue = playerStats.stats[statIndex].GetValue() - playerStats.stats[statIndex].baseValue;

            if (InventoryManager.Instance.MovingSlot.IsEmpty && !InventoryManager.Instance.selectStackSize.activeSelf)
            {
                statsTooltip.SetActive(true);
                
                visualText.text = $"{playerStats.stats[statIndex].statName}: {statValue:F2}{Environment.NewLine}" +
                                  $"Base Value: {baseStatValue:F2}{Environment.NewLine}" +
                                  $"Bonus Value: {additionalStatValue:F2}";
                sizeText.text = visualText.text;

                var position = statsText[statIndex].GetComponent<RectTransform>().position;
                var xPos = position.x;
                var yPos = position.y;

                statsTooltip.transform.position = new Vector2(xPos, yPos);
            }
        }

        public void HideStatTooltip()
        {
            statsTooltip.SetActive(false);
        }

        public void HideAdvancedStats()
        {
            placeholder.SetActive(false);
        }

        public void ShowAdvancedStats(GameObject placeholder)
        {
            placeholder.SetActive(!placeholder.activeSelf);
        }
        
        public void UpdateStatsText()
        {
            for (int i = 0; i < statsText.Count; i++)
            {
                statsText[i].text = playerStats.stats[i].statName == "Attack Speed"
                    ? $"{playerStats.stats[i].statName}: {playerStats.stats[i].GetValue():F2}"
                    : $"{playerStats.stats[i].statName}: {playerStats.stats[i].GetValue():F0}";
            }
        }
    }
}
