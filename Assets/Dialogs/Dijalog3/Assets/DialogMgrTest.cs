using System.Collections.Generic;
using UnityEngine;

public class DialogMgrTest : MonoBehaviour
{
    private void Start()
    {
        List<DialogMgr.Dialog> dialogs = new List<DialogMgr.Dialog>()
        {
            // dialog #1
            new DialogMgr.Dialog()
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
            new DialogMgr.Dialog()
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
            new DialogMgr.Dialog()
            {
                ActorName="PoglavicaSela",
                DialogLines=new List<string>()
                {
                    "Upute. Ako si razumio, klikni na NEXT!"
                },
                ActionButton1Text="Next"
            },
            // dialog #4
            new DialogMgr.Dialog()
            {
                ActorName="PoglavicaSela",
                DialogLines=new List<string>()
                {
                    "Upute. Ako si sve razumio klikni na CLOSE i nastavi GAME",
                    "Upute. Za detaljnije objašnjenje klikni na MORE. "
                },
                ActionButton1Text="Close",
                ActionButton2Text="More",
                Action2=new UnityEngine.Events.UnityAction(()=>{ Debug.Log("Detaljnije upute"); }),
                Action2Next=false
            }
        };
        DialogMgr.Instance.BeginDialog(dialogs);
    }
}
