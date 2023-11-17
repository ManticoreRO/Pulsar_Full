using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Add/Reactions/Events/Start event")]
public class StartEventReaction : Reaction
{
    public Event whichEvent;
    public bool forceStart = false;

    public override void ExecuteReaction(GameController controller)
    {
        if (whichEvent == null)
        {
            Debug.Log("Ooops! " + name + " is calling a null whichEvent!");
            return;
        }

        if (whichEvent.TryTrigger())
        {
            GameController.controller.eventRunning = true;
            GameController.controller.theEventRunning = whichEvent;

            if (forceStart) GameController.controller.ShowEventScreen(whichEvent);
        }
    }
}
