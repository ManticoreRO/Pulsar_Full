using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelsSave
{
    public bool isactivated;
    public bool isdiscovered;
    public bool ispasswordprotected;

    public List<string> logName = new List<string>();
    public List<bool> logislocked = new List<bool>();
    public List<bool> logisviewed = new List<bool>();

    public List<string> commandName = new List<string>();
    public List<bool> commandisenabled = new List<bool>();

 
    public PanelsSave()
    { }

    public PanelsSave(CommandPanel panel)
    {
        isactivated = panel.isActivated;
        isdiscovered = panel.isDiscovered;
        ispasswordprotected = panel.isPasswordProtected;

        for (int i = 0; i < panel.activeLogs.Count; i++)
        {
            logName.Add(panel.activeLogs[i].name);
            logislocked.Add(panel.activeLogs[i].isLocked);
            logisviewed.Add(panel.activeLogs[i].isViewed);
        }

        for (int i = 0; i < panel.activeCommands.Count; i++)
        {
            commandName.Add(panel.activeCommands[i].name);
            commandisenabled.Add(panel.activeCommands[i].isEnabled);
        }
    }
}
