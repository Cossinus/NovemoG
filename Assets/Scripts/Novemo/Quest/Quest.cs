﻿using UnityEngine;

namespace Novemo.Quest
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        public bool isActive;
    
        public string Title;
        public string Description;
        public int ExpReward;
        public int GoldReward;
        // TODO more rewards;

        public QuestGoal goal;

        public void Complete()
        {
            isActive = false;
            // TODO extend that
        }
    }
}
