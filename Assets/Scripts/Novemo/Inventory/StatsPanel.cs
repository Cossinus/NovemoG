using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Novemo.Player;
using Novemo.Stats;
using TMPro;
using UnityEngine;

namespace Novemo.Inventory
{
    public class StatsPanel : MonoBehaviour
    {
        public List<TextMeshProUGUI> statsText = new List<TextMeshProUGUI>();
        public TextMeshProUGUI visualText;
        public TextMeshProUGUI sizeText;
        
        public GameObject statsTooltip;

        private CharacterStats playerStats;

        void Start()
        {
            playerStats = PlayerManager.Instance.player.GetComponent<CharacterStats>();
        }

        void Update()
        {
            StartCoroutine(UpdateStatsText());
            
            if (Inventory.Instance.canvasGroup.alpha <= 0)
                statsTooltip.SetActive(false);
        }

        public void ShowStatTooltip(int statIndex)
        {
            var statValue = playerStats.stats[statIndex].GetValue();
            var baseStatValue = playerStats.stats[statIndex].baseValue;
            var additionalStatValue = playerStats.stats[statIndex].GetValue() - playerStats.stats[statIndex].baseValue;

            if (InventoryManager.Instance.MovingSlot.IsEmpty &&
                !InventoryManager.Instance.selectStackSize.activeSelf)
            {
                statsTooltip.SetActive(true);
                
                visualText.text = $"<i>{playerStats.stats[statIndex].statName}:</i> {statValue:F2}\n" +
                                  $"<i>Base Value:</i> {baseStatValue:F2}\n" +
                                  $"<i>Bonus Value:</i> {additionalStatValue:F2}";
                sizeText.text = visualText.text;

                var position = statsText[statIndex].GetComponent<RectTransform>().position;
                var xPos = position.x - statsText[statIndex].GetComponent<RectTransform>().rect.width / 2 - 5f;
                var yPos = position.y + 20f;

                statsTooltip.transform.position = new Vector2(xPos, yPos);
            }
        }

        public void HideStatTooltip()
        {
            statsTooltip.SetActive(false);
        }

        public void ShowAdvancedStats(GameObject placeholder)
        {
            placeholder.SetActive(!placeholder.activeSelf);
        }
        
        private IEnumerator UpdateStatsText()
        {
            for (int i = 0; i < statsText.Count; i++)
            {
                statsText[i].text = playerStats.stats[i].statName == "Attack Speed"
                    ? $"<i>{playerStats.stats[i].statName}:</i> {playerStats.stats[i].GetValue():F2}"
                    : $"<i>{playerStats.stats[i].statName}:</i> {playerStats.stats[i].GetValue():F0}";
            }
            
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
