using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerController player;

    public void AcceptQuest()
    {
        //questWindow.SetActive(false);
        quest.isActive = true;
        player.quest = quest;
    }
}
