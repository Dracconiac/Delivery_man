using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject pointOfFocus; // Reference to the player object

    //Camera follows the player object
    void LateUpdate()
    {
        transform.position = pointOfFocus.transform.position + new Vector3(0, 0, -10); // Adjust the z position to keep the camera at a distance
    }
}
