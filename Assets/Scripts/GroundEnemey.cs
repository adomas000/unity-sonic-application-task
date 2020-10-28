using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemey : MonoBehaviour
{
    public float speed = 2f;
    public float maxSpeed = 2;
    public Vector2 raycastOffset = new Vector2(0,0);
    // ->
    public bool moveRight = false;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            //transform.position = new Vector2(transform.position.x + speed, 0);
            if (rb.velocity.x < maxSpeed)
                rb.AddForce(new Vector2(speed, 0));
        } else
        {
            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
                rb.AddForce(new Vector2(-speed, 0));
            //transform.position = new Vector2(transform.position.x + speed, 0);
        }

        CheckForEdge();
    }

    void CheckForEdge()
    {
        RaycastHit2D hit;
        if (moveRight)
        {
            float angle = 65 * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Sin(angle), 0);
            Vector2 startPos = new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y);
            hit = Physics2D.Raycast(startPos, dir, 2);
        } else
        {
            float angle = 200 * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Sin(angle), 0);
            Vector2 startPos = new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y);
            hit = Physics2D.Raycast(startPos, dir, 2);
        }

        if (hit.collider == null || rb.velocity.x == 0)
        {
            moveRight = !moveRight;
            transform.GetComponent<SpriteRenderer>().flipX = !transform.GetComponent<SpriteRenderer>().flipX;

        }

    }
}
