using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Slider slider;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    //public Animator animator;
    public GameObject flash;
    public GameObject flashInstantiate;
    private Vector2 movement;
    private float battery = 100f;
    private float batteryCap = 100f;
    private float fillSpeed = 1f;

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        //animator.SetFloat("Speed", movement.sqrMagnitude);

        if (slider.value < battery / batteryCap)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }

        if (slider.value > battery / batteryCap)
        {
            slider.value -= fillSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (battery >= 10)
            {
                if (movement.y > 0)
                {
                    CameraShot("North");
                }
                else if (movement.x > 0)
                {
                    CameraShot("East");
                }
                else if (movement.y < 0)
                {
                    CameraShot("South");
                }
                else if (movement.x < 0)
                {
                    CameraShot("West");
                }
            }
            else
            {
                //play no battery sound
            }
        }
    }

    private void CameraShot(string direction)
    {
        switch (direction)
        {
            case "North":
                flashInstantiate = Instantiate(flash, transform.position, Quaternion.Euler(0, 0, 180));
                break;
            case "East":
                flashInstantiate = Instantiate(flash, transform.position, Quaternion.Euler(0, 0, 90));
                break;
            case "South":
                flashInstantiate = Instantiate(flash, transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case "West":
                flashInstantiate = Instantiate(flash, transform.position, Quaternion.Euler(0, 0, 270));
                break;
        }
        battery -= 10;
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
