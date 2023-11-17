using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommonProperties : ISerializationCallbackReceiver
{
    public float chanceToFind = 100f;
    public bool visibleWhenUnlit = false;
    public bool forceRequireLight = false;
    public bool secret = false;
    [TextArea]
    public string descriptionWhenFound = "";
    [TextArea]
    public string descriptionWhenInspected = "";

    public Reaction reactionsOnUse;

    public void OnAfterDeserialize()
    { }

    public void OnBeforeSerialize()
    { }
}
