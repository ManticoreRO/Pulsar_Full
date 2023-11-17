using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exit: ISerializationCallbackReceiver
{
    public Area destination;
    public int timeToTravel = 0;
    public float chanceToFind = 100;
    public bool discoverableWithLight = false;
    public bool secret = false;

    [TextArea]
    public string textWhenTravellingTo = "";
    [TextArea]
    public string textWhenDiscovering = "";

    public bool discovered = false;
    [System.NonSerialized]
    public bool isDiscovered;
    [System.NonSerialized]
    public bool isLocked;

    public void OnAfterDeserialize()
    {
        isDiscovered = discovered;
        isLocked = false;
    }

    public void OnBeforeSerialize()
    { }

    // Try and travel from here?
    public void Travel()
    {
        if (GameData.gameData.GetComponent<PlayerHealth>().currentEnergy >= timeToTravel)
        {
            GameController.controller.AddTextWithReturn("  " + textWhenTravellingTo, Color.yellow);
            GameController.controller.areaManager.ChangeArea(destination);
            // Add time here
            GameController.controller.AddTime(timeToTravel);
            GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-timeToTravel);
        }
        else
        {
            // We show the player the beg for money screen
            GameController.controller.activeWindow = GameController.controller.noEnergyWindow;
        }
    }


    // Show about
    public void About()
    {
        if (destination.isVisited)
        {
            GameController.controller.secondaryWindowLog.text = destination.whenVisited;
        }
        else
        {
            GameController.controller.secondaryWindowLog.text = "  You do not have knowledge about this place yet!";
        }
    }

    // Try and find it
    public bool TryDiscover()
    {
        float rollchange = 0f;

        if (isDiscovered) return true;
        if (secret) return false;
        if (isLocked) return false;

        if (!GameController.controller.areaManager.currentArea.isLit && !GameController.controller.areaManager.currentArea.isLitByItem)
        {
            if (discoverableWithLight)
                rollchange = -50f;
        }

        float roll = Random.Range(1f, 100f);

        if (roll <= chanceToFind + rollchange)
        {
            isDiscovered = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
