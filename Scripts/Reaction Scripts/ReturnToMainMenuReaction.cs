using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName =("Add/Reactions/Misc/Return to main menu"))]
public class ReturnToMainMenuReaction : Reaction
{
    public override void ExecuteReaction(GameController controller)
    {
        // close all windows?
        controller.ReturnToMainWindowCommand();
        
        // SAVE FIRST
        GameData.gameData.SaveGame();
  
        controller.imagePanel.GetComponent<Image>().color = Color.clear;

        controller.StartReturnToMainMenu();
    }
}
