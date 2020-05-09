using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Vector3 origin;
    private float timeRemaining;
    private float shakePower;
    private float shakeFadeTime;

    private void Start()
    {
        origin = GetComponent<Transform>().position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartShake(0.5f, 1f);
        }
    }

    private void LateUpdate()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            float xAmount = Random.Range(-0.2f, 0.2f) * shakePower;
            float yAmount = Random.Range(-0.2f, 0.2f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
        else
        {
            transform.position = origin;
        }
    }

    public void StartShake(float length, float power)
    {
        timeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;
    }
}
