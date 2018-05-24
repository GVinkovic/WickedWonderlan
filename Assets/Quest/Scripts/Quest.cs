using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

    
    public enum QuestType
    {
        KillEnemy,
        GoToLocation,
        CollectItems
    }

    [Serializable]
    public struct ArrayWrapper
    {
        public int[] items;

    }


    public string name;

    public string displayName;

    public string description;

    public QuestType questType;


    [ConditionalField("questType", QuestType.KillEnemy)]
    public EnemyController.EnemyType enemyType;

    [ConditionalField("questType", QuestType.GoToLocation)]
    public string locationName;

    [ConditionalField("questType", QuestType.CollectItems)]
    public ArrayWrapper ItemIds;


    public int finishProgress = 1;

    public string[] requiredQuestNames;

   // public string requiredQuestStepErroMsg;
    
    public int[] itemIds
    {
        get { return ItemIds.items; }
    }
}
