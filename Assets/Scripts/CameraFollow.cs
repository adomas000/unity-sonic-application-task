using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private void Start()
    {
        // y valye hardcoded to avoid camera stutter when player gets into camera boundaries
        transform.position = new Vector3(0, 0, -10);
    }

    void Update()
    {
        if (player.position.x > 0f && player.position.x < 230f)
        {
            if (player.position.y < 0)
            {
                transform.position = new Vector3(player.position.x + offset.x, 0, offset.z); // Camera follows the player with specified offset position
            }
            else
            {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
            }
        } else if (player.position.x > 230f && player.position.x < 360f)
        {
            if (player.position.y < -9)
            {
                transform.position = new Vector3(player.position.x + offset.x, -9, offset.z);
            }
            else
            {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
            }
        }
    }
}
