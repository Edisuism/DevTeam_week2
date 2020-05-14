using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class OutletScript : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerBattery;
    public float outletBattery;
    public float chargeIncrement;
    public Light2D outletLight;
    public GameObject particleSparks;
    private AudioManager audioManager;
    private bool isCharging = true;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        outletLight = GetComponentInChildren<Light2D>();
    }

    private void Update() {
        if(playerBattery != null && isCharging)
        {
            if(outletBattery <= 0)
            {
                outletLight.intensity = 0;
                particleSparks.SetActive(false);
                audioManager.Stop("charging");
                if(!audioManager.IsPlaying("shutdown"))
                    audioManager.Play("shutdown");
            }
            else if(playerBattery.batteryCap >= playerBattery.battery)
            {
                playerBattery.battery += chargeIncrement;
                outletBattery -= chargeIncrement;

                if(!audioManager.IsPlaying("charging"))
                    audioManager.Play("charging");
            }
            else if(playerBattery.batteryCap <= playerBattery.battery)
            {
                if(!audioManager.IsPlaying("finishcharging"))
                {
                    audioManager.Play("finishcharging");
                }
                    
                audioManager.Stop("charging");
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && outletBattery > 0)
        {
            player = collision.gameObject;
            playerBattery = player.GetComponent<PlayerController>();
            audioManager.Play("startcharge");
            isCharging = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
            isCharging = false;
            audioManager.Play("unplug");
            audioManager.Stop("charging");
        }
    }
}
