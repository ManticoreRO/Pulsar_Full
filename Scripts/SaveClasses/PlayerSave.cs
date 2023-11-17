using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerSave
{
    public float MaxShield;
    public float CurrentShield;
    public float MaxEnergy;
    public float CurrentEnergy;
    public float EnvironmentalResistance;
    public DateTime timeofSave;
    public string currentArea;
    //public string previousArea;
    public bool isportablePickedUp;
    public bool relayfixed;
    public int savedTicks;
    public int savedCycles;
    public float backgroundCurrentRotation;
    public float soundVol;
    public float musicVol;
    public bool isDuringBattle;

    //public string[] buffer;

    public PlayerSave()
    { }

    public PlayerSave(PlayerHealth player)
    {
        if (GameController.controller.activeWindow != GameController.controller.menuWindow && GameController.controller.activeWindow != GameController.controller.optionsScreen)
        {
            #region REMOVED NEW CODE
            /*MaxShield = player.MaxValue;
            CurrentShield = player.current;
            MaxEnergy = player.MaxEnergy;
            CurrentEnergy = player.currentEnergy;
            EnvironmentalResistance = player.environmentDamageResistance;
            timeofSave = DateTime.Now;
            // check first if it is not a not save area
            if (!GameController.controller.areaManager.currentArea.noSaveArea)
            {
                currentArea = GameController.controller.areaManager.currentArea.name;
                if (GameController.controller.areaManager.previousArea != null)
                {
                    //previousArea = GameController.controller.areaManager.previousArea.name;
                }
                else
                {
                    //previousArea = "";
                }
            }
            else
            {
                // else we save the previous area
                currentArea = GameController.controller.areaManager.previousArea.name;
                //previousArea = currentArea;
            }

            isportablePickedUp = GameController.controller.portablePickedUp;
            relayfixed = GameController.controller.relayFixed;
            savedTicks = GameController.controller.currentHour;
            savedCycles = GameController.controller.currentDay;
            // save current background state
            backgroundCurrentRotation = GameController.controller.background.rectTransform.rotation.eulerAngles.z;
            soundVol = GameController.controller.soundVolume;
            musicVol = GameController.controller.musicVolume;
            isDuringBattle = GameController.controller.duringBattle;

           /* buffer = new string[100];
            for (int i = 0; i < 100; i++)
            {
                buffer[i] = "";
            } */
            #endregion

            MaxShield = player.MaxValue;
            CurrentShield = player.current;
            MaxEnergy = player.MaxEnergy;
            CurrentEnergy = player.currentEnergy;
            EnvironmentalResistance = player.environmentDamageResistance;
            timeofSave = DateTime.Now;
            currentArea = GameController.controller.areaManager.currentArea.name;
            isportablePickedUp = GameController.controller.portablePickedUp;
            relayfixed = GameController.controller.relayFixed;
            savedTicks = GameController.controller.currentHour;
            savedCycles = GameController.controller.currentDay;
            // save current background state
            backgroundCurrentRotation = GameController.controller.background.rectTransform.rotation.eulerAngles.z;
            soundVol = GameController.controller.soundVolume;
            musicVol = GameController.controller.musicVolume;
            isDuringBattle = GameController.controller.duringBattle;
        }
    }
}
