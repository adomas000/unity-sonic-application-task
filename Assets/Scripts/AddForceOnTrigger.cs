using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnTrigger : MonoBehaviour
{
    public GameObject player;
    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
            player.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
            player.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
            player.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        }
        
    }
}
