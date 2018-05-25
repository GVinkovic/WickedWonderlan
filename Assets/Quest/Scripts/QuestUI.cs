using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{

    public Text questName;
    public Text questDescription;

    public Quest quest { get; set; }


    public void UpdateUI()
    {
        questName.text = quest.displayName;
        questDescription.text = quest.description;
        var progress = QuestManager.GetQuestProgressText(quest);
        if (progress != null)
        {
            questDescription.text += "\n" + progress;
        }
    }

    public void Remove()
    {
        UpdateUI();
        if (quest.rewardItems == null || quest.rewardItems.Length == 0) StartCoroutine(FadeCorutine(-0.1f));
        else StartCoroutine(ShowRecievedItems());
        //TODO: neki zvuk
    }
    public void RemoveImmediate()
    {
        Destroy(gameObject);
    }

    IEnumerator ShowRecievedItems()
    {
        yield return new WaitForSeconds(1);
        string tekst = "";
        foreach(var rewardItem in quest.rewardItems)
        {
            tekst += rewardItem.quantity + " " + GameManager.ItemDatabase.getItemByID(rewardItem.itemId).itemName +", ";
        }
        questDescription.text = "Recieved: " + tekst.Substring(0, tekst.Length - 2);

        yield return new WaitForSeconds(3);

        StartCoroutine(FadeCorutine(-0.1f));

    }
    IEnumerator FadeCorutine(float delta)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha <= 1 && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha += delta;
            yield return new WaitForSeconds(.1f);
        }
        Destroy(gameObject);

    }
}
