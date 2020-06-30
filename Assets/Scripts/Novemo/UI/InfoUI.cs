using System;
using Novemo.Characters.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.UI
{
    [RequireComponent(typeof(Characters.Character))]
    public class InfoUI : MonoBehaviour
    {
        public Sprite[] starsContainer;
        
        public GameObject uiPrefab;
        public Transform target;

        public float visibleTime;
        
        private float lastMadeVisibleTime;

        private Characters.Character targetStats;
        
        private Transform ui;
        private Image healthSlider;
        private Image shieldSlider;

        private void Start()
        {
            targetStats = target.GetComponent<Characters.Character>();
            foreach (var c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, c.transform).transform;
                    healthSlider = ui.Find("HealthMask/HealthBar").GetComponent<Image>();
                    shieldSlider = ui.Find("HealthMask/ShieldBar").GetComponent<Image>();
                    
                    try
                    {
                        var enemyStats = (EnemyStats) targetStats;
                        var levelText = ui.Find("LevelBackground/LevelText").GetComponent<TextMeshProUGUI>();
                        levelText.text = targetStats.level.ToString();
                        var enemyName = ui.Find("NameBackground/EnemyName").GetComponent<TextMeshProUGUI>();
                        enemyName.text = gameObject.name;
                        var enemyStars = ui.Find("LevelBackground/Stars").GetComponent<Image>();
                        enemyStars.sprite = starsContainer[enemyStats.stars - 1];
                    } catch { /*Ignored*/ }
                    
                    ui.gameObject.SetActive(false);
                    break;
                }
            }
            
            targetStats.OnHealthChanged += OnHealthChanged;
        }
    
        private void OnHealthChanged(float maxHealth, float currentHealth, float shield)
        {
            if (ui == null) return;
            
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;
            
            var maxValue = currentHealth + shield;

            if (maxValue > maxHealth)
            {
                shieldSlider.fillAmount = currentHealth / maxValue + shield / maxValue;
                healthSlider.fillAmount = currentHealth / maxValue;
            }
            else
            {
                shieldSlider.fillAmount = currentHealth / maxHealth + shield / maxValue;
                healthSlider.fillAmount = currentHealth / maxHealth;
            }

            if (currentHealth <= 0)
            {
                targetStats.OnHealthChanged -= OnHealthChanged;
                Destroy(ui.gameObject);
            }
        }
    
        private void LateUpdate()
        {
            if (ui == null) return;
            
            ui.position = target.position;

            if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}

