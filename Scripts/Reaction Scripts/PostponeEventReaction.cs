using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add/Reactions/Events/Postpone event")]
public class PostponeEventReaction : Reaction
{
    public Event whichEvent;

    public override void ExecuteReaction(GameController controller)
    {
        whichEvent.isActivated = true;
        whichEvent.isDone = false;
        whichEvent.isRunning = false;
        whichEvent.onceADay = false;
        controller.eventRunning = false;
    }
}
