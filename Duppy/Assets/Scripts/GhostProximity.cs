using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProximity : MonoBehaviour
{
    //Set in Inspector
    public float ghostSpeed;
    public float farDistance, middleDistance, nearDistance, closebyDistance, onTargetDistance;

    public struct GhostDistance { 
        public float Far;
        public float Middle;
        public float Near;
        public float Closeby;
        public float OnTarget;
    }
    public GhostDistance ghostDistance;

    //currentDistanceState is used to check the distance state (i.e. closeby, far)
    //For the exact distance number, use distanceDifference
    private float currentDistanceState;
    public bool isGhostDead = false;

    private GameObject targetObject;
    //Finds the exact difference between the player and ghost
    private float distanceDifference;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        ghostDistance.Far = farDistance;
        ghostDistance.Middle = middleDistance;
        ghostDistance.Near = nearDistance;
        ghostDistance.Closeby = closebyDistance;
        ghostDistance.OnTarget = onTargetDistance;

        audioManager = FindObjectOfType<AudioManager>();
        targetObject = GameObject.FindGameObjectWithTag("Player");
        currentDistanceState = ghostDistance.Far;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDistanceState != ghostDistance.OnTarget && !isGhostDead)
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
        if (collision.gameObject == targetObject && currentDistanceState != ghostDistance.OnTarget)
        {
            //TODO: CALL PLAYER CONTROLLER AND FREEZE THE PLAYER
            currentDistanceState = ghostDistance.OnTarget;
            StopGhostNoises();
            audioManager.Play("playercaught");
        }
    }

    private void CheckDistance()
    {
        distanceDifference = Vector3.Distance(targetObject.transform.position, this.gameObject.transform.position);

        if (distanceDifference < (float)ghostDistance.Closeby)
            PerformDistanceFunctions("Closeby");
        else if (distanceDifference < (float)ghostDistance.Near)
            PerformDistanceFunctions("Near");
        else if (distanceDifference < (float)ghostDistance.Middle)
        {
            //TODO: CALL GHOST AI TO START CHASING THE PLAYER
            PerformDistanceFunctions("Middle");
        } 
        else
            PerformDistanceFunctions("Far");
    }

    //Plays a set of functions like audio noises based on the ghost's distance from the player
    private void PerformDistanceFunctions(string newDistanceState)
    {
        //Depending on the ghost proximity to the player
        //Play the heart beat sound and ghos noises
        switch (newDistanceState) {
            case "Closeby":
                PlayGhostNoise();
                transform.position = Vector2.MoveTowards(transform.position, targetObject.transform.position, ghostSpeed);
                audioManager.PlayHeartBeat("rapidbeat");
                currentDistanceState = ghostDistance.Closeby;
                break;
            case "Near":
                PlayGhostNoise();
                transform.position = Vector2.MoveTowards(transform.position, targetObject.transform.position, ghostSpeed);
                audioManager.PlayHeartBeat("fastbeat");
                currentDistanceState = ghostDistance.Near;
                break;
            case "Middle":
                audioManager.Stop("ghostnoise");
                audioManager.PlayHeartBeat("slowbeat");
                currentDistanceState = ghostDistance.Middle;
                break;
            case "Far":
                StopGhostNoises();
                currentDistanceState = ghostDistance.Far;
                break;
            default:
                Debug.LogWarning("Warning: Player distance state could not be detected!");
                break;
        }

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
        float geo = Mathf.Pow(2, (float)ghostDistance.Near - distanceDifference) / 20;
        audioManager.AdjustGhostVolume("ghostnoise", geo);
    }

    //Stop all ghost noises and heartbeat
    private void StopGhostNoises()
    {
        audioManager.Stop("ghostnoise");
        audioManager.PlayHeartBeat("stop");
    }
}
