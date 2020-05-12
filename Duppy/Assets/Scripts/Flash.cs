using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flash : MonoBehaviour
{
    public Light2D light;
    public PolygonCollider2D collider;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Dissolve ghost = collision.gameObject.GetComponent<Dissolve>();
            ghost.Death();
            GhostProximity proximity = collision.gameObject.GetComponent<GhostProximity>();
            proximity.isGhostDead = true;
        }
    }


}
