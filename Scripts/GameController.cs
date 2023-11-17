using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements; // only compile Ads code on supported platforms

public class GameController : MonoBehaviour
{
    // make this a singleton
    public static GameController controller;

    [Header("Log")]
    public string completeLog = "";
    public Text logTextPrefab;
    public Transform logTextParent;

    [Space(5)]
    [Header("Area name text")]
    public Text areaNameText;

    [Space(5)]
    [Header("List items prefab")]
    public GameObject listItemPrefab;
    public Transform listItemParent;
    public Text secondaryWindowLog;

    [HideInInspector]
    public AreaManagement areaManager;

    [Space(5)]
    [Header("Time")]
    public int currentHour = 1;
    public int currentDay = 1;
    public Text timeText;

    [Space(5)]
    [Header("Background")]
    public Image background;
    public GameObject imagePanel;
    public AudioClip mainMenuMusic;

    [Space(5)]
    [Header("Energy&Shield Management")]
    public Text energyText;
    public Text shieldText;
    public GameObject energyBar;
    public GameObject shieldBar;

    [Space(5)]
    [Header("Window Management")]
    // Window management
    GameObject previousWindow;
    public GameObject currentWindow;
    public GameObject activeWindow;

    public GameObject menuWindow;
    public GameObject mainWindow;
    public GameObject secondaryWindow;
    public GameObject panelWindow;
    public GameObject eventWindow;
    public GameObject deathWindow;
    public GameObject noEnergyWindow;
    public GameObject thankYouWindow;
    public GameObject battleWindow;
    public GameObject portableTerminalWindow;
    public GameObject optionsScreen;
    public GameObject creditsWindow;
    public GameObject noAdWindow;

    [Space(5)]
    [Header("Portable Terminal")]
    public Transform portableCommandsParent;
    public Button portableCommandButton;
    public Text portableTextLog;

    [Space(5)]
    [Header("Used with Battle Window")]
    public Button battleActionButtonPrefab;
    public Transform battleActionButtonParent;
    public Text battleWindowTitle;
    public Text battleWindowLog;
    public GameObject battleEndScreen;
    public GameObject battleBackground;
    public Text battleWinTitleText;
    public Text battleDetailsText;
    public Button battleContinueButton;

    [Space(5)]
    [Header("Used with secondary windows")]
    public Button actionButtonPrefab;
    public Transform actionButtonParent;
    public Text windowTitle;

    [Space(5)]
    [Header("Command Panels")]
    public Button panelButtonPrefab;
    public Transform panelButtonParent;
    public Text panelMessageText;
    public GameObject passwordWindow;
    public GameObject commandWindow;
    public GameObject textWindow;

    public Text passwordText;
    public Button acceptPasswordButton;

    [Space(5)]
    [Header("Events")]
    public Text eventTitleText;
    public Text eventMessageText;
    public Transform eventOptionsParent;

    [Space(5)]
    [Header("Button blinking and effects")]
    public Button travelButton;
    public Button inspectButton;
    public Button pickupButton;
    public Button inventoryButton;
    public Button combineButton;
    public AudioSource clickSoundMenuButtons;
    public GameObject pulsarSFX;

    [HideInInspector]
    public bool[] blinking = new bool[5];

    [Space(5)]
    [Header("Options screen")]
    public Slider soundFX;
    public Slider musicFX;
    public float soundVolume;
    public float musicVolume;

    // Version text
    public Text versionText;

    // IsAreaLit and return to main menu
    public Image isAreaLit;

    // use on variable
    [HideInInspector]
    public Item selectedItemToUseOn = null;
    [HideInInspector]
    public CommandPanel selectedPanelToUse = null;
    [HideInInspector]
    public CommandPanel currentlyShownPanel = null;

    // event bug fixer?
    [HideInInspector]
    public bool areaChanged = false;

    // portable terminal helper
    [HideInInspector]
    public bool portablePickedUp = false;
    [HideInInspector]
    public bool relayFixed = false;
    [HideInInspector]
    public bool duringBattle = false;

    // password bug fixer
    string inputPassword = "";

    // Combine items list
    [HideInInspector]
    public List<Item> selectedItemsToCombine = new List<Item>();
    Color savedButtonColor;

    [HideInInspector]
    public bool eventRunning = false;
    [HideInInspector]
    public Event theEventRunning = null;

    [HideInInspector]
    public bool gameStarted = false;  // for saving problem

    [Space(5)]
    [Header("Ad buttons")]
    public Button watchEnergyAdButton;
    public Button watchDeathAdButton;

    #region AWAKE AND START
    private void Awake()
    {
        if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(this);
        }
        else if (controller != this)
        {
            Destroy(controller);
        }
    }

    public void Start()
    {
        areaManager = GetComponent<AreaManagement>();
        gameStarted = false;
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundVolume = PlayerPrefs.GetFloat("Sound");
        }
        else
        {
            PlayerPrefs.SetFloat("Sound", 0.5f);
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            musicVolume = PlayerPrefs.GetFloat("Music");
        }
        else
        {
            PlayerPrefs.SetFloat("Music", 0.5f);
        }

        currentWindow = null;
        previousWindow = null;

        GetComponent<AudioSource>().clip = mainMenuMusic;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();

        pulsarSFX.GetComponent<AudioSource>().volume = soundVolume;

        // CHANGE HERE TO START THE GAME ON OLD UI
        activeWindow = menuWindow;

        versionText.text = "     " + Application.version;

#if UNITY_ADS
        Advertisement.Initialize(Advertisement.gameId, false);
