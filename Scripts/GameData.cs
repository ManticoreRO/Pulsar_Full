using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;

public class GameData : MonoBehaviour {

    public static GameData gameData;
    public static GameData savedGame;

    [Space(5)]
    [Header("Portable terminal data")]
    public List<TerminalLog> savedLogs = new List<TerminalLog>();

    [Header("AREAS")]
    public List<Area> allAreas = new List<Area>();
    [Space(5)]
    [Header("INTERACTIBLES")]
    public List<Interactible> allInteractibles = new List<Interactible>();
    [Space(5)]
    [Header("ITEMS")]
    public List<Item> allItems = new List<Item>();
    [Space(5)]
    [Header("TERMINALS")]
    public List<CommandPanel> allCommandPanels = new List<CommandPanel>();
    [Space(5)]
    [Header("EVENTS")]
    public List<Event> allEvents = new List<Event>();
    [Space(5)]
    [Header("RECIPES")]
    public List<ItemCombination> allRecipes = new List<ItemCombination>();
    [Header("ENEMIES")]
    public List<Enemy> allEnemies = new List<Enemy>();
    [Header("TERMINAL LOGS")]
    public List<TerminalLog> allTerminalLogs = new List<TerminalLog>();
    [Header("TERMINAL COMMANDS")]
    public List<PanelCommandButton> allTerminalCommands = new List<PanelCommandButton>();
    [Header("BOOLEAN SWITCHES")]
    public List<BooleanSwitch> allBooleanSwitches = new List<BooleanSwitch>();

    [Header("SAVE DATA")]
    public List<AreaSave> savedAreas = new List<AreaSave>();
    public List<ItemSave> savedItems = new List<ItemSave>();
    public List<InteractibleSave> savedInteractibles = new List<InteractibleSave>();
    public List<PanelsSave> savedPanels = new List<PanelsSave>();
    public List<EventSave> savedEvents = new List<EventSave>();
    public List<EnemySave> savedEnemies = new List<EnemySave>();
    public PlayerSave savedPlayer;
    public string savedCurrentArea;

    public bool justLoaded;

    string SavePath;
    string fileName;

    public TextAsset textsFile;

    private void Awake()
    {
        // make this a singleton
        if (gameData == null)
        {
            gameData = this;
            DontDestroyOnLoad(this);
            savedLogs.Clear();
#if (UNITY_EDITOR)
            SavePath = Path.Combine(Application.dataPath, "SaveGame");
#elif (UNITY_ANDROID)
            SavePath = Path.Combine(Application.persistentDataPath, "SaveGame");
#else
            SavePath = Path.Combine(Application.dataPath, "SaveGame");
#endif
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
        }
        else if (gameData != this)
        {
            Destroy(gameData);
        }
    }

    private void Start()
    {
        //SaveAllStrings();
    }

