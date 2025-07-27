using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    bool hasPackage = false;
    int packageHolding = 0;
    [SerializeField] Color32 hasPackageColor = new Color32(0, 255, 0, 255); // Green color for has package
    [SerializeField] Color32 noPackageColor = new Color32(255, 0, 0, 255); // Red color for no package
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on Delivery object.");
        }
        spriteRenderer.color = noPackageColor; // Set initial color to no package
    }
    void Update()
    {
        // Update the color based on whether the player has a package
        if (hasPackage)
        {
            spriteRenderer.color = hasPackageColor;
        }
        else
        {
            spriteRenderer.color = noPackageColor;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Customer":
                if (hasPackage)
                {
                    packageHolding--;
                    if (packageHolding == 0)
                    {
                        hasPackage = false;
                    }
                    Debug.Log("Delivered the package! Remaining packages: " + packageHolding);
                    Destroy(collider.gameObject); // Remove the customer from the scene
                }
                else
                {
                    Debug.Log("No package to deliver! Go back and pick one up.");
                    // Add logic for when trying to deliver without a package
                }
                break;

            case "Package":
                hasPackage = true;
                packageHolding++;
                Debug.Log("Picked up a package! Total packages: " + packageHolding);
                Destroy(collider.gameObject); // Remove the package from the scene
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("Collision with obstacle!");
        }
    }
}
