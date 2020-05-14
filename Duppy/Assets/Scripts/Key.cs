using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KeyType { key1, key2, key3 }

public class Key : MonoBehaviour
{
    public GameObject player;
    public Image image;
    public KeyType keyType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KeyGet(keyType);
        }
    }

    private void KeyGet(KeyType key)
    {
        switch (key)
        {
            case KeyType.key1:
                player.GetComponent<PlayerController>().key1 = true;
                image.enabled = true;
                Destroy(gameObject);
                break;
            case KeyType.key2:
                player.GetComponent<PlayerController>().key2 = true;
                image.enabled = true;
                Destroy(gameObject);
                break;
            case KeyType.key3:
                player.GetComponent<PlayerController>().key3 = true;
                image.enabled = true;
                Destroy(gameObject);
                break;
        }
    }
}
