using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;
    bool isDissolving = false;
    float fade = 1f;
    float speed = 2f;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void Death()
    {
        isDissolving = true;
    }

    void Update()
    {
        if (isDissolving)
        {
            fade -= Time.deltaTime * speed;
            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
                Destroy(gameObject);
            }

            material.SetFloat("_Fade", fade);
        }
    }
}
