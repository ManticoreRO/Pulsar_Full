using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class AreaSave
{
    public bool isvisited;
    public bool islit;
    public bool isexplored;
    public bool islitbyitem;

    public int searchtime;
    public int lookaroundtime;
    public int exploretime;

    public bool noitemsleft;
    public bool nointeractiblesleft;
    public bool nopanelsleft;
    public bool noexitsleft;

    // exits data
    public List<bool> isdiscovered = new List<bool>();
    public List<bool> islocked = new List<bool>();
    // extra lore
    public List<bool> extraLoreData = new List<bool>();
    // current background
    public int savedBackground;

    public AreaSave()
    {

    }

    public AreaSave(Area area)
    {
        isvisited = area.isVisited;
        islit = area.isLit;
        isexplored = area.isExplored;
        islitbyitem = area.isLitByItem;

        searchtime = area.searchTime;
        lookaroundtime = area.lookAroundTime;
        exploretime = area.exploreTime;

        noitemsleft = area.noItemsLeft;
        nointeractiblesleft = area.noInteractiblesLeft;
        nopanelsleft = area.noPanelsLeft;
        noexitsleft = area.noExitsLeft;

        savedBackground = area.currentBackground;
        // exit data
        for (int i = 0; i < area.availableExits.Count; i++)
        {
            isdiscovered.Add(area.availableExits[i].isDiscovered);
            islocked.Add(area.availableExits[i].isLocked);
        }
    }
}