#endif
    }

    public void StartReturnToMainMenu()
    {
        areaManager = GetComponent<AreaManagement>();
        gameStarted = false;
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundVolume = PlayerPrefs.GetFloat("Sound");
        }
        else
        {
            PlayerPrefs.SetFloat("Sound", 0.5f);
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            musicVolume = PlayerPrefs.GetFloat("Music");
        }
        else
        {
            PlayerPrefs.SetFloat("Music", 0.5f);
        }

        GetComponent<AudioSource>().clip = mainMenuMusic;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();

        pulsarSFX.GetComponent<AudioSource>().volume = soundVolume;
        activeWindow = menuWindow;

        versionText.text = "     " + Application.version;
    }

    public void ButtonReturnToMainWindow()
    {
        // close all windows?
        ReturnToMainWindowCommand();

        // SAVE FIRST
        GameData.gameData.SaveGame();

        imagePanel.GetComponent<Image>().color = Color.clear;

        StartReturnToMainMenu();
    }

    #endregion

    #region GAME START/LOAD
    public void NewGame()
    {
        // HERE WE ALSO NEED TO RESET EVERYTHING
        // reset gamecontroller internal variables
        eventRunning = false;
        theEventRunning = null;
        inputPassword = "";
        selectedItemsToCombine.Clear();
        selectedItemToUseOn = null;
        selectedPanelToUse = null;
        duringBattle = false;
        currentlyShownPanel = null;
        currentDay = 1;
        currentHour = 1;
        completeLog = "";


        // clear all text in the text game object
        for (int i = 0; i < logTextParent.childCount; i++)
        {
            Destroy(logTextParent.GetChild(i).gameObject);
        }

        // clear all discovered interactibles and items
        AreaManagement am = areaManager;
        am.discoveredExits.Clear();
        am.discoveredInteractibles.Clear();
        am.discoveredInteractibles.Clear();
        am.discoveredPanels.Clear();
        am.inventory.Clear();
        am.discoveredSecretArea = false;
        am.foundSomething = false;
        eventRunning = false;

        // here we need to save a newgame state... 
        GameData.gameData.ResetGame();

        areaManager.ChangeArea(areaManager.mainArea);
        AddTime(0);
        oldButtonColor = inventoryButton.GetComponent<Image>().color;

        gameStarted = true;
    }

    public void SetupLoadGame()
    {
        ReturnToMainWindowCommand();
        areaManager.ChangeArea(areaManager.currentArea);
        AddTime(0);
        oldButtonColor = inventoryButton.GetComponent<Image>().color;
        gameStarted = true;
    }
    #endregion

    #region TEXT LOG
    string preparedTextToShow = "";
    string showedText = "";
    Color textColor;

    public void AddTextWithReturn(string theText, Color color)
    {
        // dont show empty text
        if (theText == "") return;

        preparedTextToShow += theText + "\n";
        textColor = color;

        showedText = preparedTextToShow;
        Text newText = Instantiate(logTextPrefab);
        newText.transform.SetParent(logTextParent, false);
        newText.color = textColor;
        newText.text = showedText;
        preparedTextToShow = "";

        // make the others fade away
        float gradualFade = 30;

        for (int i = logTextParent.childCount - 3; i >= 0; i--)
        {
            Text current = logTextParent.GetChild(i).GetComponent<Text>();

            current.color = new Color(current.color.r, current.color.g, current.color.b, gradualFade / 100);
            gradualFade -= 15;
            if (gradualFade <= 0) gradualFade = 0;
        }

        // we now remove the children with 0 transparency and add their text to the saved log
        for (int i = 0; i < logTextParent.childCount; i++)
        {
            if (logTextParent.GetChild(i).GetComponent<Text>().color.a == 0f)
            {

                completeLog += logTextParent.GetChild(i).GetComponent<Text>().text + "\n";
                Destroy(logTextParent.GetChild(i).gameObject);
            }
        }
        // scrolldown to bottom
    }

    #endregion

    #region TIME

    public void AddTime(int numHours)
    {
        currentHour += numHours;

        if (currentHour > 24)
        {
            currentHour = currentHour - 24;
            currentDay += 1;
        }

        timeText.text = "Tick " + currentHour.ToString() + "  Cycle " + currentDay.ToString();
    }

    #endregion

    #region BUTTON SOUNDS
    public void ButtonClickSound(Button b)
    {
        clickSoundMenuButtons.volume = soundVolume;
        clickSoundMenuButtons.Play();
    }
    #endregion

    #region COMMANDS

    public void ReturnToMainWindowCommand()
    {
        ButtonClickSound(null);
        activeWindow = mainWindow;
        secondaryWindowLog.text = "";

        if (areaManager.currentArea == null) return;

        if (areaManager.currentArea.musicFile != null && controller.GetComponent<AudioSource>().clip != areaManager.currentArea.musicFile)
        {
            controller.GetComponent<AudioSource>().clip = areaManager.currentArea.musicFile;
            controller.GetComponent<AudioSource>().Play();
        }
    }

    public void ExploreCommand()
    {
        //ButtonClickSound(travelButton);
        activeWindow = secondaryWindow;
        blinking[0] = false;

        //areaManager.Explore();
        areaManager.discoveredSecretArea = false;

        areaManager.CheckIfFullyExplored();

        // Clear the list
        ClearListItems();
        ClearActionButtons();

        // now we build the screen
        for (int i = 0; i < areaManager.discoveredExits.Count; i++)
        {
            CreateTravelButton(areaManager.discoveredExits[i]);
        }
        // we init the log area
        if (!areaManager.currentArea.noExitsLeft)
        {
            secondaryWindowLog.text = "  You can travel from here or search for exits!";
        }
        else
        {
            secondaryWindowLog.text = "  You can travel from here but there are no other exits except the one found already...";
        }
        // Then we draw the one we need
        Button newButton = Instantiate(actionButtonPrefab);
        newButton.transform.SetParent(actionButtonParent, false);
        newButton.GetComponentInChildren<Text>().text = "Explore more (" + areaManager.currentArea.exploreTime.ToString() + (areaManager.currentArea.exploreTime == 1 ? " tick)" : " ticks)");
        newButton.onClick.AddListener(delegate { ButtonClickSound(newButton); areaManager.Explore(); newButton.GetComponentInChildren<Text>().text = "Explore more (" + areaManager.currentArea.exploreTime.ToString() + (areaManager.currentArea.exploreTime == 1 ? " tick)" : " ticks)"); });
        windowTitle.text = "TRAVEL";
    }

    public void InspectCommand()
    {
        //ButtonClickSound(inspectButton);
        activeWindow = secondaryWindow;

        //areaManager.LookAround();

        ClearActionButtons();
        DrawAllInteractibleButtons();
        areaManager.CheckIfFullyExplored();
        // we init the log area

        if (!areaManager.currentArea.noInteractiblesLeft || !areaManager.currentArea.noPanelsLeft)
        {
            secondaryWindowLog.text = "  There may be objects here!";
        }
        else
        {
            secondaryWindowLog.text = "  I think there is nothing left to find here...";
        }

        // We instantiate the buttons

        // First we clear any button there
        ClearActionButtons();
        // Then we draw the one we need
        Button newButton = Instantiate(actionButtonPrefab);
        newButton.transform.SetParent(actionButtonParent, false);
        newButton.GetComponentInChildren<Text>().text = "Look around more (" + areaManager.currentArea.lookAroundTime.ToString() + (areaManager.currentArea.lookAroundTime == 1 ? " tick)" : " ticks)");
        newButton.onClick.AddListener(delegate { /*ButtonClickSound(newButton);*/ areaManager.LookAround(); newButton.GetComponentInChildren<Text>().text = "Look around more (" + areaManager.currentArea.lookAroundTime.ToString() + (areaManager.currentArea.lookAroundTime == 1 ? " tick)" : " ticks)"); });
        windowTitle.text = "INSPECT OBJECTS";
    }

    public void UseOnCommand()
    {
        activeWindow = secondaryWindow;

        DrawAllUseOnButtons();
        // we init the log area
        if (secondaryWindowLog.text == "") secondaryWindowLog.text = "  Perhaps you can use " + selectedItemToUseOn.name + " here!";
        // We instantiate the buttons

        // First we clear any button there
        ClearActionButtons();

        windowTitle.text = "USE " + selectedItemToUseOn.name.ToUpper();
    }

    public void PickUpCommand()
    {
        //ButtonClickSound(pickupButton);
        activeWindow = secondaryWindow;

        //areaManager.Search();

        DrawAllGroundItemsButtons();
        areaManager.CheckIfFullyExplored();
        // we init the log area

        if (!areaManager.currentArea.noItemsLeft)
        {
            secondaryWindowLog.text = "  There may be items here!";
        }
        else
        {
            secondaryWindowLog.text = "  All that was to be found here was found. There is nothing else...";
        }

        // We instantiate the buttons

        // First we clear any button there
        ClearActionButtons();
        // Then we draw the one we need
        Button newButton = Instantiate(actionButtonPrefab);
        newButton.transform.SetParent(actionButtonParent, false);
        newButton.GetComponentInChildren<Text>().text = "Search more (" + areaManager.currentArea.searchTime.ToString() + (areaManager.currentArea.searchTime == 1 ? " tick)" : " ticks)");
        newButton.onClick.AddListener(delegate { ButtonClickSound(newButton); areaManager.Search(); newButton.GetComponentInChildren<Text>().text = "Search more (" + areaManager.currentArea.searchTime.ToString() + (areaManager.currentArea.searchTime == 1 ? " tick)" : " ticks)"); });
        windowTitle.text = "ITEMS SCATTERED ABOUT";
    }

    public void InventoryCommand()
    {
        //ButtonClickSound(inventoryButton);
        // for debugging only, i add this
        for (int i = 0; i < GameData.gameData.allItems.Count; i++)
        {
            Item itm = GameData.gameData.allItems[i];

            if (itm.isInInventory && !areaManager.inventory.Contains(itm))
            {
                // add it
                areaManager.inventory.Add(itm);
            }
        }

        if (areaManager.inventory.Count == 0)
        {
            // we dont draw if nothing in inventory
            AddTextWithReturn("  The inventory is empty!", Color.red);
            return;
        }

        activeWindow = secondaryWindow;

        DrawAllInventoryButtons();
        // we init the log area
        if (secondaryWindowLog.text == "") secondaryWindowLog.text = "  Some of these items can be used!";
        // We instantiate the buttons

        // First we clear any button there
        ClearActionButtons();

        windowTitle.text = "INVENTORY";
    }

    public void ShowPanelCommand(CommandPanel panel)
    {
        if (panel.isActivated)
        {
            activeWindow = panelWindow;
            currentlyShownPanel = panel;

            if (panel.isPasswordProtected)
            {
                passwordWindow.SetActive(true);
                commandWindow.SetActive(false);
                textWindow.SetActive(false);
                passwordText.text = "";
                inputPassword = "";

                acceptPasswordButton.onClick.AddListener(delegate { VerifyPassword(panel); });
            }
            else
            {
                passwordWindow.SetActive(false);
                commandWindow.SetActive(true);
                textWindow.SetActive(true);


                panelMessageText.text = "";
                StopAllCoroutines();

                StartCoroutine(panel.ShowLog(panelMessageText, panel.startingText));

                // if the initial text was shown, we continue

                // We create all buttons
                // We clear all buttons first
                for (int i = 0; i < panelButtonParent.childCount; i++)
                {
                    Destroy(panelButtonParent.GetChild(i).gameObject);
                }
                // then we draw

                for (int i = 0; i < panel.activeLogs.Count; i++)
                {
                    TerminalLog current = panel.activeLogs[i];

                    if (!current.isLocked)
                    {
                        // instantiate Panel buttons
                        Button panelButton = Instantiate(panelButtonPrefab);
                        panelButton.transform.SetParent(panelButtonParent, false);

                        string btnName = (current.isViewed ? "(Viewed)" : "(NEW)");
                        panelButton.GetComponentInChildren<Text>().text = current.logLabel + " " + btnName;
                        panelButton.onClick.AddListener(delegate { StopAllCoroutines(); panelMessageText.text = ""; StartCoroutine(panel.ShowLog(panelMessageText, current.LogString())); current.isViewed = true; btnName = (current.isViewed ? "(Viewed)" : "NEW"); panelButton.GetComponentInChildren<Text>().text = current.logLabel + " " + btnName; if (current.reactionUponReading != null) current.reactionUponReading.ExecuteReaction(this); });
                    }
                }


                // we create the commands buttons
                for (int i = 0; i < panel.activeCommands.Count; i++)
                {
                    PanelCommandButton current = panel.activeCommands[i];
                    if (current.isEnabled)
                    {// instantiate Panel buttons
                        Button panelButton = Instantiate(panelButtonPrefab);
                        panelButton.transform.SetParent(panelButtonParent, false);

                        string btnName = "CMD_" + current.name;
                        panelButton.GetComponentInChildren<Text>().text = btnName;
                        panelButton.onClick.AddListener(delegate { StopAllCoroutines(); panelMessageText.text = ""; StartCoroutine(panel.ShowLog(panelMessageText, current.Messages())); current.ExecuteIfTrue(); if (!current.isEnabled) Destroy(panelButton.gameObject); });
                    }
                }

                // we create the transfer button
                // only if our inventory contains the portable item
                if (portablePickedUp)
                {
                    Button transferButton = Instantiate(panelButtonPrefab);
                    transferButton.transform.SetParent(panelButtonParent, false);
                    transferButton.GetComponentInChildren<Text>().text = "Transfer Data";
                    transferButton.onClick.AddListener(delegate { StopAllCoroutines(); panelMessageText.text = ""; panel.ShowLog(panelMessageText, "Uploading..."); panel.TransferDataToPortable(); panelMessageText.text += "SUCCESS!"; });
                }

                // we create the close button
                Button closeButton = Instantiate(panelButtonPrefab);
                closeButton.transform.SetParent(panelButtonParent, false);
                closeButton.GetComponentInChildren<Text>().text = "Close";
                closeButton.onClick.AddListener(delegate { StopAllCoroutines(); currentlyShownPanel = null; activeWindow = mainWindow; });

            }
        }
        else
        {
            secondaryWindowLog.text = "  <color=red>It will not turn on.</color>";
        }
    }

    public void CombineCommand()
    {
        //ButtonClickSound(combineButton);
        if (areaManager.inventory.Count == 0)
        {
            // we dont draw if nothing in inventory
            AddTextWithReturn("  The inventory is empty!", Color.red);
            return;
        }

        activeWindow = secondaryWindow;

        selectedItemsToCombine.Clear();

        DrawAllCombineButtons();
        // we init the log area
        if (secondaryWindowLog.text == "") secondaryWindowLog.text = "  Perhaps some of these items can be combined!";
        // We instantiate the buttons

        // First we clear any button there
        ClearActionButtons();

        // And add the combine button
        Button newButton = Instantiate(actionButtonPrefab);


        UnityEngine.Events.UnityAction combineCall = delegate
        {
            bool combineResult = false;
            ButtonClickSound(newButton);
            for (int i = 0; i < GameData.gameData.allRecipes.Count; i++)
            {
                ItemCombination current = GameData.gameData.allRecipes[i];
                combineResult = current.Combine();
                if (combineResult)
                {
                    // refresh the list
                    CombineCommand();
                }
            }

            //if (!combineResult) secondaryWindowLog.text = "  <color=red>This doesn't work...</color>";
        };


        newButton.transform.SetParent(actionButtonParent, false);
        newButton.GetComponentInChildren<Text>().text = "Combine";
        newButton.onClick.AddListener(combineCall);

        windowTitle.text = "COMBINE";
    }

    public void BattleScreenCommand(Enemy enemy)
    {
        if (!enemy.isActivated)
        {
            return;
        }

        activeWindow = battleWindow;
        duringBattle = true;

        // start battle music
        if (enemy.battleMusic != null)
        {
            GetComponent<AudioSource>().clip = enemy.battleMusic;
            GetComponent<AudioSource>().Play();
        }

        battleBackground.SetActive(true);
        battleEndScreen.SetActive(false);
        battleWindowTitle.text = "FIGHTING <color=red>" + enemy.enemyName + "</color>";
        battleWindowLog.text = enemy.enemyDescription + "\n";

        // first we clear all buttons
        for (int i = 0; i < battleActionButtonParent.childCount; i++)
        {
            Destroy(battleActionButtonParent.GetChild(i).gameObject);
        }

        // here we decide what buttons to show
        for (int i = 0; i < areaManager.inventory.Count; i++)
        {
            Item currentItem = areaManager.inventory[i];

            if (currentItem.isWeapon)
            {
                ShowBattleButton(enemy, currentItem);
            }
        }

        // here we add the defend and run command
        Button buttonToShow = Instantiate(battleActionButtonPrefab);
        buttonToShow.transform.SetParent(battleActionButtonParent, false);

        buttonToShow.GetComponentInChildren<Text>().text = "Defend";
        buttonToShow.onClick.AddListener(delegate { ButtonClickSound(null); Defend(enemy); });

        // run button
        buttonToShow = Instantiate(battleActionButtonPrefab);
        buttonToShow.transform.SetParent(battleActionButtonParent, false);

        buttonToShow.GetComponentInChildren<Text>().text = "Retreat";
        buttonToShow.onClick.AddListener(delegate { ButtonClickSound(null); Retreat(enemy); });
    }

    public void PortableTerminalCommand()
    {
        activeWindow = portableTerminalWindow;
        portableTextLog.text = "Portable terminal ver. 0.9\nTerminal online...";

        for (int i = 0; i < portableCommandsParent.childCount; i++)
        {
            Destroy(portableCommandsParent.GetChild(i).gameObject);
        }

        // add close button
        Button closeButton = Instantiate(portableCommandButton);
        closeButton.transform.SetParent(portableCommandsParent, false);

        closeButton.GetComponentInChildren<Text>().text = "Shut down";
        closeButton.onClick.AddListener(delegate { ReturnToMainWindowCommand(); });

        // we will add the buttons here
        // for each saved log
        for (int i = 0; i < GameData.gameData.savedLogs.Count; i++)
        {
            TerminalLog current = GameData.gameData.savedLogs[i];

            if (!current.isLocked)
            {
                // instantiate Panel buttons
                Button panelButton = Instantiate(portableCommandButton);
                panelButton.transform.SetParent(portableCommandsParent, false);

                string btnName = (current.isViewed ? "(Viewed)" : "(NEW)");
                panelButton.GetComponentInChildren<Text>().text = current.logLabel + " " + btnName;
                panelButton.onClick.AddListener(delegate { portableTextLog.text = current.LogString(); current.isViewed = true; btnName = (current.isViewed ? "(Viewed)" : "NEW"); panelButton.GetComponentInChildren<Text>().text = current.logLabel + " " + btnName; });
            }
        }
    }

    public void OptionsCommand()
    {
        // settings screen
        activeWindow = optionsScreen;
        soundFX.value = soundVolume;
        musicFX.value = musicVolume;

    }

    public void CreditsCommand()
    {
        activeWindow = creditsWindow;
    }
    #endregion

    #region SECONDARY WINDOW MANAGEMENT


    public void ClearListItems()
    {
        for (int i = 0; i < listItemParent.childCount; i++)
        {
            Destroy(listItemParent.GetChild(i).gameObject);
        }
    }

    public void ClearActionButtons()
    {
        for (int i = 0; i < actionButtonParent.childCount; i++)
        {
            Destroy(actionButtonParent.GetChild(i).gameObject);
        }
    }

    public void CreateTravelButton(Exit travelTo)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        string newDest = (travelTo.destination.isVisited ? "" : "<color=green>(NEW)</color>");
        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = travelTo.destination.name + " " + newDest;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = travelTo.textWhenDiscovering;

        // TRAVEL BUTTON
        int extraTickIfDark = (!areaManager.currentArea.isLit && !areaManager.currentArea.isLitByItem ? 1 : 0);
        string timeStamp = (travelTo.timeToTravel + extraTickIfDark == 1 ? " tick)" : " ticks)");
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = "Travel\n(" + (travelTo.timeToTravel + extraTickIfDark).ToString() + timeStamp;
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-extraTickIfDark); travelTo.Travel(); });

        // ABOUT BUTTON
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "About";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(1).GetComponent<Button>()); travelTo.About(); });
    }

    public void CreateInspectButton(Interactible inspect)
    {
        if (inspect.isDestroyed && !inspect.stillVisibleAfterDestroyed) return;

        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = inspect.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = inspect.properties.descriptionWhenFound;

        // USE BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = inspect.actionVerb;
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); inspect.Use(this); });
        if (inspect.staticInteractible || (inspect.destroyed && inspect.stillVisibleAfterDestroyed) || (!inspect.UsedAlone()))
        {
            listTransform.GetChild(1).GetChild(0).GetComponent<Button>().enabled = false;
            listTransform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.gray;
            listTransform.GetChild(1).GetChild(0).GetComponentInChildren<Text>().color = Color.gray;
        }

        // INSPECT BUTTON
        Button btn = listTransform.GetChild(1).GetChild(1).GetComponent<Button>();
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(btn); secondaryWindowLog.text = inspect.Inspect(); });
    }

    public void CreateUseOnButton(Interactible useOn)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = useOn.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = useOn.properties.descriptionWhenFound;

        // USE BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = "Use On";
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); selectedItemToUseOn.Use(this, useOn); });

        // INSPECT BUTTON
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(1).GetComponent<Button>()); secondaryWindowLog.text = useOn.Inspect(); });
    }

    public void CreateItemButton(Item item)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = "  " + item.itemDescriptionWhenFound;

        // PICKUP BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = "Pick up";
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); item.PickUpItem(this); Destroy(listItem.gameObject); });

        // INSPECT BUTTON
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(1).GetComponent<Button>()); secondaryWindowLog.text = item.Inspect(); });
    }

    public void CreateInventoryButton(Item item)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.itemDescription;

        string useText = item.actionVerb;

        // USE BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = useText;

        UnityEngine.Events.UnityAction useAction;

        if (item.IsUsableAlone())
        {
            if (!item.onlyOnPanels)
            {
                useAction = delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); item.Use(this, null); };
            }
            else
            {
                useAction = delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); selectedItemToUseOn = item; UseOnCommand(); };
            }
        }
        else
        {
            useAction = delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); selectedItemToUseOn = item; UseOnCommand(); };
        }

        string usetext = (item.usesLeft == 1 ? " use)" : " uses)");

        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(useAction);
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = useText + "\n(" + item.usesLeft + usetext;

        // INSPECT BUTTON
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(1).GetComponent<Button>()); secondaryWindowLog.text = item.Inspect(); });
    }

    public void CreateCommandPanelButton(CommandPanel terminal)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = terminal.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = terminal.panelDescription;

        // TURN ON BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = "Turn On";
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>()); ShowPanelCommand(terminal); });

        // INSPECT BUTTON
        Button btn = listTransform.GetChild(1).GetChild(1).GetComponent<Button>();
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(btn); secondaryWindowLog.text = terminal.Inspect(); });
    }

    public void CreateCommandPanelUseOnButton(CommandPanel terminal)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;
        selectedPanelToUse = terminal;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = terminal.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = terminal.panelDescription;

        // USE ON BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = "Use On";
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { selectedItemToUseOn.UseOnPanel(terminal); });

        // INSPECT BUTTON
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { secondaryWindowLog.text = terminal.Inspect(); });
    }

    public void CreateCombineButton(Item item)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(listItemParent, false);

        Transform listTransform = listItem.transform;

        listTransform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.name;
        listTransform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.itemDescription;

        string useText = "Combine";

        // USE BUTTON
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = useText;

        UnityEngine.Events.UnityAction useAction;

        useAction = delegate
        {
            ButtonClickSound(listTransform.GetChild(1).GetChild(0).GetComponent<Button>());
            if (!selectedItemsToCombine.Contains(item))
            {
                selectedItemsToCombine.Add(item);
                savedButtonColor = listTransform.GetChild(1).GetChild(0).GetComponent<Image>().color;
                listTransform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                selectedItemsToCombine.Remove(item);
                listTransform.GetChild(1).GetChild(0).GetComponent<Image>().color = savedButtonColor;
            }
        };

        string usetext = (item.usesLeft == 1 ? " use)" : " uses)");

        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(useAction);
        listTransform.GetChild(1).GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = "Combine\n(" + item.usesLeft + usetext;

        // INSPECT BUTTON
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Inspect";
        listTransform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ButtonClickSound(listTransform.GetChild(1).GetChild(1).GetComponent<Button>()); secondaryWindowLog.text = item.Inspect(); });
    }

    // Draw all 
    public void DrawAllInteractibleButtons()
    {
        // Clear the list
        ClearListItems();
        // now we build the screen
        for (int i = 0; i < areaManager.discoveredInteractibles.Count; i++)
        {
            CreateInspectButton(areaManager.discoveredInteractibles[i]);
        }
        // panels too
        for (int i = 0; i < areaManager.discoveredPanels.Count; i++)
        {
            CreateCommandPanelButton(areaManager.discoveredPanels[i]);
        }
    }

    public void DrawAllUseOnButtons()
    {
        // Clear the list
        ClearListItems();
        // now we build the screen
        if (!selectedItemToUseOn.onlyOnPanels)
        {
            for (int i = 0; i < areaManager.discoveredInteractibles.Count; i++)
            {
                CreateUseOnButton(areaManager.discoveredInteractibles[i]);
            }
        }
        // panels too
        if (selectedItemToUseOn.onlyOnPanels)
        {
            for (int i = 0; i < areaManager.discoveredPanels.Count; i++)
            {
                CreateCommandPanelUseOnButton(areaManager.discoveredPanels[i]);
            }
        }
    }

    public void DrawAllGroundItemsButtons()
    {
        ClearListItems();
        // now we build the screen
        for (int i = 0; i < areaManager.discoveredItems.Count; i++)
        {
            CreateItemButton(areaManager.discoveredItems[i]);
        }
    }

    public void DrawAllInventoryButtons()
    {
        ClearListItems();
        // now we build the screen
        for (int i = 0; i < areaManager.inventory.Count; i++)
        {
            CreateInventoryButton(areaManager.inventory[i]);
        }
    }

    public void DrawAllCombineButtons()
    {
        ClearListItems();
        // now we build the screen
        for (int i = 0; i < areaManager.inventory.Count; i++)
        {
            CreateCombineButton(areaManager.inventory[i]);
        }
    }
    #endregion

    #region BATTLE
    public void ShowBattleButton(Enemy enemy, Item weaponItem)
    {
        Button buttonToShow = Instantiate(battleActionButtonPrefab);
        buttonToShow.transform.SetParent(battleActionButtonParent, false);

        buttonToShow.GetComponentInChildren<Text>().text = "Attack with " + weaponItem.name;
        buttonToShow.onClick.AddListener(delegate { ButtonClickSound(null); AttackEnemy(enemy, weaponItem); });
    }

    public void ShowRetreatScreen(Enemy enemy)
    {
        battleBackground.SetActive(false);
        battleEndScreen.SetActive(true);
        battleWinTitleText.text = "<color=red>Retreat!</color>";
        battleDetailsText.text = "  You have managed to escape " + enemy.enemyName + "!";
        battleContinueButton.onClick.AddListener(delegate { battleEndScreen.SetActive(false); ReturnToMainWindowCommand(); });
    }

    public void ShowWinScreen(Enemy enemy)
    {
        battleBackground.SetActive(false);
        battleEndScreen.SetActive(true);
        battleWinTitleText.text = "<color=red>Victory!</color>";
        battleDetailsText.text = "  You have managed to defeat " + enemy.enemyName + "!";
        battleContinueButton.onClick.AddListener(delegate { battleEndScreen.SetActive(false); PickUpCommand(); GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-5); });
    }

    public void ShowEnemyEscapeScreen(Enemy enemy)
    {
        battleBackground.SetActive(false);
        battleEndScreen.SetActive(true);
        battleWinTitleText.text = "<color=red>Victory!</color>";
        battleDetailsText.text = "  " + enemy.enemyName + " managed to escape into the darkness!";
        duringBattle = false;
        battleContinueButton.onClick.AddListener(delegate { battleEndScreen.SetActive(false); ReturnToMainWindowCommand(); });
    }

    public void Retreat(Enemy enemy)
    {
        int chanceToRetreat = (int)GameData.gameData.GetComponent<PlayerHealth>().currentEnergy;
        int roll = Random.Range(0, (int)GameData.gameData.GetComponent<PlayerHealth>().MaxEnergy);

        if (roll <= chanceToRetreat)
        {
            ShowRetreatScreen(enemy);
            // retreat
            AddTextWithReturn("  You have escaped " + enemy.enemyName, Color.green);
            duringBattle = false;
            // remove energy
            GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-5);
        }
        else
        {
            battleWindowLog.text += "\n<color=red>  " + enemy.enemyName + " blocks your retreat!</color>";
            // we give the turn to the enemy
            battleWindowLog.text += "\n" + enemy.AttackPlayer();
        }
    }

    public void Defend(Enemy enemy)
    {
        GameData.gameData.GetComponent<PlayerHealth>().current += 1;
        battleWindowLog.text += "\n  You get into defensive position!";
        battleWindowLog.text += "\n" + enemy.AttackPlayer();
    }

    public void AttackEnemy(Enemy enemy, Item item)
    {
        // first we try and attack
        battleWindowLog.text += "\n" + item.AttackEnemy(enemy);
        // check if need to escape
        if (enemy.currentEnemyArmor < enemy.enemyArmor / 3)
        {
            if (enemy.TryEscape())
            {
                ShowEnemyEscapeScreen(enemy);
                // escaped
                AddTextWithReturn(enemy.runAwayDescription, Color.red);

                // remove energy from player
                GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-5);

                // here we make the enemy heal
                enemy.currentEnemyArmor += enemy.enemyArmor / 3;
            }
        }
        // check if enemy is dead
        enemy.isDestroyed = enemy.IsDead();
        if (enemy.isDestroyed)
        {
            ShowWinScreen(enemy);
            AddTextWithReturn(enemy.defeatDescription, Color.green);
            // give reward here, if we roll >:)
            for (int i = 0; i < enemy.loot.Count; i++)
            {
                if (i < enemy.chanceToDrop.Count)
                {
                    float roll = Random.Range(0, 100f);
                    if (roll <= enemy.chanceToDrop[i])
                        areaManager.discoveredItems.Add(enemy.loot[i]);
                }              
            }
        }
        else
        {
            // if it is not destroyed and it doesnt try to run, it attacks
            battleWindowLog.text += "\n" + enemy.AttackPlayer();
        }
    }
    #endregion

    #region PANEL RELATED
    public void AddDigit(int digit)
    {
        if (inputPassword.Length < 4)
        {
            inputPassword += digit.ToString();
            passwordText.text += digit.ToString();
        }
        else
        {
            passwordText.text = "";
            inputPassword = "";
        }
    }

    public void VerifyPassword(CommandPanel panel)
    {

        if (inputPassword == panel.password)
        {
            panel.isPasswordProtected = false;
            inputPassword = "";
            passwordText.text = "";
            ShowPanelCommand(panel);

        }
        else
        {
            ShowPanelCommand(panel);
        }
    }
    #endregion

    #region EVENTS

    public void ShowEventScreen(Event currentEvent)
    {
        activeWindow = eventWindow;

        eventTitleText.text = currentEvent.eventName;
        eventMessageText.text = currentEvent.eventDescription;

        // clear event options buttons
        for (int i = 0; i < eventOptionsParent.childCount; i++)
        {
            Destroy(eventOptionsParent.GetChild(i).gameObject);
        }

        // we build all the option buttons
        for (int i = 0; i < currentEvent.optionText.Count; i++)
        {
            Button option = Instantiate(actionButtonPrefab);
            option.transform.SetParent(eventOptionsParent, false);
            option.GetComponentInChildren<Text>().text = currentEvent.optionText[i];
            Event bugEvent = currentEvent;
            int optionNumber = i;
            option.onClick.AddListener(delegate { bugEvent.ExecutePlayerOption(optionNumber); }); // if (!eventRunning || !duringBattle) ReturnToMainWindowCommand(); });
        }
    }

    #endregion

    #region BUTTONS
    Color oldButtonColor;

    public void ChangeButtonToColor(Button buton, Color newColor)
    {

        buton.GetComponent<Image>().color = newColor;
        //buton.GetComponentInChildren<Text>().color = newColor;
    }

    #endregion

    #region OPTIONS
    public void CloseOptions()
    {
        // we will also save
        soundVolume = soundFX.value;
        musicVolume = musicFX.value;

        activeWindow = previousWindow;
    }

    public void ChangeSound()
    {
        soundVolume = soundFX.value;
        PlayerPrefs.SetFloat("Sound", soundVolume);
    }

    public void ChangeMusic()
    {
        musicVolume = musicFX.value;
        PlayerPrefs.SetFloat("Music", musicVolume);
    }

    public void CloseCredits()
    {
        activeWindow = menuWindow;
    }
    #endregion

    #region ADS
