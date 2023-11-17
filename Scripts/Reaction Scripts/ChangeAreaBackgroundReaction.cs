using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Add/Reactions/Visuals/Change area background")]
public class ChangeAreaBackgroundReaction : Reaction
{
    public Area whichArea;
    public int newIndex = 0;
    
    public override void ExecuteReaction(GameController controller)
    {
        if (whichArea == null) return;

        if (newIndex >= whichArea.backgroundImages.Count) return;

        whichArea.currentBackground = newIndex;

        if (whichArea == controller.areaManager.currentArea)
        {
            controller.imagePanel.GetComponent<Image>().sprite = whichArea.backgroundImages[whichArea.currentBackground];
            if (whichArea.backgroundImages.Count == 0)
            {
                controller.imagePanel.GetComponent<Image>().color = Color.black;
            }
            else
            {
                controller.imagePanel.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
