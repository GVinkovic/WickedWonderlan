using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Quests Collection")]
public class QuestsCollection : ScriptableObject {

    [SerializeField]
    public List<Quest> quests;
    
    public Quest FindByName(string name)
    {
        foreach (var quest in quests)
            if (name == quest.name) return quest;
        return null;
    }
}
