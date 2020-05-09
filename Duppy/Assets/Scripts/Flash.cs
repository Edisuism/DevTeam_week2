using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flash : MonoBehaviour
{
    public Light2D light;

    private void Start()
    {
        StartCoroutine(FadeTo(0.0f, 0.3f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float intensity = light.intensity;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float newIntensity = Mathf.Lerp(intensity, aValue, t);
            light.intensity = newIntensity;
            yield return null;
        }
        Destroy(gameObject);
    }


        /*
        void Start()
        {
            material = transform.GetComponent<SpriteRenderer>().material;
            StartCoroutine(FadeTo(0.0f, 1.0f));
        }

        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = material.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                material.color = newColor;
                yield return null;
            }
        }
        */


    }
