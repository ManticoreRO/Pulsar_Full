using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManagement : MonoBehaviour
{
    public Area mainArea;
    public Area currentArea;
    public Area previousArea;

    GameController controller;


    // Discovered exits from the area
    [HideInInspector]
    public List<Exit> discoveredExits = new List<Exit>();
    // Discovered interactibles in the area
    [HideInInspector]
    public List<Interactible> discoveredInteractibles = new List<Interactible>();
    // Discovered items in the area
    [HideInInspector]
    public List<Item> discoveredItems = new List<Item>();
    // Inventory
    
    public List<Item> inventory = new List<Item>();
    // Panels
    [HideInInspector]
    public List<CommandPanel> discoveredPanels = new List<CommandPanel>();


    // TO ADD LIGHTING BY ITEM
    [HideInInspector]
    public bool discoveredSecretArea = false;
    [HideInInspector]
    public bool foundSomething = false;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void ChangeArea(Area newArea)
    {
        controller.areaChanged = false;
        previousArea = currentArea;
        // make sure we unlit the previous area
        if (currentArea != null)
        {
            if (currentArea.isLitByItem) currentArea.isLitByItem = false;
        }

        // pulsar sfx
        if (GameController.controller.soundVolume != 0)
        {
            GameController.controller.pulsarSFX.GetComponent<AudioSource>().volume = GameController.controller.soundVolume + newArea.pulsingIntensity;
        }
        else
        {
            GameController.controller.pulsarSFX.GetComponent<AudioSource>().volume = 0f;
        }
        currentArea = newArea;
        InitDiscoveredExits();
        InitDiscoveredInteractibles();
        InitDiscoveredItems();
        InitDiscoveredPanels();

        // Check if fully explored first
        CheckIfFullyExplored();

        GameController.controller.areaNameText.text = currentArea.name;

        controller.AddTextWithReturn(newArea.GetAreaDescription(), Color.white);

        if (!GameController.controller.eventRunning) GameController.controller.ReturnToMainWindowCommand();
        // we also need to check if our travel screen is still open... damn bug
        if (controller.activeWindow == controller.secondaryWindow) GameController.controller.ReturnToMainWindowCommand();

        // change area state to visited
        newArea.isVisited = true;
        discoveredSecretArea = false;
        //change sounds and background
        if (currentArea.musicFile != null && controller.GetComponent<AudioSource>().clip != currentArea.musicFile)
        {
            controller.GetComponent<AudioSource>().volume = controller.musicVolume;
            controller.GetComponent<AudioSource>().clip = currentArea.musicFile;
            controller.GetComponent<AudioSource>().Play();
        }

        controller.imagePanel.GetComponent<Image>().sprite = currentArea.backgroundImages[currentArea.currentBackground];
        if (currentArea.backgroundImages.Count == 0)
        {
            controller.imagePanel.GetComponent<Image>().color = Color.black;
        }
        else
        {
            controller.imagePanel.GetComponent<Image>().color = Color.white;
        }

        controller.areaChanged = true;

        // reaction on enter
        if (currentArea.onEnterArea != null)
        {
            currentArea.onEnterArea.ExecuteReaction(GameController.controller);
            return;
        }

        // check for enemies
        if (!GameData.gameData.justLoaded)
        {
            for (int i = 0; i < GameData.gameData.allEnemies.Count; i++)
            {
                Enemy enemy = GameData.gameData.allEnemies[i];
                if (enemy.areasToAppearIn.Contains(currentArea))
                {
                    int roll = Random.Range(0, 100);
                    if (roll <= enemy.chanceToSpawn[i] && !enemy.isDestroyed && enemy.isActivated)
                    {
                        // spawn and battle
                        controller.BattleScreenCommand(enemy);
                        return;
                    }
                }
            }
        }
        else
        {
            GameData.gameData.justLoaded = false;
        }

        // check for random events
        // if during an event, skip this
        if (!GameController.controller.eventRunning)
        {
            for (int i = 0; i < GameData.gameData.allEvents.Count; i++)
            {
                Event cEv = GameData.gameData.allEvents[i];
                if (cEv.randomEvent)
                {
                    if (cEv.TryTrigger())
                    {
                        controller.ShowEventScreen(cEv);
                        return; // bail
                    }
                }
            }
        }
    }


    #region EXPLORATION
    // Prepare exploration list for new area
    public void InitDiscoveredExits()
    {
        discoveredExits.Clear();
        for (int i = 0; i < currentArea.availableExits.Count; i++)
        {
            Exit exit = currentArea.availableExits[i];
            if (exit.isDiscovered && !exit.isLocked)
            {
                // Add it
                discoveredExits.Add(exit);
            }
        }
    }

    // Explore current area
    public void Explore()
    {
        if (GameData.gameData.GetComponent<PlayerHealth>().currentEnergy >= currentArea.exploreTime)
        {
            InitDiscoveredExits();
            string textToShow = "";
            for (int i = 0; i < currentArea.availableExits.Count; i++)
            {
                Exit exit = currentArea.availableExits[i];

                // only if not discovered, we try to discover it
                if (!exit.isDiscovered)
                {
                    bool result = exit.TryDiscover();
                    if (result)
                    {
                        discoveredExits.Add(exit);
                        exit.isDiscovered = true;
                        textToShow += " ";
                        // we should make the button glow
                        //GameController.controller.ChangeButtonToColor(GameController.controller.travelButton, Color.yellow);
                        //textToShow += exit.textWhenDiscovering + " ";
                    }
                }
            }

            if (textToShow != "")
            {
                GameController.controller.AddTextWithReturn("  You discovered a new place to travel!", Color.green);
                GameController.controller.areaManager.discoveredSecretArea = true;
            }
            GameController.controller.AddTime(currentArea.exploreTime);
            GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-currentArea.exploreTime);
            
            // increasing lookaround time
            currentArea.exploreTime += (int)(currentArea.exploreTime+1)/2;
            if (currentArea.exploreTime == 0) currentArea.exploreTime = 1;
            if (currentArea.exploreTime >= 16) currentArea.exploreTime = 16;

            CheckIfFullyExplored();
            GameController.controller.ExploreCommand();
        }
        else
        {
            // We show the player the beg for money screen
            GameController.controller.activeWindow = GameController.controller.noEnergyWindow;
        }
    }
    #endregion

    #region INTERACTIBLES
    public void InitDiscoveredInteractibles()
    {
        discoveredInteractibles.Clear();

        for (int i = 0; i < GameData.gameData.allInteractibles.Count; i++)
        {
            Interactible interactible = GameData.gameData.allInteractibles[i];

            if (interactible.isFound && !interactible.isDestroyed && interactible.locatedInArea == currentArea)
            {
                discoveredInteractibles.Add(interactible);
                
            }
        }
    }

    // Looks around for interactibles, items and/or factories
    public void LookAround()
    {
        if (GameData.gameData.GetComponent<PlayerHealth>().currentEnergy >= currentArea.lookAroundTime)

        {
            string textToShow = "";

            for (int i = 0; i < GameData.gameData.allInteractibles.Count; i++)
            {
                Interactible interactible = GameData.gameData.allInteractibles[i];

                if (!interactible.isFound && interactible.locatedInArea == currentArea)
                {
                    bool result = interactible.TryFindInteractible(currentArea.isLit || currentArea.isLitByItem);
                    if (result)
                    {
                        discoveredInteractibles.Add(interactible);
                        interactible.isFound = true;
                        textToShow += interactible.properties.descriptionWhenFound + " ";
                        foundSomething = true;
                    }
                }
            }

            if (textToShow == "")
            {
                // We discovered nothing
                foundSomething = false;
                controller.secondaryWindowLog.text = "  <color=red>You noticed nothing of interest.</color>";
                if (!currentArea.isLit && !currentArea.isLitByItem) controller.secondaryWindowLog.text += "<color=red> It is too dark to see anything!</color>";
            }
            else
            {
                //controller.AddTextWithReturn(textToShow, Color.cyan);
                controller.DrawAllInteractibleButtons();
            }
            // time passing
            GameController.controller.AddTime(currentArea.lookAroundTime);
            GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-currentArea.lookAroundTime);

            // increasing lookaround time
            currentArea.lookAroundTime += (int)(currentArea.lookAroundTime+1)/2;
            if (currentArea.lookAroundTime >= 16) currentArea.lookAroundTime = 16;

            CheckIfFullyExplored();
        }
        else
        {
            // We show the player the beg for money screen
            GameController.controller.activeWindow = GameController.controller.noEnergyWindow;
        }
    }
    #endregion

    #region ITEMS
    public void InitDiscoveredItems()
    {
        discoveredItems.Clear();

        for (int i = 0; i < GameData.gameData.allItems.Count; i++)
        {
            Item item = GameData.gameData.allItems[i];

            if (item.isFound && !item.isDestroyed && !item.isInInventory && item.foundInArea == currentArea)
            {
                discoveredItems.Add(item);
            }
        }
    }

    // Search for items to use
    public void Search()
    {
        if (GameData.gameData.GetComponent<PlayerHealth>().currentEnergy >= currentArea.searchTime)

        {
            string textToShow = "";

            for (int i = 0; i < GameData.gameData.allItems.Count; i++)
            {
                Item item = GameData.gameData.allItems[i];

                if (!item.isFound && !item.isInInventory && item.foundInArea == currentArea)
                {
                    bool result = item.TryDiscoverItem(currentArea.isLit || currentArea.isLitByItem);
                    if (result)
                    {
                        discoveredItems.Add(item);
                        item.isFound = true;
                        textToShow += item.itemDescriptionWhenFound + " ";
                        foundSomething = true;

                    }
                }
            }

            if (textToShow == "")
            {
                // We discovered nothing
                foundSomething = false;
                controller.secondaryWindowLog.text = "  <color=red>You noticed nothing of interest.</color>";
                if (!currentArea.isLit && !currentArea.isLitByItem) controller.secondaryWindowLog.text += " It is too dark to see anything!";
            }
            else
            {
                //controller.AddTextWithReturn(textToShow, Color.cyan);
                controller.DrawAllGroundItemsButtons();
            }

            GameController.controller.AddTime(currentArea.searchTime);
            GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(-currentArea.searchTime);

            // increasing time to search
            currentArea.searchTime += (int)(currentArea.searchTime+1)/2;

            if (currentArea.searchTime == 0) currentArea.searchTime = 1;
            if (currentArea.searchTime >= 16) currentArea.searchTime = 16;

            CheckIfFullyExplored();
        }
        else
        {
            // We show the player the beg for money screen
            GameController.controller.activeWindow = GameController.controller.noEnergyWindow;
        }
    }

    public void CheckIfFullyExplored()
    {
        // check interactibles
        currentArea.noInteractiblesLeft = true;
        for (int i = 0; i < GameData.gameData.allInteractibles.Count; i++)
        {
            Interactible current = GameData.gameData.allInteractibles[i];
            if (current.locatedInArea == currentArea && (!current.isFound) && (!current.properties.secret)) currentArea.noInteractiblesLeft = false;
        }
        
        // check panels
        currentArea.noPanelsLeft = true;
        for (int i = 0; i < GameData.gameData.allCommandPanels.Count; i++)
        {
            CommandPanel current = GameData.gameData.allCommandPanels[i];
            if (current.locatedInArea == currentArea && (!current.isDiscovered)) currentArea.noPanelsLeft = false;
        }
        
        // check items
        currentArea.noItemsLeft = true;
        for (int i = 0; i < GameData.gameData.allItems.Count; i++)
        {
            Item current = GameData.gameData.allItems[i];
            if (current.foundInArea == currentArea && (!current.isFound) && (!current.isSecret)) currentArea.noItemsLeft = false;
        }
        
        // check exits
        currentArea.noExitsLeft = true;
        for (int i = 0; i < currentArea.availableExits.Count; i++)
        {
            Exit current = currentArea.availableExits[i];
            if (!current.isDiscovered && !current.secret && !current.isLocked) currentArea.noExitsLeft = false;
        }
        
        // if we got here then we have discovered everything
        currentArea.isExplored = true;
    }
    #endregion

    #region PANELS
    public void InitDiscoveredPanels()
    {
        discoveredPanels.Clear();
        for (int i = 0; i < GameData.gameData.allCommandPanels.Count; i++)
        {
            CommandPanel currentPanel = GameData.gameData.allCommandPanels[i];
            if (currentPanel.isDiscovered && currentPanel.locatedInArea == currentArea)
                discoveredPanels.Add(currentPanel);
        }
    }
    #endregion
}
