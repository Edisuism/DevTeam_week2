using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractableTiles : MonoBehaviour
{
    private PlayerController playerController;
    
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.isCobwebbed = true;

            //Vector3Int removePos = tilemap.WorldToCell(collision.gameObject.transform.position);
            //tilemap.SetTile(removePos, null);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.isCobwebbed = false;
        }
    }
}
