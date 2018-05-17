using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    
    private string theCollider;

    void OnTriggerEnter(Collider other)
    {
        theCollider = other.tag;
        if (theCollider == "Player")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            audio.loop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        theCollider = other.tag;
        if (theCollider == "Player")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Stop();
        }
    }
}