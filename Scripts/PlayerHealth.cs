using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour {

    public float MaxValue = 25f;
    public float current = 25f;

    public float MaxEnergy = 40f;
    public float currentEnergy = 40f;

    public float environmentDamageResistance = 0f;

    public float timeDelta = 0f;
    public float mins;

    public void PlayerReset()
    {
        current = MaxValue;
        currentEnergy = MaxEnergy;
        timeDelta = 0f;
        environmentDamageResistance = 0f;
    }

    public void LoadPlayerData(PlayerSave data)
    {
        MaxValue = data.MaxShield;
        current = data.CurrentShield;
        MaxEnergy = data.MaxEnergy;
        currentEnergy = data.CurrentEnergy;
        environmentDamageResistance = data.EnvironmentalResistance;

        // now we check how much we add to the energy of the player
        DateTime currentTime = DateTime.Now;
        DateTime savedTime = data.timeofSave;

        TimeSpan timePassed = currentTime.Subtract(savedTime);
        int numEnergy = timePassed.Minutes/5;

        // add the energy
        currentEnergy += numEnergy;
        if (currentEnergy > MaxEnergy) currentEnergy = MaxEnergy;
    }

    public void ModifyHealth(float dH)
    {
        float modifiedDH;

        if (dH < 0)
        {
            modifiedDH = dH + environmentDamageResistance;
            if (modifiedDH > 0) modifiedDH = 0f;
            current += modifiedDH;
            if (current <= 0) current = 0;
            GameController.controller.AddTextWithReturn("  The personal shield is taking all the damage! Personal shield at " + current.ToString() + " % ", Color.red);
        }
        else
        {
            current += dH;
            if (current > MaxValue) current = MaxValue;
            GameController.controller.AddTextWithReturn("  Personal shield at " + current.ToString() + "%", Color.red);
        }
    }

    public void ModifyEnergy(float dE)
    {
        currentEnergy += dE;
        if (currentEnergy <= 0) currentEnergy = 0;
     
    }
    private void Update()
    {
        // check for death
        if (current == 0)
        {
            // Show death screen
            GameController.controller.activeWindow = GameController.controller.deathWindow;
        }

        if (currentEnergy == 0 && GameController.controller.activeWindow == GameController.controller.mainWindow)
        {
            // Show beg for money screen
            GameController.controller.activeWindow = GameController.controller.noEnergyWindow;
            // set it to 1
            currentEnergy = 1;
        }

        // restore movement energy in real time
        timeDelta += Time.deltaTime;

        mins = Mathf.Floor(timeDelta/60);
        
        if (mins >= 2)
        {
            timeDelta = 0;
            ModifyEnergy(1);
            
        }

        // keep the energy in bounds
        if (currentEnergy >= MaxEnergy) currentEnergy = MaxEnergy;
    }
}
