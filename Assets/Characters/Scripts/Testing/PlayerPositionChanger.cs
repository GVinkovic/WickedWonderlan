using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionChanger : MonoBehaviour {

    [System.Serializable]
    public class PositionEntry
    {
        public KeyCode hotKey;
        public Vector3 position;
    }

    public PositionEntry[] positions;

    void Update () {
	    foreach(var posEntry in positions)
        {
            if (Input.GetKeyDown(posEntry.hotKey))
            {
                PlayerManager.Player.transform.position = posEntry.position;
                PlayerManager.RhinoPosition = new Vector3(posEntry.position.x + 1, posEntry.position.y, posEntry.position.z);

                break;
            }
        }
	}
}
