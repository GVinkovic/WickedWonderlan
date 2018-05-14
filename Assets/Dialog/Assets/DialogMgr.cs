using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogMgr : MonoBehaviour
{
    public GameObject DialogBox; // the dialog box
    public Text DialogActorNameText; // the text object in the dialog box to render an actor's name
    public Text DialogText; // the text object in the dialog box to render the dialog text
    public Button ActionButton1; // reference to the first action button in the dialog box
    public Button ActionButton2; // reference to the second action button in the dialog box

    // key the user has to press to progress through the dialogs
    public KeyCode NextDialogKey = KeyCode.Return; // enter key

    // use this flag to deal with the game state when dialogs are in progress
    // e.g., stop the player from moving etc.
    [HideInInspector] public bool InProgress;

    private bool _skipDialog;

    private void Awake()
    {
        this._Reset();
    }

    // use as a global accessor
    public static DialogMgr Instance { get { return FindObjectOfType<DialogMgr>(); } }

    public void BeginDialog(List<Dialog> dialogs)
    {
        if (this.InProgress)
            return;
        this.InProgress = true;
        this.DialogBox.SetActive(true);
        this.StartCoroutine(this.DialogRoutine(dialogs));
    }

    public void SkipDialog()
    {
        this.StartCoroutine(this.SkinDialogRoutine());
    }

    private IEnumerator DialogRoutine(List<Dialog> dialogs)
    {
        // go over all of the dialogs
        foreach (var dialog in dialogs)
        {
            //Debug.Log("Dialog itteration");

            // update the actor name text
            this.DialogActorNameText.text = dialog.ActorName;

            // determines if the dialog has action buttons
            bool hasAction = false;

            // enable the first action button if the user has specified so
            if (!string.IsNullOrEmpty(dialog.ActionButton1Text))
            {
                hasAction = true;
                this.ActionButton1.GetComponentInChildren<Text>().text = dialog.ActionButton1Text;
                this.ActionButton1.gameObject.SetActive(true);
                this.ActionButton1.onClick.RemoveAllListeners();
                this.ActionButton1.onClick.AddListener(dialog.Action1);
                if (dialog.Action1Next)
                    this.ActionButton1.onClick.AddListener(this.SkipDialog);
            }
            else
                this.ActionButton1.gameObject.SetActive(false);

            // enable the second action button if the user has specified so
            if (!string.IsNullOrEmpty(dialog.ActionButton2Text))
            {
                hasAction = true;
                this.ActionButton2.GetComponentInChildren<Text>().text = dialog.ActionButton2Text;
                this.ActionButton2.gameObject.SetActive(true);
                this.ActionButton2.onClick.RemoveAllListeners();
                this.ActionButton2.onClick.AddListener(dialog.Action2);
                if (dialog.Action2Next)
                    this.ActionButton2.onClick.AddListener(this.SkipDialog);
            }
            else
                this.ActionButton2.gameObject.SetActive(false);

            // begin going through dialog lines
            foreach (var line in dialog.DialogLines)
            {
                yield return new WaitForEndOfFrame();

                this.DialogText.text = line;

                bool preventNextOnAction = hasAction && dialog.PreventNextOnAction;

                // next message
                while (preventNextOnAction || !Input.GetKeyDown(this.NextDialogKey))
                {
                    if (this._skipDialog)
                        break;
                    yield return null;
                }

                // wait a frame before the next iteration
                yield return new WaitForEndOfFrame();
            }
        }
        this._Reset();
    }

    private void _Reset()
    {
        this.InProgress = false;
        this.DialogBox.SetActive(false);
        this.ActionButton1.gameObject.SetActive(false);
        this.ActionButton2.gameObject.SetActive(false);
    }

    private IEnumerator SkinDialogRoutine()
    {
        this._skipDialog = true;
        yield return null;
        this._skipDialog = false;
    }

    // represents a dialog
    public class Dialog
    {
        public string ActorName; // name of the dialog actor
        public List<string> DialogLines; // collection of lines of the dialog
        public string ActionButton1Text; // name of the first action, leave empty if none
        public string ActionButton2Text; // name of the second action, leave empty for none
        public UnityAction Action1=new UnityAction(()=> { }); // code to execute when the first action is taken
        public UnityAction Action2=new UnityAction(()=> { }); // code to execute when the second action is taken
        public bool PreventNextOnAction=true;
        public bool Action1Next=true;
        public bool Action2Next=true;
    }
}