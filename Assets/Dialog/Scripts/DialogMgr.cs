using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogMgr : MonoBehaviour
{
    public GameObject DialogBox; // referenca na GameObject koji predstavlja dialog holder
    public Text DialogActorNameText; // referenca na text GameObject u kojem će se prikazivat ime trenutnog aktera
    public Text DialogText; // referenca na text GameObject u kojem se prikazuje tekst dialoga
    public Button LeftActionButon; // referenca na lijevi GameObject button
    public Button RightActionButton; // referenca na desni GameObject button
    public Text TooltipText; // referenca na text GameObject koji prikazuje tooltip text ('press nesto to continue')

    // tipka sa kojom se prelazi na sljedeći dialog
    public KeyCode NextDialogKey = KeyCode.Return; // enter key

    [HideInInspector] public bool InProgress;

    // zastavica koja se postavlja na true kada se klikne na button, 
    // ako je na button-u označena akcija ActionButtonNext
    private bool _skipDialog;

    // sakrij GameObject komponente dialoga kada se inicializira GameObject dialoga
    private void Awake()
    {
        this._Reset();
    }

    // use as a global accessor
    public static DialogMgr Instance { get { return FindObjectOfType<DialogMgr>(); } }

    // metoda koju se pozove za početak prikazivanja dialoga
    public void BeginDialog(List<Dialog> dialogs)
    {
        // ako se trenutno prikazuju dialozi u ovom dialog menageru onda ne radi nista
        if (this.InProgress) return;

        // signaliziraj da se dialozi prikazuju
        this.InProgress = true;
        // prikaži dialog 
        this.DialogBox.SetActive(true);
        // započni prikazivanje liste dialoga 
        this.StartCoroutine(this.DialogRoutine(dialogs));
    }

    // metoda koja se poziva na klik buttona ako je označeno svojstvo ActionButtonNext
    // prelazi na sljedeći dialog
    public void SkipDialog()
    {
        this.StartCoroutine(this.SkinDialogRoutine());
    }


    // metoda koja prolazi kroz sve dialoge i izmjenjuje ih kako korisnik interakta s njima
    private IEnumerator DialogRoutine(List<Dialog> dialogs)
    {
        // za svaki dialog iz liste dialoga 
        foreach (var dialog in dialogs)
        {

            // postavi trenutno ime aktora koji priča
            this.DialogActorNameText.text = dialog.ActorName;

            // zastavica koja će označit dali ima button akcija
            bool hasAction = false;

            // ako je prenesen GameObject za lijevi button
            if (LeftActionButon)
            {
                // ako je upisan text za lijevi button
                if (!string.IsNullOrEmpty(dialog.LeftActionButtonText))
                {
                    // postavi zastavicu da postoji baren jedan button
                    hasAction = true;
                    // postavi text lijevog buttona na text lijevog gumba u dialogu (LeftActionButtonText)
                    this.LeftActionButon.GetComponentInChildren<Text>().text = dialog.LeftActionButtonText;
                    // prikaži lijevi button, postavi ga aktivnog
                    this.LeftActionButon.gameObject.SetActive(true);
                    
                    // this.LeftActionButon.onClick.RemoveAllListeners();
                    //       this.LeftActionButon.onClick.AddListener(dialog.LeftButtonAction);

                    // na klik buttona pozovi akcije definirane za lijevi button
                    this.LeftActionButon.onClick = dialog.LeftButtonAction;
                    // ako je postavljena ActionLeftNext na true
                    // onda dodaj još jedan slušač na button
                    // koji prelazi na sljedeći dialog
                    if (dialog.ActionLeftNext)
                        this.LeftActionButon.onClick.AddListener(this.SkipDialog);
                }
                else
                    // nije upisan text za lijevi button, zato ga sakrij
                    this.LeftActionButon.gameObject.SetActive(false);

            }
            if (RightActionButton)
            {
                // ako je upisan text za desni button
                if (!string.IsNullOrEmpty(dialog.RightActionButtonText))
                {
                    // postavi zastavicu da postoji baren jedan button
                    hasAction = true;
                    // postavi text desnog buttona na text desnog gumba u dialogu (RightActionButtonText)
                    this.RightActionButton.GetComponentInChildren<Text>().text = dialog.RightActionButtonText;
                    // prikaži desni button, postavi ga aktivnog
                    this.RightActionButton.gameObject.SetActive(true);

                    //                    this.RightActionButton.onClick.RemoveAllListeners();
                    //    this.RightActionButton.onClick.AddListener(dialog.RightButtonAction);

                    // na klik buttona pozovi akcije definirane za desni button
                    this.RightActionButton.onClick = dialog.RightButtonAction;
                    // ako je postavljena ActionLeftNext na true
                    // onda dodaj još jedan slušač na button
                    // koji prelazi na sljedeći dialog
                    if (dialog.ActionRightNext)
                        this.RightActionButton.onClick.AddListener(this.SkipDialog);
                }
                else
                    // nije upisan text za desni button, zato ga sakrij
                    this.RightActionButton.gameObject.SetActive(false);

            }
            //ako nije prikazan ni jedan button onda prikaži tooltip text GameObject
            if (!hasAction)
            {
                TooltipText.gameObject.SetActive(true);
            }
            // prikazan je baren jedan button, sakrij tooltip 
            else
            {
                TooltipText.gameObject.SetActive(false);
            }

            // prođi kroz svaku liniju unutar dialoga
            foreach (var line in dialog.DialogLines)
            {
                // pričekaj kraj trenutnog renredirajućeg frame-a
                yield return new WaitForEndOfFrame();

                // postavi text dialog na liniju dialoga koju 
                // akter trenutno govori
                this.DialogText.text = line;

                bool preventNextOnAction = hasAction && dialog.PreventNextOnAction;

                // ako nema buttona čekaj da korisnik klikne key za sljedeći dialog
                while (preventNextOnAction || !Input.GetKeyDown(this.NextDialogKey))
                {
                    // ako je igrač kliknua na button kojem je postavljeno da
                    // prelazi na sljedeći dialog, onda je _skipDialog variabla postavljena na true
                    // u tom slučaju prekini čekanje na korisničku akciju
                    if (this._skipDialog) break;
                    yield return null;
                }

                // pričekaj kraj trenutnog frame-a prije sljedećeg dialoga
                yield return new WaitForEndOfFrame();
            }
        }
        // kada su svi dialozi prikazani, resetiraj postavke i sakrij dialog
        this._Reset();
    }

    private void _Reset()
    {
        // trenutno se ne prikazuju dialozi, nije InProgress
        this.InProgress = false;
        // sakrij dialog
        this.DialogBox.SetActive(false);
        // ako je GameObject lijevog buttona postavljen onda ga sakrij
        if(LeftActionButon) this.LeftActionButon.gameObject.SetActive(false);
        // ako je GameObject desnog buttona postavljen onda ga sakrij
        if (RightActionButton) this.RightActionButton.gameObject.SetActive(false);
        // sakrij tooltip text
        this.TooltipText.gameObject.SetActive(false);
    }

    private IEnumerator SkinDialogRoutine()
    {
        this._skipDialog = true;
        yield return null;
        this._skipDialog = false;
    }


}