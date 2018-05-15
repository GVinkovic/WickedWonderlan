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
    public Button LeftActionButon; // reference to the first action button in the dialog box
    public Button RightActionButton; // reference to the second action button in the dialog box
    public Text TooltipText; // reference to the tooltip text shown when no buttons are shown
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

            if (LeftActionButon)
            {
                // enable the first action button if the user has specified so
                if (!string.IsNullOrEmpty(dialog.LeftActionButtonText))
                {
                    hasAction = true;
                    this.LeftActionButon.GetComponentInChildren<Text>().text = dialog.LeftActionButtonText;
                    this.LeftActionButon.gameObject.SetActive(true);
                    // this.LeftActionButon.onClick.RemoveAllListeners();
                    //       this.LeftActionButon.onClick.AddListener(dialog.LeftButtonAction);
                    this.LeftActionButon.onClick = dialog.LeftButtonAction;
                    if (dialog.ActionLeftNext)
                        this.LeftActionButon.onClick.AddListener(this.SkipDialog);
                }
                else
                    this.LeftActionButon.gameObject.SetActive(false);

            }
            if (RightActionButton)
            {
                // enable the second action button if the user has specified so
                if (!string.IsNullOrEmpty(dialog.RightActionButtonText))
                {
                    hasAction = true;
                    this.RightActionButton.GetComponentInChildren<Text>().text = dialog.RightActionButtonText;
                    this.RightActionButton.gameObject.SetActive(true);
                    //                    this.RightActionButton.onClick.RemoveAllListeners();
                    //    this.RightActionButton.onClick.AddListener(dialog.RightButtonAction);
                    this.RightActionButton.onClick = dialog.RightButtonAction;
                    if (dialog.ActionRightNext)
                        this.RightActionButton.onClick.AddListener(this.SkipDialog);
                }
                else
                    this.RightActionButton.gameObject.SetActive(false);

            }

            if (!hasAction)
            {
                TooltipText.gameObject.SetActive(true);
            }
            else
            {
                TooltipText.gameObject.SetActive(false);
            }

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
        if(LeftActionButon) this.LeftActionButon.gameObject.SetActive(false);
        if(RightActionButton) this.RightActionButton.gameObject.SetActive(false);
        this.TooltipText.gameObject.SetActive(false);
    }

    private IEnumerator SkinDialogRoutine()
    {
        this._skipDialog = true;
        yield return null;
        this._skipDialog = false;
    }


}