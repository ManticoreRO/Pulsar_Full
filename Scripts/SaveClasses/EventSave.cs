using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventSave
{
    public bool isrunning;
    public bool isdone;
    public bool isactivated;
    public bool onceaday;
    public int nexttimetorepeat;

    public EventSave()
    {

    }

    public EventSave(Event ev)
    {
        isrunning = ev.isRunning;
        isdone = ev.isDone;
        isactivated = ev.isActivated;
        onceaday = ev.onceADay;
        nexttimetorepeat = ev.nextTimeToRepeat;
    }
}
