using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {

    public void GiveQuest(string name)
    {
        QuestManager.AcceptQuest(QuestManager.Quests.FindByName(name));
    }
}
