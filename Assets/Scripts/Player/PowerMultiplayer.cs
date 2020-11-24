using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerMultiplayer : MonoBehaviour
{
    public TMP_Text multiplayerText;
    public int multiplayer = 1;
    public int multiplayerSteps = 10;
    private int multiplayerCurrentStep = 0;

    void Start()
    {
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

}
