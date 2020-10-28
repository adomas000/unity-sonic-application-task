using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    public GameObject sonic;
    public GameObject[] jumpThroughPlatforms;

    public bool areCollidersTurnedOff = false;

    private Rigidbody2D rb;
    private List<BoxCollider2D> platformColliders = new List<BoxCollider2D>();

    
    // Start is called before the first frame update
    void Start()
    {
        rb = sonic.GetComponent<Rigidbody2D>();
        Debug.Log(jumpThroughPlatforms.Length);
        for (int i = 0; i < jumpThroughPlatforms.Length; i++)
        {
            platformColliders.Add(jumpThroughPlatforms[i].GetComponent<BoxCollider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y > 0)
        {
            foreach (BoxCollider2D coll in platformColliders)
            {
                coll.isTrigger = true;
            }

            areCollidersTurnedOff = true;
        }
        else if (areCollidersTurnedOff)
        {
            foreach (BoxCollider2D coll in platformColliders)
            {
                coll.isTrigger = false;
            }

            areCollidersTurnedOff = false;
        }
    }
}
