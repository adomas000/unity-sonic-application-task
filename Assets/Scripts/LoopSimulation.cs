using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSimulation : MonoBehaviour
{
    public GameObject anchor;
    public GameObject player;
    public GameObject loopCollider;
    public GameObject groundedCheck;

    private Rigidbody2D rb2d;
    private SonicController sc;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
        sc = player.GetComponent<SonicController>();
        anchor.GetComponent<Animator>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && sc.currSpeed > (sc.maxSpeed * 0.75))
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log("Loop triggered");
            StartCoroutine(Spin());
        }
    }

    IEnumerator Spin()
    {
        rb2d.isKinematic = true;
        //rb2d.velocity = new Vector2(0, 0);
        SonicController sc = player.GetComponent<SonicController>();
        //
        sc.currSpeed = 0;
        sc.isAnimated = true;
        //
        player.GetComponent<Animator>().ResetTrigger("isRunning");
        player.GetComponent<Animator>().SetTrigger("isRolling");
        //
        player.transform.parent = anchor.transform;
        //
        anchor.GetComponent<Animator>().enabled = true;
        loopCollider.GetComponent<PolygonCollider2D>().isTrigger = true;
        // Hide behind loop sprite
        player.GetComponent<SpriteRenderer>().sortingOrder = -1;
        //
        yield return new WaitForSeconds(1);
        // After animation
        player.GetComponent<Animator>().SetTrigger("isRunning");
        player.transform.parent = null;
        anchor.GetComponent<Animator>().enabled = false;
        sc.isAnimated = false;
        sc.currSpeed = 0.5f;

        yield return new WaitForSeconds(0.35f);
        player.GetComponent<SpriteRenderer>().sortingOrder = 10;
        loopCollider.GetComponent<PolygonCollider2D>().isTrigger = false;
        rb2d.isKinematic = false;
        yield return new WaitForSeconds(0.5f);
        // Set grounded to allow jumping after animation
        groundedCheck.GetComponent<GroundedCheck>().isGrounded = true;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        //sc.isAnimated = false;
        //sc.currSpeed = 0;


    }

}
