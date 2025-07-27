using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    float currentSpeed = 0f;
    float acceleration = 2f;
    float deceleration = 10f;
    float breaking = 20f;
    float maxSpeed = 80f;
    float steerSpeed = 130f;
    float adjustedSteerSpeed;
    float moveAmount;
    float steerAmount;

    void Start()
    {

    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isAccelerating = Input.GetKey(KeyCode.W);
        bool isBreaking = Input.GetKey(KeyCode.S);

        // Accelerate, decelerate, brake or reverse
        if (isAccelerating)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (isBreaking && currentSpeed > 0) // braking
        {
            currentSpeed -= breaking * Time.deltaTime;
        }
        else if (!isBreaking && currentSpeed > 0) // decelerating
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }
        else if (isBreaking && currentSpeed <= 0f) //reversing
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else if (!isAccelerating && currentSpeed < 0f) // decelerating in reverse
        {
            currentSpeed += breaking * Time.deltaTime;
        }

        // Clamp speed for both forward and reverse
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        // Makes steering harder at higher speeds
        adjustedSteerSpeed = steerSpeed * Mathf.Lerp(1f, 0.3f, Mathf.Abs(currentSpeed) / maxSpeed); // 0.3x steer at max speed
        moveAmount = currentSpeed * Time.deltaTime;
        steerAmount = horizontalInput * adjustedSteerSpeed * Time.deltaTime;

        transform.Translate(Vector3.up * moveAmount);
        transform.Rotate(0, 0, -steerAmount);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            currentSpeed = 0f; // Stop the car on collision
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Boost")
        {
            currentSpeed += 6f; // Boost speed when hitting a boost trigger
            Destroy(collider.gameObject); // Remove the boost object from the scene
        }
    }
}
