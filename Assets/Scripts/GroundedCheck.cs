using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour
{
    
    public bool isGrounded = true;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ground-collider")
        {
            isGrounded = true;
        }
        if (collision.tag == "Enemy" && !isGrounded)
        {
            transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            transform.parent.GetComponent<SonicController>().score += 100;
            Destroy(collision.transform.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground-collider")
        {
            isGrounded = false;
        }
    }
}
