﻿using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class PlayerUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;
    
    private float visibleTime = 5;
    private float lastMadeVisibleTime;

    private Transform ui;
    private Image healthSlider;
    private Transform cam;

    void Start()
    {
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

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            float healthPercent = (float) currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    }

    void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position;
            ui.forward = -cam.forward;
            ui.right = cam.right;

            if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}