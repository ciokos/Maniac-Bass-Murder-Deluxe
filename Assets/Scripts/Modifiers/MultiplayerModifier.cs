using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerModifier : IModifier
{
    public TMP_Text multiplayerText;
    public int multiplayer = 1;
    public int multiplayerSteps = 1;
    private int multiplayerCurrentStep = 0;
    public void Modify(List<BulletParameters> bullets)
    {
        if(!bullets[0].isEmpowered)
        {
            ResetMultiplayer();
        }
        foreach(BulletParameters b in bullets)
        {
            b.dmg *= GetMultiplayer();
        }
    }

    public MultiplayerModifier(TMP_Text text)
    {
        this.multiplayerText = text;
        SetMultiplayerText();
    }

    private void SetMultiplayerText()
    {
        multiplayerText.text = "x" + multiplayer.ToString();
    }
    public int GetMultiplayer()
    {
        return multiplayer;
    }

    public void ResetMultiplayer()
    {
        multiplayer = 1;
        multiplayerCurrentStep = 0;
        SetMultiplayerText();
    }

    public void IncreaseMultiplayerStep()
    {
        multiplayerCurrentStep++;
        if (multiplayerCurrentStep == multiplayerSteps)
        {
            multiplayer++;
            multiplayerCurrentStep = 0;
            SetMultiplayerText();
        }
    }

    public void EnemyHit(bool isEmpowered)
    {
        if (isEmpowered)
            IncreaseMultiplayerStep();
    }

}
