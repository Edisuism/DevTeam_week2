using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioManager audioManager;

    public List<GameObject> ghostList = new List<GameObject>();
    private GameObject closestGhost;
    private float closestGhostDistance;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private bool FindClosestGhost(GameObject newGhost)
    {
        closestGhost = null;
        closestGhostDistance = float.MaxValue;

        foreach (GameObject ghost in ghostList)
        {
            if(Vector2.Distance(this.gameObject.transform.position, ghost.transform.position) < closestGhostDistance)
            {
                closestGhost = ghost;
                closestGhostDistance = Vector2.Distance(this.gameObject.transform.position, ghost.transform.position);
            }
        }

        if(newGhost == closestGhost)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }

    public void AddToGhostList(GameObject ghost)
    {
        ghostList.Add(ghost);
    }

    public void RemoveFromGhostList(GameObject ghost)
    {
        ghostList.Remove(ghost);
    }

    //Stop all ghost noises and heartbeat
    public void StopGhostNoises(GameObject ghost, bool stopHeartbeat)
    {
        if(FindClosestGhost(ghost) || closestGhost == null)
        {
            audioManager.Stop("ghostnoise");
            if(stopHeartbeat)
                audioManager.PlayHeartBeat("stop");
        }
    }

    //Play series of caught noises
    public void PlayerCaught()
    {
        audioManager.Play("playercaught");
    }

    //Plays the spooky ghost approach noise
    public void PlayGhostNoise(GameObject ghost, float ghostNearDistance)
    {
        if(FindClosestGhost(ghost))
        {
            if (!audioManager.IsPlaying("ghostnoise"))
            {
                audioManager.Play("ghostnoise");
            }

            float distanceDifference = Vector2.Distance(this.transform.position, ghost.transform.position);

            //A geometric progression is used to adjust the ghost approach volume
            //This progression works but it uses the hardcoded numbers 2 and 20
            float geo = Mathf.Pow(2, ghostNearDistance - distanceDifference) / 20;
            audioManager.AdjustGhostVolume("ghostnoise", geo);
        }
    }

    public void Heartbeat(GameObject ghost, string beatSpeed)
    {
        if(FindClosestGhost(ghost))
        {
            audioManager.PlayHeartBeat(beatSpeed);
        }
    }

    public void GhostDeath()
    {
        audioManager.Play("ghostdeath");
    }

    public bool CheckGhostDeath()
    {
        return audioManager.IsPlaying("ghostdeath");
    }
}
