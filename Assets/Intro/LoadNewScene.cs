using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{

    // Use this for initialization
    public void loadWorld(string World)
    {
        SceneManager.LoadScene(World);
    }
}