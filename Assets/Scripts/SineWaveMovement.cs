using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{

    /// <summary>The objects initial position.</summary>
    private Vector3 startPosition;
    /// <summary>The objects updated position for the next frame.</summary>
    private Vector3 newPosition;

    /// <summary>The speed at which the object moves.</summary>
    public float speed = 0.5f;
    /// <summary>The maximum distance the object may move in either y direction.</summary>
    public int maxDistance = 150;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(150, 0, -10);
        startPosition = transform.position;
        newPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newPosition.x = startPosition.x + (maxDistance * Mathf.Sin(Time.time * speed));
        newPosition.z = -10;
        transform.position = newPosition;

    }
}