    public void SaveAreas()
    {
        savedAreas.Clear();

        for (int i = 0; i < allAreas.Count; i++)
        {
            AreaSave saved = new AreaSave(allAreas[i]);
            savedAreas.Add(saved);
        }

        // save
        fileName = Path.Combine(SavePath, "Areas.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<AreaSave>));
        FileStream stream = new FileStream(fileName, FileMode.Create);
        xmlS.Serialize(stream, savedAreas);
        stream.Close();
    }

    public void LoadAreas()
    {
        savedAreas.Clear();

        // load
        fileName = Path.Combine(SavePath, "Areas.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<AreaSave>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedAreas = (List<AreaSave>)xmlS.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < savedAreas.Count; i++)
        {
            allAreas[i].LoadAreaData(savedAreas[i]);
        }
    }

    public void SaveItems()
    {
        savedItems.Clear();

        for (int i = 0; i < allItems.Count; i++)
        {
            ItemSave saved = new ItemSave(allItems[i]);
            savedItems.Add(saved);
        }

        // save
        fileName = Path.Combine(SavePath, "Items.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<ItemSave>));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, savedItems);
        stream.Close();
    }

    public void LoadItems()
    {
        savedItems.Clear();

        // load
        fileName = Path.Combine(SavePath, "Items.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<ItemSave>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedItems = (List<ItemSave>)xmlS.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < savedItems.Count; i++)
        {
            allItems[i].LoadItemData(savedItems[i]);
            // add it to the inventory if it belongs there
            if (allItems[i].isInInventory) GameController.controller.areaManager.inventory.Add(allItems[i]);
        }
    }

    public void SaveInteractibles()
    {
        savedInteractibles.Clear();

        for (int i = 0; i < allInteractibles.Count; i++)
        {
            InteractibleSave saved = new InteractibleSave(allInteractibles[i]);
            savedInteractibles.Add(saved);
        }

        // save
        fileName = Path.Combine(SavePath, "Interactibles.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<InteractibleSave>));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, savedInteractibles);
        stream.Close();
    }

    public void LoadInteractibles()
    {
        savedInteractibles.Clear();

        // load
        fileName = Path.Combine(SavePath, "Interactibles.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<InteractibleSave>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedInteractibles = (List<InteractibleSave>)xmlS.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < savedInteractibles.Count; i++)
        {
            allInteractibles[i].LoadInteractibleData(savedInteractibles[i]);
        }
    }

    public void SavePanels()
    {
        savedPanels.Clear();

        for (int i = 0; i < allCommandPanels.Count; i++)
        {
            PanelsSave saved = new PanelsSave(allCommandPanels[i]);
            savedPanels.Add(saved);
        }

        // save
        fileName = Path.Combine(SavePath, "Panels.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<PanelsSave>));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, savedPanels);
        stream.Close();
    }

    public void LoadPanels()
    {
        savedPanels.Clear();

        // load
        fileName = Path.Combine(SavePath, "Panels.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<PanelsSave>));
        FileStream stream = new FileStream(fileName, FileMode.Open);
        savedPanels = (List<PanelsSave>)xmlS.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < savedPanels.Count; i++)
        {
            allCommandPanels[i].LoadPanelData(savedPanels[i]);
        }
    }

    public void SaveBooleanSwitches()
    {
        List<bool> runValues = new List<bool>();

        for (int i = 0; i < allBooleanSwitches.Count; i++)
        {
            runValues.Add(allBooleanSwitches[i].runValue);
            runValues.Add(allBooleanSwitches[i].executedReaction);
        }

        // save
        fileName = Path.Combine(SavePath, "BooleanSwitches.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<bool>));
        FileStream stream = new FileStream(fileName, FileMode.Create);
        xmlS.Serialize(stream, runValues);
        stream.Close();
    }

    public void LoadBooleanSwitches()
    {
        List<bool> savedValues = new List<bool>();
        // load
        fileName = Path.Combine(SavePath, "BooleanSwitches.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<bool>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedValues = (List<bool>)xmlS.Deserialize(stream);
        stream.Close();

        int k = 0;
        for (int i = 0; i < savedValues.Count-1; i+=2)
        {
            allBooleanSwitches[k].runValue = savedValues[i];
            allBooleanSwitches[k].executedReaction = savedValues[i+1];
            k += 1;
        }
    }

    public void SaveEvents()
    {
        savedEvents.Clear();

        for (int i = 0; i < allEvents.Count; i++)
        {
            EventSave saved = new EventSave(allEvents[i]);
            savedEvents.Add(saved);
        }

        // save
        fileName = Path.Combine(SavePath, "Events.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<EventSave>));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, savedEvents);
        stream.Close();
    }

    public void LoadEvents()
    {
        savedEvents.Clear();

        // load
        fileName = Path.Combine(SavePath, "Events.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<EventSave>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedEvents = (List<EventSave>)xmlS.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < savedEvents.Count; i++)
        {
            allEvents[i].LoadEventData(savedEvents[i]);
        }
    }

    public void SaveEnemies()
    {
        savedEnemies.Clear();

        for (int i = 0; i < allEnemies.Count; i++)
        {
            EnemySave save = new EnemySave(allEnemies[i]);
            savedEnemies.Add(save);
        }

        // save
        fileName = Path.Combine(SavePath, "Enemies.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<EnemySave>));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, savedEnemies);
        stream.Close();
    }
    
    public void LoadEnemies()
    {
        savedEnemies.Clear();
        // load
        fileName = Path.Combine(SavePath, "Enemies.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<EnemySave>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedEnemies = (List<EnemySave>)xmlS.Deserialize(stream);
        stream.Close();

        // update enemies
        for (int i = 0; i < savedEnemies.Count; i++)
        {
            allEnemies[i].LoadEnemyData(savedEnemies[i]);
        }
    }

    public void SavePortableTerminal()
    {
        List<string> logsInTerminal = new List<string>();

        for (int i = 0; i < savedLogs.Count; i++)
        {
            logsInTerminal.Add(savedLogs[i].name);
        }

        // save
        fileName = Path.Combine(SavePath, "Portable.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<string>));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, logsInTerminal);
        stream.Close();
    }

    public void LoadPortableTerminal()
    {
        List<string> logsInTerminal = new List<string>();

        // load
        fileName = Path.Combine(SavePath, "Portable.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(List<string>));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        logsInTerminal = (List<string>)xmlS.Deserialize(stream);
        stream.Close();

        savedLogs.Clear();
        // update the portable terminal
        if (logsInTerminal.Count > 0)
        {
            for (int i = 0; i < logsInTerminal.Count; i++)
            {
                for (int j = 0; j < allTerminalLogs.Count; j++)
                {
                    if (allTerminalLogs[j].name == logsInTerminal[i])
                    {
                        // add it
                        savedLogs.Add(allTerminalLogs[j]);
                    }
                }
            }
        }

    }

    public void SavePlayer()
    {
        savedPlayer = new PlayerSave(GetComponent<PlayerHealth>());
        // save
        fileName = Path.Combine(SavePath, "Player.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(PlayerSave));
        FileStream stream = new FileStream( fileName, FileMode.Create);
        xmlS.Serialize(stream, savedPlayer);
        stream.Close();
    }

    public void LoadPlayer()
    {
        // load
        fileName = Path.Combine(SavePath, "Player.xml");
        XmlSerializer xmlS = new XmlSerializer(typeof(PlayerSave));
        FileStream stream = new FileStream( fileName, FileMode.Open);
        savedPlayer = (PlayerSave)xmlS.Deserialize(stream);
        stream.Close();

        // set it up
        GetComponent<PlayerHealth>().LoadPlayerData(savedPlayer);
        GameController.controller.portablePickedUp = savedPlayer.isportablePickedUp;
        GameController.controller.relayFixed = savedPlayer.relayfixed;
        // time
        GameController.controller.currentDay = savedPlayer.savedCycles;
        GameController.controller.currentHour = savedPlayer.savedTicks;

        // setup background
        GameController.controller.background.rectTransform.Rotate(new Vector3(0, 0, savedPlayer.backgroundCurrentRotation));

        // setup sound
        GameController.controller.soundVolume = savedPlayer.soundVol;
        GameController.controller.musicVolume = savedPlayer.musicVol;

        // set up current area
        for (int i = 0; i < allAreas.Count; i++)
        {
            if (allAreas[i].name == savedPlayer.currentArea)
            {
                GameController.controller.areaManager.currentArea = allAreas[i];
                GameController.controller.areaManager.previousArea = null;
                return;
            }
            /*if (allAreas[i].name == savedPlayer.previousArea)
            {
                GameController.controller.areaManager.previousArea = allAreas[i];
            }*/
        }

        //if (savedPlayer.previousArea == "") 
        GameController.controller.areaManager.previousArea = null;

        // here, we will load from the buffer whatever we need
    }

    public void SaveGame()
    {
        PlayerPrefs.Save();

        if (!GameController.controller.gameStarted) return;

        SaveAreas();
        SaveItems();
        SaveInteractibles();
        SavePanels();
        SaveBooleanSwitches();
        SaveEvents();
        SaveEnemies();
        SavePortableTerminal();
        SavePlayer();
    }

    public void LoadGame()
    {
        LoadAreas();
        LoadItems();
        LoadInteractibles();
        LoadPanels();
        LoadBooleanSwitches();
        LoadEvents();
        LoadEnemies();
        LoadPortableTerminal();
        LoadPlayer();
        AddOfflineEnergy();
    }

    public void AddOfflineEnergy()
    {
        // check hour
        // we save the time when the player quit so we can calculate the number of minutes
        string unixTime = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMinutes.ToString();

        // get our saved one that in player prefs
        if (!PlayerPrefs.HasKey("Key")) return; // first run

        string savedTime = PlayerPrefs.GetString("Key");

        double uT = double.Parse(unixTime);
        double sT = double.Parse(savedTime);

        double minuteDifference = uT - sT;
        int numEnergy = (int)minuteDifference / 5;


        GetComponent<PlayerHealth>().ModifyEnergy(numEnergy);
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGame();

            // we save the time when the player left so we can calculate the number of minutes
            string unixTime = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMinutes.ToString();

            // write that in player prefs
            PlayerPrefs.SetString("Key", unixTime);
        }
        else
        {
            AddOfflineEnergy();
        }
    }

    public void OnApplicationQuit()
    {
        // we save the time when the player quit so we can calculate the number of minutes
        string unixTime = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMinutes.ToString();

        // write that in player prefs
        PlayerPrefs.SetString("Key", unixTime);
    }

    public void ContinueCommand()
    {
        fileName = Path.Combine(SavePath, "Areas.xml");
        if (System.IO.File.Exists(fileName))
        {
            // we do the loading
            LoadGame();
            justLoaded = true;
            GameController.controller.SetupLoadGame();
        }
        else
        {
            // we just start a new game
            GameController.controller.NewGame();
        }
    }

    public void ResetGame()
    {
        for (int i = 0; i < allAreas.Count; i++)
        {
            allAreas[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allItems.Count; i++)
        {
            allItems[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allInteractibles.Count; i++)
        {
            allInteractibles[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allEvents.Count; i++)
        {
            allEvents[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allEnemies.Count; i++)
        {
            allEnemies[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allBooleanSwitches.Count; i++)
        {
            allBooleanSwitches[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allCommandPanels.Count; i++)
        {
            allCommandPanels[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allTerminalCommands.Count; i++)
        {
            allTerminalCommands[i].OnAfterDeserialize();
        }

        for (int i = 0; i < allTerminalLogs.Count; i++)
        {
            allTerminalLogs[i].OnAfterDeserialize();
        }

        savedLogs.Clear();
        // now the player
        GetComponent<PlayerHealth>().PlayerReset();
        GameController.controller.currentDay = 1;
        GameController.controller.currentHour = 1;
    }

    public void SaveAllStrings()
    {
        // we will save all scriptable object strings to a file so we can see them all there

        // areas first
        string fileString = "Pulsar - version " + Application.version.ToString() + "\n";
        fileString += "\n\nAreas:";
        for (int i = 0; i < allAreas.Count; i++)
        {
            Area a = allAreas[i];
            fileString += "\n\nArea name:" + a.name;
            fileString += "\nFirst visit description:\n" + a.whenNotVisited;
            fileString += "\nVisit description:\n" + a.whenVisited;
            fileString += "\nWhen lit description:\n" + a.whenLit;
        }

        fileString += "\n\n\nEvents";

        // now for the events
        for (int i = 0; i < allEvents.Count; i++)
        {
            Event e = allEvents[i];
            fileString += "\n\nEvent name:" + e.eventName;
            fileString += "\nEvent description:" + e.eventDescription;
        }

        // now we write to the file
        string path = "Assets/Locale/areas.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(fileString);
        writer.Close();

        //AssetDatabase.ImportAsset(path);
    }

    public void LoadAllStringsFromFile()
    {
        // we load all scriptable object strings from a file so we can ease translations and corrections and
        // writing
    }
}
