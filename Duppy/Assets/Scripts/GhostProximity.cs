using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProximity : MonoBehaviour
{
    public enum GhostDistance { Far = 8, Middle = 6, Near = 4, Closeby = 2, OnTarget = 0}
    public GhostDistance ghostDistance;
    public bool isGhostDead = false;

    private GameObject targetObject;
    private float distanceDifference;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        targetObject = GameObject.FindGameObjectWithTag("Player");
        ghostDistance = GhostDistance.Far;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostDistance != GhostDistance.OnTarget && !isGhostDead)
            CheckDistance();
        else {
            StopGhostNoises();
            if (isGhostDead && !audioManager.IsPlaying("ghostdeath"))
            {
                audioManager.Play("ghostdeath");
            }
        }
    }

    //If the ghost touches the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetObject && ghostDistance != GhostDistance.OnTarget)
        {
            //TODO: CALL PLAYER CONTROLLER AND FREEZE THE PLAYER
            ghostDistance = GhostDistance.OnTarget;
            StopGhostNoises();
            audioManager.Play("playercaught");
        }
    }

    private void CheckDistance()
    {
        distanceDifference = Vector3.Distance(targetObject.transform.position, this.gameObject.transform.position);

        if (distanceDifference < (float)GhostDistance.Closeby)
            PerformDistanceFunctions(GhostDistance.Closeby);
        else if (distanceDifference < (float)GhostDistance.Near)
            PerformDistanceFunctions(GhostDistance.Near);
        else if (distanceDifference < (float)GhostDistance.Middle)
        {
            //TODO: CALL GHOST AI TO START CHASING THE PLAYER
            PerformDistanceFunctions(GhostDistance.Middle);
        } 
        else
            PerformDistanceFunctions(GhostDistance.Far);
    }

    //Plays a set of functions like audio noises based on the ghost's distance from the player
    private void PerformDistanceFunctions(GhostDistance newDistanceState)
    {
        //Depending on the ghost proximity to the player
        //Play the heart beat sound and ghos noises
        switch (newDistanceState) {
            case GhostDistance.Closeby:
                PlayGhostNoise();
                audioManager.PlayHeartBeat("rapidbeat");
                break;
            case GhostDistance.Near:
                PlayGhostNoise();
                audioManager.PlayHeartBeat("fastbeat");
                break;
            case GhostDistance.Middle:
                audioManager.Stop("ghostnoise");
                audioManager.PlayHeartBeat("slowbeat");
                break;
            case GhostDistance.Far:
                StopGhostNoises();
                break;
            default:
                Debug.LogWarning("Warning: Player distance state could not be detected!");
                break;
        }

        ghostDistance = newDistanceState;
    }

    //Plays the spooky ghost approach noise
    private void PlayGhostNoise()
    {
        if (!audioManager.IsPlaying("ghostnoise"))
        {
            audioManager.Play("ghostnoise");
        }

        //A geometric progression is used to adjust the ghost approach volume
        //This progression works but it uses the hardcoded numbers 2 and 20
        float geo = Mathf.Pow(2, (float)GhostDistance.Near - distanceDifference) / 20;
        audioManager.AdjustGhostVolume("ghostnoise", geo);
    }

    //Stop all ghost noises and heartbeat
    private void StopGhostNoises()
    {
        audioManager.Stop("ghostnoise");
        audioManager.PlayHeartBeat("stop");
    }
}
