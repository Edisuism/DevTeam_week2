using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum doorType { door1, door2, door3 }

public class Doors : MonoBehaviour
{
    public GameObject player;
    public doorType doorType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckDoor(doorType);
        }
    }

    private void CheckDoor(doorType door)
    {
        switch (door)
        {
            case doorType.door1:
                if (player.GetComponent<PlayerController>().key1)
                {
                    Destroy(gameObject);
                }
                break;
            case doorType.door2:
                if (player.GetComponent<PlayerController>().key2)
                {
                    Destroy(gameObject);
                }
                break;
            case doorType.door3:
                if (player.GetComponent<PlayerController>().key3)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
}
