using System;
using Novemo.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo
{
    [RequireComponent(typeof(CharacterStats))]
    public class InfoUI : MonoBehaviour
    {
        public TextMeshProUGUI levelText;
        public GameObject uiPrefab;
        public Transform target;

        private float visibleTime = 5;
        private float lastMadeVisibleTime;
    
        private Transform ui;
        private Image healthSlider;
        private Transform cam;
    
        void Start()
        {
            var targetStats = target.GetComponent<CharacterStats>();
            levelText.text = targetStats.level.ToString();
            if (Camera.main != null) cam = Camera.main.transform;
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, c.transform).transform;
                    healthSlider = ui.transform.Find("HealthMask/HealthBar").GetComponent<Image>();
                    ui.gameObject.SetActive(false);
                    break;
                }
            }
            
            GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        }
    
        void OnHealthChanged(float maxHealth, float currentHealth)
        {
            if (ui == null) return;
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;
    
            var healthPercent = currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
            
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    
        void LateUpdate()
        {
            var targetStats = target.GetComponent<CharacterStats>();
            if (targetStats.CurrentHealth >= targetStats.stats[0].GetValue())
            {
                healthSlider.fillAmount = 1f;
            }
            
            if (ui == null) return;
            ui.position = target.position;
            levelText.text = targetStats.level.ToString();

            if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}

