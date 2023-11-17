using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Terminal Log")]
public class TerminalLog : ScriptableObject, ISerializationCallbackReceiver
{
    public string timeStamp;
    public string logLabel;
    [TextArea]
    public string messageText;

    public bool locked = true;
    [System.NonSerialized]
    public bool isLocked;

    public bool viewed = false;
    [System.NonSerialized]
    public bool isViewed;

    public Reaction reactionUponReading;

    public void OnAfterDeserialize()
    {
        isLocked = locked;
        isViewed = viewed;
    }

    public void OnBeforeSerialize()
    { }

    public string LogString()
    {
        string textToReturn = "";

        textToReturn += "Date: " + timeStamp + "\n";
        textToReturn += "Title: " + logLabel + "\n";
        textToReturn += "Contents:\n" + messageText;

        return textToReturn;
    }
}
