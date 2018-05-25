using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public GameObject QuestUIParent;
    public GameObject QuestUI;
    public QuestsCollection questsCollection;

    private static Dictionary<string, int> questsProgress = new Dictionary<string, int>();

    #region Singleton

    private static QuestManager instance;

    void Awake () {

        instance = this;

       
    }
    #endregion
  

    public static void AcceptQuest(string name)
    {
        AcceptQuest(Quests.FindByName(name));
    }
    public static void AcceptQuest(Quest quest)
    {
        if(questsProgress.ContainsKey(quest.name))
        {
            // quest već izvršen
            if (questsProgress[quest.name] == quest.finishProgress)
            {
                QuestAlreadyFinished(quest);
                return;
            }
            else
            {
                // quest se trenutno izvrsava
                return;
            }
        }

        if(quest.requiredQuestNames != null && quest.requiredQuestNames.Length > 0)
        {
            foreach(var requiredQuest in quest.requiredQuestNames)
            {
                // ako ni prosa obavezne questove prije ovog questa
                if (!(questsProgress.ContainsKey(requiredQuest) &&
                    questsProgress[requiredQuest] == instance.questsCollection.FindByName(requiredQuest).finishProgress))
                {
                    RequiredQuestNotFinished(quest);
                    return;
                }
            }
           
        }

        questsProgress.Add(quest.name, 0);

        ShowQuestInUI(quest);
    }

    public static string GetQuestProgressText(Quest quest)
    {
        if (!questsProgress.ContainsKey(quest.name)) return null;

        if (quest.questType == Quest.QuestType.CollectItems)
        {
            return questsProgress[quest.name] + "/" + quest.finishProgress + " items collected";
        }
        else if (quest.questType == Quest.QuestType.KillEnemy)
        {
            return questsProgress[quest.name] + "/" + quest.finishProgress + " enemy's killed";
        }

        return null;
    }

    static void ShowQuestInUI(Quest quest)
    {
        var questGO = Instantiate(instance.QuestUI, instance.QuestUIParent.transform);

        var questUI = questGO.GetComponent<QuestUI>();
        questUI.quest = quest;
        questUI.UpdateUI();
       
    }

    static void QuestCompleted(Quest quest)
    {
        foreach(var questUI in instance.QuestUIParent.GetComponentsInChildren<QuestUI>())
        {
            if(questUI.quest.name == quest.name)
            {
                questUI.Remove();
                break;
            }
        }
    }
    static void RefreshQuestUI(Quest quest)
    {
        foreach (var questUI in instance.QuestUIParent.GetComponentsInChildren<QuestUI>())
        {
            if (questUI.quest.name == quest.name)
            {
                questUI.UpdateUI();
                break;
            }
        }
    }

    static void QuestAlreadyFinished(Quest quest)
    {

    }
    static void RequiredQuestNotFinished(Quest quest)
    {
     //   if (!string.IsNullOrEmpty(quest.requiredQuestStepErroMsg))
        {
            //TODO: prikazi poruku da mora najprije prethodne
        }
    }

    public static Dictionary<string, int> QuestsProgress
    {
        get { return questsProgress; }
        set {
                questsProgress = value;
                UpdateQuestsUI();
            }
    }
    public static QuestsCollection Quests
    {
        get { return instance.questsCollection; }
    }

    static void UpdateQuestsUI()
    {
        foreach (var questUI in instance.QuestUIParent.GetComponentsInChildren<QuestUI>())
        {
            questUI.RemoveImmediate();
        }

        foreach(var kvp in questsProgress)
        {
            var quest = Quests.FindByName(kvp.Key);

            ShowQuestInUI(quest);

        }
    }

    public static List<Quest> GetActiveQuests()
    {
        List<Quest> active = new List<Quest>();

        foreach(var kvp in questsProgress)
        {
            var quest = Quests.FindByName(kvp.Key);
            if (quest.finishProgress != kvp.Value) active.Add(quest);
        }

        return active;
    }

    public static List<Quest> GetAvailableQuests()
    {
        List<Quest> available = new List<Quest>();

        foreach(var quest in Quests.quests)
        {
            // ne gledaj aktivne ili završene
            if(!questsProgress.ContainsKey(quest.name))
            {
                // ako ne ovisi o nikakvom quest-u dodaj
                if (quest.requiredQuestNames == null || quest.requiredQuestNames.Length == 0) available.Add(quest);
                else
                {
                    bool add = true;
                    foreach(var reqQuest in quest.requiredQuestNames)
                    {
                        add = add & questsProgress.ContainsKey(reqQuest) && 
                            questsProgress[reqQuest] == Quests.FindByName(reqQuest).finishProgress;
                    }
                    if (add) available.Add(quest);
                }
            }
        }


        return available;
    }

    public static void EnemyKilled(EnemyController.EnemyType enemyType)
    {
        foreach(var kvp in questsProgress)
        {
            // aktivan quest koji nije završen
            var quest = Quests.FindByName(kvp.Key);
            if (quest.finishProgress != kvp.Value)
            {
               if (quest.questType == Quest.QuestType.KillEnemy)
                {
                    if(enemyType == quest.enemyType)
                    {
                        ProgressQuest(quest);
                    }
                }
            }
        }
    }


    public static void ItemCollected(int id)
    {
        Quest temp = null;
        foreach (var kvp in questsProgress)
        {
            // aktivan quest koji nije završen
            var quest = Quests.FindByName(kvp.Key);
            if (quest.finishProgress != kvp.Value)
            {
                if (quest.questType == Quest.QuestType.CollectItems)
                {
                    foreach(var itemId in quest.itemIds)
                    {
                        if(id == itemId)
                        {
                            temp = quest;
                            break;
                        }
                    }
                }
            }
        }
        if (temp != null) ProgressQuest(temp);
    }

    public static void ArriveAtLocation(string location)
    {
        Quest temp = null;
        foreach (var kvp in questsProgress)
        {
            // aktivan quest koji nije završen
            var quest = Quests.FindByName(kvp.Key);
            if (quest.finishProgress != kvp.Value)
            {
                if (quest.questType == Quest.QuestType.GoToLocation)
                {
                  if(location == quest.locationName)
                    {
                        temp = quest;
                        break;
                    }
                }
            }
        }
        if (temp != null) ProgressQuest(temp);
    }



    static void ProgressQuest(Quest quest)
    {

        questsProgress[quest.name]++;
        if (questsProgress[quest.name] >= quest.finishProgress)
        {
            QuestCompleted(quest);
        }
        else
        {
            RefreshQuestUI(quest);
        }
    }

    /*
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!GetActiveQuests().Contains(Quests.FindByName("Village")))
                AcceptQuest("Village");
            else
            {
                ArriveAtLocation("Village");
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
           if(!GetActiveQuests().Contains(Quests.FindByName("Silk")))
            AcceptQuest("Silk");
           else
            {
                ItemCollected(1);
            }
        }
    }
    */
}
