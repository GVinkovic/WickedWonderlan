using System.Collections.Generic;
using UnityEngine;

public class DialogMgrTest : MonoBehaviour
{

    public Dialog testDialog;

    private void Start()
    {
        List<Dialog> dialogs = new List<Dialog>()
        {
            // dialog #1
            new Dialog()
            {
                ActorName="Kompanjon",
                DialogLines =new List<string>()
                {
                    "Prvi prozor dijaloga- upute!",
                    "Drugi prozor dijaloga- upute.",
                    "Treći prozor dijaloga- upute."
                }
            },
            // dialog #2
            new Dialog()
            {
                ActorName="Igrač",
                DialogLines = new List<string>()
                {
                    "Igračev odgovor!",
                    "Igračeva pitanja.",
                    "Igračeva pitanja."
                }
            },
            // dialog #3
            new Dialog()
            {
                ActorName="PoglavicaSela",
                DialogLines=new List<string>()
                {
                    "Upute. Ako si razumio, klikni na NEXT!"
                },
                RightActionButtonText="Next"
            },
            // dialog #4
            new Dialog()
            {
                ActorName="PoglavicaSela",
                DialogLines=new List<string>()
                {
                    "Upute. Ako si sve razumio klikni na CLOSE i nastavi GAME",
                    "Upute. Za detaljnije objašnjenje klikni na MORE. "
                },
                LeftActionButtonText="Close",
                RightActionButtonText="More",
             //   RightButtonAction=new UnityEngine.Events.UnityAction(()=>{ Debug.Log("Detaljnije upute"); }),
                ActionRightNext=false
            }
        };
        DialogMgr.Instance.BeginDialog(dialogs);
    }
}        