#if UNITY_ANDROID
#if UNITY_ADS
    bool rewardShield = false;
    bool rewardEnergy = false;
 //   float adTime = 0f;
    bool showAd = true;

    public void ShowRewardedAd()
    {
        const string RewardedPlacementId = "rewardedVideo";

        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            activeWindow = noAdWindow;
            return;
        }


        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                // Give back hp
                rewardShield = true;
                GameData.gameData.GetComponent<PlayerHealth>().ModifyHealth(1f);
                activeWindow = thankYouWindow;

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void ShowRewardedAdEnergy()
    {
        const string RewardedPlacementId = "rewardedvideoenergy";

        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            activeWindow = noAdWindow;
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResultEnergy };
        Advertisement.Show(RewardedPlacementId, options);
    }

    private void HandleShowResultEnergy(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                // Give back energy
                rewardEnergy = true;
                if (GameData.gameData.GetComponent<PlayerHealth>().currentEnergy == 0) GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(1f);
                activeWindow = thankYouWindow;

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void ClaimReward()
    {
        if (rewardShield)
        {
            GameData.gameData.GetComponent<PlayerHealth>().current = (int)GameData.gameData.GetComponent<PlayerHealth>().MaxValue/2;
            rewardShield = false;
            ReturnToMainWindowCommand();
            return;
        }
        if (rewardEnergy)
        {
            GameData.gameData.GetComponent<PlayerHealth>().currentEnergy = GameData.gameData.GetComponent<PlayerHealth>().MaxEnergy;
            rewardEnergy = false;
            ReturnToMainWindowCommand();
            return;
        }
    }

    public void SwitchAdButtonEnergy()
    {
        noEnergyWindow.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(showAd);
    }
#endif
#endif

    #endregion

    #region REALTIME MANAGEMENT

    private void Update()
    {
        // Background rotation
        background.rectTransform.Rotate(0, 0, 0.02f);

        // bool switches react if changed
        for (int i = 0; i < GameData.gameData.allBooleanSwitches.Count; i++)
        {
            BooleanSwitch bs = GameData.gameData.allBooleanSwitches[i];
            bs.CheckAndReact();
        }

        // Window management
        if (currentWindow != activeWindow)
        {
            previousWindow = currentWindow;
            if (currentWindow != null) currentWindow.SetActive(false);
            currentWindow = activeWindow;
            currentWindow.SetActive(true);
        }
#if UNITY_ADS
        // Ad window management
        if (currentWindow == noEnergyWindow)
        {
            // we check if we have an ad available. only then we mark the button
            watchEnergyAdButton.interactable = Advertisement.IsReady("rewardedvideoenergy");
        }
        if (currentWindow == deathWindow)
        {
            watchDeathAdButton.interactable = Advertisement.IsReady("rewardedVideo");
        }
#endif

        // Button management to show the player if things are new around
        if (areaManager.discoveredItems.Count > 0)
        {
            ChangeButtonToColor(pickupButton, Color.blue);
        }
        else
        {
            ChangeButtonToColor(pickupButton, oldButtonColor);
        }

        if (areaManager.discoveredInteractibles.Count > 0 || areaManager.discoveredPanels.Count > 0)
        {
            ChangeButtonToColor(inspectButton, Color.blue);
        }
        else
        {
            ChangeButtonToColor(inspectButton, oldButtonColor);
        }

        if (areaManager.discoveredSecretArea)
        {
            ChangeButtonToColor(travelButton, Color.blue);

        }
        else
        {
            ChangeButtonToColor(travelButton, oldButtonColor);
        }

        // Show energy and health info
        energyText.text = "Energy:" + GameData.gameData.GetComponent<PlayerHealth>().currentEnergy.ToString() + "/" + GameData.gameData.GetComponent<PlayerHealth>().MaxEnergy.ToString();
        shieldText.text = "Shield:" + GameData.gameData.GetComponent<PlayerHealth>().current.ToString() + "/" + GameData.gameData.GetComponent<PlayerHealth>().MaxValue.ToString();

        // music and sound
        if (GetComponent<AudioSource>().volume != musicVolume) GetComponent<AudioSource>().volume = musicVolume;
        if (pulsarSFX.GetComponent<AudioSource>().volume != soundVolume) pulsarSFX.GetComponent<AudioSource>().volume = soundVolume;

        // is lit button
        if (areaManager.currentArea != null)
        {
            if (areaManager.currentArea.isLit || areaManager.currentArea.isLitByItem)
            {
                isAreaLit.color = Color.white;
            }
            else
            {
                isAreaLit.color = Color.clear;
            }
        }
    }

#endregion
}
