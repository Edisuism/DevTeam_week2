using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProximity : MonoBehaviour
{
    //Set in Inspector
    public float ghostSpeed;

    public GhostDistance ghostDistance;

    //currentDistanceState is used to check the distance state (i.e. closeby, far)
    //For the exact distance number, use distanceDifference
    private float currentDistanceState;
    public bool isGhostDead = false;
    public bool chasingPlayer = false;
    //Used so that the ghost isn't added multiple times to the playerAudio list
    private bool enteredDetectionDistance = false;

    private GameObject targetObject;
    //Finds the exact difference between the player and ghost
    private float distanceDifference;

    private PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        ghostDistance = GetComponent<GhostDistance>();

        targetObject = GameObject.FindGameObjectWithTag("Player");
        playerAudio = targetObject.GetComponent<PlayerAudio>();
        currentDistanceState = ghostDistance.Far;
    }

    // Update is called once per frame
    void Update()
    {   
        if (currentDistanceState != ghostDistance.OnTarget && !isGhostDead){
            CheckDistance();
            if(chasingPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetObject.transform.position, ghostSpeed);
            }
        }
        else {
            playerAudio.StopGhostNoises(this.gameObject, true);
            if (isGhostDead && !playerAudio.CheckGhostDeath())
            {
                playerAudio.RemoveFromGhostList(this.gameObject);
                playerAudio.GhostDeath();
            }
        }
    }

    //If the ghost touches the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == targetObject && currentDistanceState != ghostDistance.OnTarget)
        {
            //TODO: CALL PLAYER CONTROLLER AND FREEZE THE PLAYER
            currentDistanceState = ghostDistance.OnTarget;
            playerAudio.StopGhostNoises(this.gameObject, true);
            playerAudio.PlayerCaught();
            
        }
    }

    private void CheckDistance()
    {
        distanceDifference = Vector2.Distance(targetObject.transform.position, this.gameObject.transform.position);

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
        //Play the heart beat sound and ghost noises
        switch (newDistanceState) {
            case "Closeby":
                playerAudio.PlayGhostNoise(this.gameObject, (float)ghostDistance.Near);
                chasingPlayer = true;
                playerAudio.Heartbeat(this.gameObject, "rapidbeat");
                currentDistanceState = ghostDistance.Closeby;
                break;
            case "Near":
                playerAudio.PlayGhostNoise(this.gameObject, (float)ghostDistance.Near);
                chasingPlayer = true;
                playerAudio.Heartbeat(this.gameObject, "fastbeat");
                currentDistanceState = ghostDistance.Near;
                break;
            case "Middle":
                if(!enteredDetectionDistance){
                    enteredDetectionDistance = true;
                    playerAudio.AddToGhostList(this.gameObject);
                }
                playerAudio.StopGhostNoises(this.gameObject, false);
                chasingPlayer = true;
                playerAudio.Heartbeat(this.gameObject, "slowbeat");
                currentDistanceState = ghostDistance.Middle;
                break;
            case "Far":
                if(enteredDetectionDistance){
                    enteredDetectionDistance = false;
                    playerAudio.RemoveFromGhostList(this.gameObject);
                }
                playerAudio.StopGhostNoises(this.gameObject, true);
                currentDistanceState = ghostDistance.Far;
                break;
            default:
                Debug.LogWarning("Warning: Player distance state could not be detected!");
                break;
        }

    }
    
}
