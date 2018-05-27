using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Interactable {

    [System.Serializable]
    public class DialogsQuests
    {
        public string dialogName;
        public string[] questsNames;
    }

    public DialogsQuests[] questsForDialog;

    private string currentDialogName;

    public override void Interact()
    {
        base.Interact();


        if (!QuestManager.HasActiveQuests())
        {
            GiveQuest();
        }
    }

    public virtual void GiveQuest()
    {
        foreach (var kvp in questsForDialog)
        {
            bool questCompleted = true;
            foreach (var questName in kvp.questsNames)
            {
                questCompleted = questCompleted && QuestManager.IsQuestCompleted(questName);
            }

            if (!questCompleted)
            {
                ShowDialog(kvp.dialogName);
                break;
            }
        }
    }
    public override void StopInteract()
    {
        base.StopInteract();
        currentDialogName = null;
        GameManager.GetButtonsDialogMgr.EndDialog();
    }

    void ShowDialog(string name)
    {
        currentDialogName = name;
        GameManager.GetButtonsDialogMgr.BeginDialog(GameManager.GetDialogsCollection.getDialogList(name));
        DialogMgr.OnDialogEnd += OnDialogEnd;
    }
    void OnDialogEnd()
    {
        DialogMgr.OnDialogEnd -= OnDialogEnd;

        if (currentDialogName != null)
        {

            foreach(var questName in Find(currentDialogName).questsNames)
            {
                QuestManager.AcceptQuest(questName);
            }

            currentDialogName = null;
        }

    }
    DialogsQuests Find(string dialogName)
    {
        foreach(var dialogQuest in questsForDialog)
        {
            if (dialogQuest.dialogName == dialogName) return dialogQuest;
        }
        return null;
    }
}
