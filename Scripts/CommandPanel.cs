using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Add/Command Panel")]
public class CommandPanel : ScriptableObject,ISerializationCallbackReceiver 
{
    public Area locatedInArea;

    [TextArea]
    public string panelDescription = "";

    [TextArea]
    public string panelDescritpionIfNotActivated = "";

    [TextArea]
    public string startingText = "";

    public bool activated = false;
    [System.NonSerialized]
    public bool isActivated;

    public bool discovered = false;
    [System.NonSerialized]
    public bool isDiscovered;

    [System.NonSerialized]
    public bool isLoaded;

    public bool passwordProtected = false;
    [System.NonSerialized]
    public bool isPasswordProtected;

    public string password = "1234";

    [Space(5)]
    [Header("Logs Already on Terminal")]
    public List<TerminalLog> logs = new List<TerminalLog>();
    [System.NonSerialized]
    public List<TerminalLog> activeLogs = new List<TerminalLog>();

    [Space(5)]
    [Header("Commands")]
    public List<PanelCommandButton> commands= new List<PanelCommandButton>();
    [System.NonSerialized]
    public List<PanelCommandButton> activeCommands = new List<PanelCommandButton>();
    
    public void OnAfterDeserialize()
    {
        isActivated = activated;
        isDiscovered = discovered;
        isPasswordProtected = passwordProtected;

        activeLogs.Clear();
        activeCommands.Clear();

        for (int i = 0; i < logs.Count; i++)
        {
            activeLogs.Add(logs[i]);
        }

        for (int i = 0; i < commands.Count; i++)
        {
            activeCommands.Add(commands[i]);
        }
        isLoaded = false;
    }

    public void OnBeforeSerialize()
    { }

    public void LoadPanelData(PanelsSave data)
    {
        isActivated = data.isactivated;
        isDiscovered = data.isdiscovered;
        isPasswordProtected = data.ispasswordprotected;

        activeLogs.Clear();
        for (int i = 0; i < data.logName.Count; i++)
        {
            for (int j = 0; j < GameData.gameData.allTerminalLogs.Count; j++)
            {
                TerminalLog current = GameData.gameData.allTerminalLogs[j];
                if (data.logName[i] == current.name)
                {
                    current.isLocked = data.logislocked[i];
                    current.isViewed = data.logisviewed[i];
                    activeLogs.Add(current);
                }
            }
        }

        activeCommands.Clear();
        for (int i = 0; i < data.commandName.Count; i++)
        {
            for (int j = 0; j < GameData.gameData.allTerminalCommands.Count; j++)
            {
                PanelCommandButton current = GameData.gameData.allTerminalCommands[j];
                if (data.commandName[i] == current.name)
                {
                    current.isEnabled = data.commandisenabled[i];
                    activeCommands.Add(current);
                }
            }
        }
    }

    public void TurnOn()
    {
        if (!isDiscovered) return;

        if (!isActivated)
        {
            isActivated = true;
            isLoaded = true;
        }
    }

    public string Inspect()
    {
        Debug.Log("Inspecting " + name + ": ");
        if (isActivated)
        {
            return panelDescription;
        }
        else
        {
            return panelDescritpionIfNotActivated;
        }
    }

    public IEnumerator ShowLog(Text textBox, string log)
    {
        foreach (char c in log)
        {
            textBox.text += c;

            yield return new WaitForSeconds(0.02f);
        }

        yield break;
    }

    public void TransferDataToPortable()
    {
        for (int i = 0; i < activeLogs.Count; i++)
        {
            if (!GameData.gameData.savedLogs.Contains(activeLogs[i]))
                GameData.gameData.savedLogs.Add(activeLogs[i]);
        }
    }
}
