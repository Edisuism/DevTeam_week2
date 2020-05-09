using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAudio : MonoBehaviour
{

    public enum GhostDistance { Far = 10, Near = Far/2, Closeby = Far/3, OnTarget = 0}
    public GhostDistance ghostDistance;
    private GameObject targetObject;
    private float distanceDifference;

    private AudioManager audioManager;
    private PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        targetObject = GameObject.FindGameObjectWithTag("Player");
        playerAudio = targetObject.GetComponent<PlayerAudio>();
        ghostDistance = GhostDistance.Far;
    }

    // Update is called once per frame
    void Update()
    {
        if(ghostDistance != GhostDistance.OnTarget)
        {
            distanceDifference = Vector3.Distance(targetObject.transform.position, this.gameObject.transform.position);

            if (distanceDifference < (float)GhostDistance.Closeby)
            {
                ghostDistance = GhostDistance.Closeby;
                PlayGhostNoise();
            }
            else if (distanceDifference < (float)GhostDistance.Near)
            {
                ghostDistance = GhostDistance.Near;
                PlayGhostNoise();
            }
            else
            {
                ghostDistance = GhostDistance.Far;
                audioManager.Stop("ghostnoise");
            }

            playerAudio.SendDistance(distanceDifference, (float)GhostDistance.Far);
        }
    }

    private void PlayGhostNoise()
    {
        if (!audioManager.IsPlaying("ghostnoise"))
        {
            audioManager.Play("ghostnoise");
        }
        audioManager.AdjustGhostVolume("ghostnoise", ((float)GhostDistance.Near - distanceDifference) / (float)GhostDistance.Near);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == targetObject && ghostDistance != GhostDistance.OnTarget)
        {
            ghostDistance = GhostDistance.OnTarget;
            audioManager.Stop("ghostnoise");
            audioManager.Play("playercaught");
        }
    }
}
