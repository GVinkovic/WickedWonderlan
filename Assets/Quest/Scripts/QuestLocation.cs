using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour {

    public string LocationName;

    void OnTriggerEnter(Collider other)
    {
        ArrivedAtLocation();
    }

    void OnCollisionEnter(Collision collision)
    {
        ArrivedAtLocation();
    }

    public virtual void ArrivedAtLocation()
    {
        QuestManager.ArriveAtLocation(LocationName);
    }
}
