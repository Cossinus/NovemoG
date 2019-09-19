﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Novemo.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        #region Singleton

        public static InventoryManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion
    
        // Game Objects
        public GameObject HoverObject { get; set; }
        public GameObject Clicked { get; set; }
        public GameObject selectStackSize;
        public GameObject iconPrefab;
        public GameObject toolTipObject;
        public GameObject dropItem;
        public GameObject clicked { get; set; }
    
        // Text Objects
        public TextMeshProUGUI sizeTextObject;
        public TextMeshProUGUI visualTextObject;
        public TextMeshProUGUI stackTxt;
    
        // Split Stack Slider
        public Slider splitSlider;
    
        // Inventory Canvas
        public Canvas canvas;
    
        // Event System
        public EventSystem eventSystem;

        // Slot Objects
        public Slot.Slot MovingSlot { get; set; }
        public Slot.Slot From { get; set; }
        public Slot.Slot To { get; set; }

        public int MaxStackCount { get; private set; }
        public int SplitAmount { get; set; }
    
    
        public void SetStackInfo(int maxStackCount)
        {
            selectStackSize.SetActive(true);
            toolTipObject.SetActive(false);
            SplitAmount = 0;
            MaxStackCount = maxStackCount;
            splitSlider.maxValue = maxStackCount;
            stackTxt.text = SplitAmount.ToString();
        }
    }
}