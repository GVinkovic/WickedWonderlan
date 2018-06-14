using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : QuestLocation {
    public int sceneIndex = 3;
    public override void ArrivedAtLocation()
    {
        base.ArrivedAtLocation();
        if (QuestManager.IsQuestCompleted("Portal"))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
