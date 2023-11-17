using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Event")]
public class Event : ScriptableObject, ISerializationCallbackReceiver
{
    public string eventName;
    [TextArea]
    public string eventDescription;
    public float chanceToTrigger = 100f;

    public bool randomEvent = false;
    public bool outsideEvent = false;

    public bool activated = true;
    [System.NonSerialized]
    public bool isActivated;

    public bool running = false;
    [System.NonSerialized]
    public bool isRunning;
    [System.NonSerialized]
    public bool isDone;
    
    [System.NonSerialized]
    public bool onceADay;
    [System.NonSerialized]
    public int nextTimeToRepeat;

    [Space(5)]
    [Header("Boolean switch events?")]
    public List<BooleanSwitch> booleanConditions = new List<BooleanSwitch>();

    [Header("Player options")]
    public List<string> optionText = new List<string>();
    public List<Reaction> optionReaction = new List<Reaction>();

    public void OnAfterDeserialize()
    {
        isRunning = running;
        isDone = false;
        isActivated = activated;
        onceADay = false;
        nextTimeToRepeat = 0;
    }

    public void OnBeforeSerialize()
    { }

    public void LoadEventData(EventSave data)
    {
        isRunning = data.isrunning;
        isDone = data.isdone;
        isActivated = data.isactivated;
        onceADay = data.onceaday;
        nextTimeToRepeat = data.nexttimetorepeat;
    }

    public bool TryTrigger()
    {
        float roll = Random.Range(0f, 100f);

        Debug.Log("Trying to trigger " + name);
        if (!isActivated) return false;
        Debug.Log("--- is activated!");
        if (isDone) return false;
        Debug.Log("--- is not done!");
 
        // is bool triggered?
        if (booleanConditions.Count > 0)
        {
            for (int i = 0; i < booleanConditions.Count; i++)
            {
                if (!booleanConditions[i].runValue)
                {
                    //don't trigger
                    return false;
                }
            }
        }
        Debug.Log("--- passed boolean conditions...");

        // After we check the MUST conditions, we check the chance to trigger
        if (roll <= chanceToTrigger)
        {
            isRunning = true;
            isDone = false;
            GameController.controller.eventRunning = true;
            GameController.controller.theEventRunning = this;
            Debug.Log("TRIGGERED with roll of " + roll + " against DC of " + chanceToTrigger);
            return true;
        }
        else
        {
            Debug.Log("Not triggered! Roll of " + roll + " failed against DC of " + chanceToTrigger);
            return false;
        }

    }

    public void ExecutePlayerOption(int option)
    {
        if (optionReaction.Count == 0)
        {
            GameController.controller.ReturnToMainWindowCommand();
            return;
        }

        if (optionReaction[option] != null)
        {
            optionReaction[option].ExecuteReaction(GameController.controller);
        }
        else
        {
            GameController.controller.ReturnToMainWindowCommand();
        }
    }
}
