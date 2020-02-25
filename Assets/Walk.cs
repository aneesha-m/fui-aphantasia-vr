using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    public GameObject player;
    private float deadZoneAmt = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 touchCoords = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (touchCoords.x < -deadZoneAmt)
        {
            // touching left side, strafe left
            player.transform.Translate(-Vector3.right * touchCoords.x * Time.deltaTime);
        }
        else if (touchCoords.x > deadZoneAmt)
        {
            // touching right side, strafe right
            player.transform.Translate(Vector3.right * touchCoords.x * Time.deltaTime);
        }

        if (touchCoords.y < -deadZoneAmt)
        {
            // touching bottom side, move backwards
            player.transform.Translate(-Vector3.forward * touchCoords.y * Time.deltaTime);
        }
        else if (touchCoords.y > deadZoneAmt)
        {
            // touching top side, move forward
            player.transform.Translate(Vector3.forward * touchCoords.y * Time.deltaTime);
        }
    }
}
