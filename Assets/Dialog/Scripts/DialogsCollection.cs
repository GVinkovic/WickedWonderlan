using System.Collections.Generic;
using UnityEngine;


// anotacija koja u unity meni dodaje opciju kreiranja novog asset-a, odnosno nove instance ovog scriptable objekta
[CreateAssetMenu(menuName = "Dialogs/Dialogs Collection")]
public class DialogsCollection : ScriptableObject {

    // Jedan scenarij
    // sadrži ime scenarija
    // i listu dijaloga za taj scenarij
    [System.Serializable]
    public class DialogEntry
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public List<Dialog> dialogs;
    }


    [SerializeField]
    public List<DialogEntry> dialogsCollection; // lista scenaria, jedan scenarij sadrži ime i listu dialoga

    // metoda koja pretražuje scenarije po imenu, i vrača listu dialoga ako pronađe traženi scenario
    // inače vraća praznu listu
    public List<Dialog> getDialogList(string name)
    {
        foreach(var dialogEntry in dialogsCollection)
        {
            if (name == dialogEntry.Name) return dialogEntry.dialogs;
        }
        return new List<Dialog>();
    }
}
