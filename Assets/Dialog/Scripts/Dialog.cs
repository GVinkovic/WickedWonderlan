using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialog {

    [Tooltip("Ime aktora koji trenutno govori")]
    public string ActorName; // ime aktora koje će se prikazivati za ovaj dialog

    [Tooltip("Lista rečenica koje će aktor izreć u ovom dialogu.")]
    public List<string> DialogLines; // lista linija dialoga koje će aktor izreć

    [Tooltip("Tekst koji će se prikazivati na lijevom button-u. Ako je prazno onda se button ne prikazuje")]
    public string LeftActionButtonText; // text na lijevom button-u, ako nema texta, ne prikazuje se button

    [Tooltip("Dali će se prikazat sljedeća linija na klik butona")]
    public bool ActionLeftNext = true; // dali klik na button-u automatski prelazi na sljedeći dialog ili liniju

    [Tooltip("Akcije koje će se pozvati kad se klikne button")]
    public Button.ButtonClickedEvent LeftButtonAction = new Button.ButtonClickedEvent(); // metode koje će se pozvati kad se klikne button

    [Tooltip("Tekst koji će se prikazivati na desnom button-u. Ako je prazno onda se button ne prikazuje")]
    public string RightActionButtonText; // text na desnom button-u, ako nema texta, ne prikazuje se button

    [Tooltip("Dali će se prikazat sljedeća linija na klik butona")]
    public bool ActionRightNext = true; // dali klik na button-u automatski prelazi na sljedeći dialog ili liniju

    [Tooltip("Akcije koje će se pozvati kad se klikne button")]
    public Button.ButtonClickedEvent RightButtonAction = new Button.ButtonClickedEvent(); // metode koje će se pozvati kad se klikne button

    [Tooltip("Nemoj reagirati ni na bilo koje akcije")]
    public bool PreventNextOnAction = true; // globalno ograničenje za sve buttone, ako je false ne reagira ni na jedan button
   
}
