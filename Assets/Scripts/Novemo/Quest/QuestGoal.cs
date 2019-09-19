using System;
using Novemo.Items;

namespace Novemo.Quest
{
    [Serializable]
    public class QuestGoal
    {
        public GoalType goalType;
        public int requiredAmount;
        public int currentAmount;

        public bool IsReached()
        {
            return (currentAmount >= requiredAmount);
        }

        public void EnemyKilled(/* Enemy enemy */)
        {
            if (goalType == GoalType.Kill)
                currentAmount++;
            // Check if player killed specified type of enemy
        }
    
        public void ItemCollected(Item item)
        {
            if (goalType == GoalType.Gathering)
                currentAmount++;
            // Check if player gathered specified type of item
        }
    }

    public enum GoalType
    {
        Kill,
        Gathering,
        // TODO add more
    }
}