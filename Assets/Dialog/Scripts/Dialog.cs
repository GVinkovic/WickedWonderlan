using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// represents a dialog
[CreateAssetMenu(menuName = "Dialog")]
public class Dialog : ScriptableObject {
    [Tooltip("Ime aktora koji trenutno govori")]
    public string ActorName; // name of the dialog actor
    [Tooltip("Lista rečenica koje će aktor izreć u ovom dialogu.")]
    public List<string> DialogLines; // collection of lines of the dialog
    [Tooltip("Tekst koji će se prikazivati na lijevom button-u. Ako je prazno onda se button ne prikazuje")]
    public string LeftActionButtonText; // name of the first action, leave empty if none
    [Tooltip("Dali će se prikazat sljedeća linija na klik butona")]
    public bool ActionLeftNext = true;
    [Tooltip("Akcije koje će se pozvati kad se klikne button")]
    public Button.ButtonClickedEvent LeftButtonAction = new Button.ButtonClickedEvent(); // code to execute when the first action is taken
    [Tooltip("Tekst koji će se prikazivati na desnom button-u. Ako je prazno onda se button ne prikazuje")]
    public string RightActionButtonText; // name of the second action, leave empty for none
    [Tooltip("Dali će se prikazat sljedeća linija na klik butona")]
    public bool ActionRightNext = true;
    [Tooltip("Akcije koje će se pozvati kad se klikne button")]
    public Button.ButtonClickedEvent RightButtonAction = new Button.ButtonClickedEvent(); // code to execute when the second action is taken
    [Tooltip("Nemoj reagirati ni na bilo koje akcije")]
    public bool PreventNextOnAction = true;
   
}
