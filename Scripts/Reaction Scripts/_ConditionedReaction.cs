using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/_Conditioned reaction")]
public class _ConditionedReaction : Reaction
{
    public List<BooleanSwitch> conditions = new List<BooleanSwitch>();
    public List<bool> values = new List<bool>();

    public Reaction _valueIfTrue;
    public Reaction _valueIfFalse;

    public override void ExecuteReaction(GameController controller)
    {
        if (conditions.Count == 0 || values.Count == 0)
        {
            Debug.Log("conditional reaction with no conditions!");
            if (_valueIfTrue != null) _valueIfTrue.ExecuteReaction(controller);
        }

        bool result = conditions[0].runValue == values[0];
        for (int i = 1; i < conditions.Count; i++)
        {
            result = result && (conditions[i].runValue == values[i]);
        }

        if (result)
        {
            if(_valueIfTrue != null) _valueIfTrue.ExecuteReaction(controller);
        }
        else
        {
            if (_valueIfFalse != null) _valueIfFalse.ExecuteReaction(controller);
        }
    }
}
