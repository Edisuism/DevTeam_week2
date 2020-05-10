using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    //Called from ghost audio
    public void AdjustHeartRate(float ghostDistance, float farDistance)
    {
        //Really wanted to use a switch statement here but apparently
        //they don't support ranges. Used IF/ELSE instead
        if (ghostDistance < farDistance / 4)
        {
            audioManager.PlayHeartBeat("rapidbeat");
        }
        else if (ghostDistance < farDistance / 2)
        {
            audioManager.PlayHeartBeat("fastbeat");
        }
        else if (ghostDistance < farDistance)
        {
            audioManager.PlayHeartBeat("slowbeat");
        }
        else
        {
            audioManager.PlayHeartBeat("stop");
        }
    }
}
