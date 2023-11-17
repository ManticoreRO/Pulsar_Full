using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Area")]
public class Area : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Area properties")]
    public bool visited = false;
    public bool lit = false;
    public bool explored = false;
    public bool inside = false;
    public float pulsingIntensity = 0.5f;

    [System.NonSerialized]
    public bool isVisited;
    [System.NonSerialized]
    public bool isLit;
    [System.NonSerialized]
    public bool isExplored;
    [System.NonSerialized]
    public bool isLitByItem;

    [Space(2)]
    [Header("Area properties descriptions")]
    [TextArea]
    public string whenNotVisited = "";
    [TextArea]
    public string whenVisited = "";
    [TextArea]
    public string whenLit = "";
    [TextArea]
    public string whenNotLit = "";

    [Space(2)]
    [Header("Available exits")]
    public List<Exit> availableExits = new List<Exit>();

    [Space(2)]
    [Header("On enter area event")]
    public Reaction onEnterArea;

    [Space(2)]
    [Header("Interface and music")]
    public AudioClip musicFile;
    [System.NonSerialized]
    public int currentBackground;
    public List<Sprite> backgroundImages = new List<Sprite>();

    [Space(2)]
    [Header("No save area?")]
    public bool noSaveArea = false;

    [HideInInspector]
    // Exponential searching and looking around time consumption
    [System.NonSerialized]
    public int searchTime;
    [System.NonSerialized]
    public int lookAroundTime;
    [System.NonSerialized]
    public int exploreTime;
    [System.NonSerialized]
    public bool noItemsLeft;
    [System.NonSerialized]
    public bool noInteractiblesLeft;
    [System.NonSerialized]
    public bool noPanelsLeft;
    [System.NonSerialized]
    public bool noExitsLeft;

    public void OnAfterDeserialize()
    {
        isVisited = visited;
        isLit = lit;
        isExplored = explored;
        isLitByItem = false;
        searchTime = 1;
        lookAroundTime = 1;
        exploreTime = 0;
        noInteractiblesLeft = false;
        noItemsLeft = false;
        noPanelsLeft = false;
        noExitsLeft = false;
        currentBackground = 0;
    }

    public void OnBeforeSerialize()
    { }

    public void LoadAreaData(AreaSave data)
    {
        isVisited = data.isvisited;
        isLit = data.islit;
        isExplored = data.isexplored;
        isLitByItem = data.islitbyitem;
        searchTime = data.searchtime;
        lookAroundTime = data.lookaroundtime;
        exploreTime = data.exploretime;
        noInteractiblesLeft = data.nointeractiblesleft;
        noItemsLeft = data.noitemsleft;
        noPanelsLeft = data.nopanelsleft;
        noExitsLeft = data.noexitsleft;
        currentBackground = data.savedBackground;

        for (int i = 0; i < availableExits.Count; i++)
        {
            availableExits[i].isDiscovered = data.isdiscovered[i];
            availableExits[i].isLocked = data.islocked[i];
        }
    }

    public string GetAreaDescription()
    {
        string textToShow = "";

        if (isVisited)
        {
            if (whenVisited != "") textToShow += whenVisited + " ";
        }
        else
        {
            if (whenNotVisited != "") textToShow += whenNotVisited + " ";
        }

        if (isLit)
        {
            if (whenLit != "") textToShow += whenLit + " ";
        }
        else
        {
            if (whenNotLit != "") textToShow += whenNotLit + " ";
        }
        return textToShow;
    }
}
