using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Panel command button")]
public class PanelCommandButton : ScriptableObject, ISerializationCallbackReceiver
{
    public List<BooleanSwitch> conditions = new List<BooleanSwitch>();

    [TextArea]
    public List<string> messagesIfTrue = new List<string>();
    [TextArea]
    public List<string> messagesIfFalse = new List<string>();

    public Reaction reactionWhenTrue;

    public bool disableAfterUse = true;
    public bool enabled = true;
    [System.NonSerialized]
    public bool isEnabled;

    public void OnAfterDeserialize()
    {
        isEnabled = enabled;
    }

    public void OnBeforeSerialize()
    { }

    public string Messages()
    {
        string textToReturn = "";

        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i].runValue)
            {
                if (messagesIfTrue[i] != "") textToReturn += "\n" + messagesIfTrue[i];
            }
            else
            {
                if (messagesIfFalse[i] != "") textToReturn += "\n" + messagesIfFalse[i];
            }
        }

        return textToReturn;
    }

    public void ExecuteIfTrue()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (!conditions[i].runValue) return;
        }
        // if we are here, we can execute
        if (reactionWhenTrue != null)
        {
            reactionWhenTrue.ExecuteReaction(GameController.controller);

            if (disableAfterUse) isEnabled = false;
        }
    }
}
