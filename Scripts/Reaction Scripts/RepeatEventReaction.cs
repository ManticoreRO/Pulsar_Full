using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Events/Repeat event timely")]
public class RepeatEventReaction : Reaction
{
    public Event whichEvent;
    public int timeAdded;

    public override void ExecuteReaction(GameController controller)
    {
        whichEvent.nextTimeToRepeat += timeAdded;
        whichEvent.isActivated = true;
        whichEvent.isDone = false;
        whichEvent.isRunning = true;
        whichEvent.onceADay = false;
    }
}
