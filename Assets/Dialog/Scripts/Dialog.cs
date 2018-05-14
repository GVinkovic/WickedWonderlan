using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// represents a dialog
[CreateAssetMenu(menuName = "Dialog")]
public class Dialog : ScriptableObject {

    public string ActorName; // name of the dialog actor
    public List<string> DialogLines; // collection of lines of the dialog
    public string LeftActionButtonText; // name of the first action, leave empty if none
    public bool ActionLeftNext = true;
    public Button.ButtonClickedEvent LeftButtonAction = new Button.ButtonClickedEvent(); // code to execute when the first action is taken
    public string RightActionButtonText; // name of the second action, leave empty for none
    public bool ActionRightNext = true;
    public Button.ButtonClickedEvent RightButtonAction = new Button.ButtonClickedEvent(); // code to execute when the second action is taken
    public bool PreventNextOnAction = true;
   
}
