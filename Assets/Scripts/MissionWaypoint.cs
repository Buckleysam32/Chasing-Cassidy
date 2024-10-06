using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Waypoint
{
    public Image img;       // The UI image for the waypoint
    public Transform target; // The target that the waypoint is tracking
}

public class MissionWaypoint : MonoBehaviour
{
    public Waypoint waypoint1;
    public Waypoint waypoint2;
    public Waypoint waypoint3;
    public Waypoint waypoint4;
    public Waypoint waypoint5;

    public float smoothSpeed = 20f;  // Adjust this to control smoothness

    private Vector2 waypointPosition1;
    private Vector2 waypointPosition2;
    private Vector2 waypointPosition3;
    private Vector2 waypointPosition4;
    private Vector2 waypointPosition5;

    void LateUpdate()
    {
        UpdateWaypoint(waypoint1, ref waypointPosition1);

        UpdateWaypoint(waypoint2, ref waypointPosition2);

        UpdateWaypoint(waypoint3, ref waypointPosition3);

        UpdateWaypoint(waypoint4, ref waypointPosition4);

        UpdateWaypoint(waypoint5, ref waypointPosition5);

    }

    void UpdateWaypoint(Waypoint waypoint, ref Vector2 waypointPosition)
    {
        float minX = waypoint.img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = waypoint.img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(waypoint.target.position);

        // Check if the target is behind the camera
        if (Vector3.Dot((waypoint.target.position - Camera.main.transform.position), Camera.main.transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        // Clamp position to stay on screen
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Smooth the waypoint movement to reduce jittering
        waypointPosition = Vector2.Lerp(waypointPosition, pos, Time.deltaTime * smoothSpeed);

        // Apply the calculated position to the image
        waypoint.img.transform.position = waypointPosition;
    }
}

