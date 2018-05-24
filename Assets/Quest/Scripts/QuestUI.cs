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
        StartCoroutine(FadeCorutine(-0.1f));
        //TODO: neki zvuk
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
