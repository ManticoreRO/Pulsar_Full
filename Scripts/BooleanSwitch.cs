using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Boolean switch")]
public class BooleanSwitch : ScriptableObject, ISerializationCallbackReceiver
{
    public bool value = false;
    [System.NonSerialized]
    public bool runValue;
    [System.NonSerialized]
    public bool executedReaction;

    public Reaction onChangedReaction;

    public void OnAfterDeserialize()
    {
        runValue = value;
        executedReaction = false;
    }

    public void OnBeforeSerialize()
    { }

    public void CheckAndReact()
    {
        if (runValue != value && onChangedReaction != null && !executedReaction)
        {
            executedReaction = true;
            onChangedReaction.ExecuteReaction(GameController.controller);
        }

    }
}
