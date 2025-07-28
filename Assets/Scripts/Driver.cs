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
    float reverseSpeedCoeff = 0.5f; // Coefficient to reduce speed when reversing
    float steerSpeed = 130f;
    float adjustedSteerSpeed;
    float easySteeringCoeff = 1f; // Steering coefficient for low speed
    float hardSteeringCoeff = 0.3f; // Steering coefficient for high speed
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
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * reverseSpeedCoeff, maxSpeed);

        // Makes steering harder at higher speeds
        adjustedSteerSpeed = steerSpeed * Mathf.Lerp(easySteeringCoeff, hardSteeringCoeff, Mathf.Abs(currentSpeed) / maxSpeed); // sets the steering speed from 100% to 30% based on current speed
        moveAmount = currentSpeed * Time.deltaTime;
        steerAmount = horizontalInput * adjustedSteerSpeed * Time.deltaTime;

        transform.Translate(Vector3.up * moveAmount);
        transform.Rotate(Vector3.forward * -steerAmount);
    }

    void StopCar()
    {
        currentSpeed = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            StopCar();
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Boost")
        {
            currentSpeed += 6f;
            Destroy(collider.gameObject);
        }
    }
}
