using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour {

    public string LocationName;

    void OnTriggerEnter(Collider other)
    {
        QuestManager.ArriveAtLocation(LocationName);    
    }

    void OnCollisionEnter(Collision collision)
    {
        QuestManager.ArriveAtLocation(LocationName);
    }
}
