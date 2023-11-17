using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Events/End Event")]
public class EndEventReaction : Reaction
{
    public Event whichEvent;
    [TextArea]
    public string endOfEventDescription = "";

    public override void ExecuteReaction(GameController controller)
    {
        whichEvent.isRunning = false;
        whichEvent.isDone = true;
        whichEvent.isActivated = false;
        controller.eventRunning = false;

        if (endOfEventDescription != "") controller.AddTextWithReturn(endOfEventDescription, Color.yellow);
        controller.ReturnToMainWindowCommand();
    }
}
