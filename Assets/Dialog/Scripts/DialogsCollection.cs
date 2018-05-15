using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Dialogs/Dialogs Collection")]
public class DialogsCollection : ScriptableObject {

    [System.Serializable]
    public class DialogEntry
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public List<Dialog> dialogs;
    }
    [SerializeField]
    public List<DialogEntry> dialogsCollection;

    public List<Dialog> getDialogList(string name)
    {
        foreach(var dialogEntry in dialogsCollection)
        {
            if (name == dialogEntry.Name) return dialogEntry.dialogs;
        }
        return new List<Dialog>();
    }
}
