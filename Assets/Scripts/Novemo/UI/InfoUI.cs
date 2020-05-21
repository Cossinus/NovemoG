using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.UI
{
    [RequireComponent(typeof(Characters.Character))]
    public class InfoUI : MonoBehaviour
    {
        public TextMeshProUGUI levelText;
        public GameObject uiPrefab;
        public Transform target;

        public float visibleTime;
        
        private float lastMadeVisibleTime;

        private Characters.Character targetStats;
        
        private Transform ui;
        private Image healthSlider;
        private Image shieldSlider;

        void Start()
        {
            targetStats = target.GetComponent<Characters.Character>();
            levelText.text = targetStats.level.ToString();
            foreach (var c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, c.transform).transform;
                    healthSlider = ui.transform.Find("HealthMask/HealthBar").GetComponent<Image>();
                    shieldSlider = ui.transform.Find("HealthMask/ShieldBar").GetComponent<Image>();
                    ui.gameObject.SetActive(false);
                    break;
                }
            }
            
            targetStats.OnHealthChanged += OnHealthChanged;
        }
    
        void OnHealthChanged(float maxHealth, float currentHealth, float shield)
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
                Destroy(ui.gameObject);
            }
        }
    
        void LateUpdate()
        {
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

