using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Interactible")]
public class Interactible : ScriptableObject, ISerializationCallbackReceiver
{
    public Area locatedInArea;

    public string actionVerb = "Use";

    public CommonProperties properties;

    public List<BooleanSwitch> foundIfConditions = new List<BooleanSwitch>();
    public List<BooleanSwitch> usedAloneIfConditions = new List<BooleanSwitch>();

    public bool found = false;
    [System.NonSerialized]
    public bool isFound;

    public bool destroyed = false;
    [System.NonSerialized]
    public bool isDestroyed;

    public bool destroyOnUse = false;
    public bool stillVisibleAfterDestroyed = false;

    public bool staticInteractible = false;

    public Reaction reactionOnInspect;

    public void OnAfterDeserialize()
    {
        isFound = found;
        isDestroyed = destroyed;
    }

    public void OnBeforeSerialize()
    { }

    public void LoadInteractibleData(InteractibleSave data)
    {
        isFound = data.isfound;
        isDestroyed = data.isdestroyed;
    }

    // The use command is taken from here.
    public void Use(GameController controller)
    {
        // if destroyed for some reason, exit
        if (isDestroyed) return;
        if (staticInteractible) return;

        if (!UsedAlone())
        {
            GameController.controller.secondaryWindowLog.text = "<color=red>  This doesn't seem to work!</color>";
            return;
        }

        if (destroyOnUse)
        {
            isDestroyed = true;
            controller.areaManager.InitDiscoveredInteractibles();
        }

        GameController.controller.ReturnToMainWindowCommand();
        properties.reactionsOnUse.ExecuteReaction(controller);
    }

    public bool UsedAlone()
    {
        if (staticInteractible) return false;

        for (int i = 0; i < GameData.gameData.allItems.Count; i++)
        {
            if (GameData.gameData.allItems[i].usableWith.Contains(this))
            {
                return false;
            }
        }

        return UseConditionsMet();
    }

    public bool UseConditionsMet()
    {
        if (usedAloneIfConditions.Count == 0) return true;

        bool result = usedAloneIfConditions[0].runValue;
 
        for (int i = 1; i < usedAloneIfConditions.Count; i++)
        {
            result = result && usedAloneIfConditions[i].runValue;
        }

        return result;
    }

    public bool ConditionsMet()
    {
        if (foundIfConditions.Count < 1)
        {
            return true;
        }

        bool result = true;

        for (int i = 0; i < foundIfConditions.Count; i++)
        {
            if (!foundIfConditions[i].runValue)
            {
                return false;
            }
        }

        return result;
    }

    public bool TryFindInteractible(bool isRoomLit)
    {
        float rollChange=0f;
        if (isFound) return true;
        if (!ConditionsMet()) return false;
        if (!isRoomLit && !properties.visibleWhenUnlit && properties.forceRequireLight)
        {
            return false;
        }

        else if (!isRoomLit && !properties.visibleWhenUnlit && !properties.forceRequireLight)
        {
            rollChange = -50f;
        }

        float roll = Random.Range(1f, 100f);

        if (roll <= properties.chanceToFind + rollChange)
        {
            isFound = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public string Inspect()
    {
        string textToReturn = " ";

        textToReturn += properties.descriptionWhenInspected;

        if (reactionOnInspect != null) reactionOnInspect.ExecuteReaction(GameController.controller);
        return textToReturn;
    }

}
