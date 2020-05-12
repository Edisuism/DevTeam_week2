using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KeyType { key1, key2, key3 }

public class Key : MonoBehaviour
{
    public GameObject player;
    public GameObject panel;
    public KeyType keyType;
    public Image[] keyImage;

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
                Instantiate(keyImage[0], panel.transform);
                Destroy(gameObject);
                break;
            case KeyType.key2:
                player.GetComponent<PlayerController>().key2 = true;
                Instantiate(keyImage[1], panel.transform);
                Destroy(gameObject);
                break;
            case KeyType.key3:
                player.GetComponent<PlayerController>().key3 = true;
                Instantiate(keyImage[2], panel.transform);
                Destroy(gameObject);
                break;
        }
    }
}
