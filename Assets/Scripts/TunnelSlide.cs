using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSlide : MonoBehaviour
{
    public GameObject player;
    public GameObject transporter;
    private Rigidbody2D rb2d;
    
    private Vector2 previousColliderSize;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
        previousColliderSize = player.GetComponent<CapsuleCollider2D>().size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Slide());
         
    }

    IEnumerator Slide()
    {

        player.GetComponent<SonicController>().isAnimated = true;
        player.GetComponent<SonicController>().currSpeed = 0;

        rb2d.isKinematic = true;

        player.transform.parent = transporter.transform;
        player.transform.localPosition = new Vector3(0, 0, 5);

        transporter.GetComponent<Animator>().enabled = true;

        player.GetComponent<Animator>().ResetTrigger("isRunning");
        player.GetComponent<Animator>().ResetTrigger("isWalking");
        player.GetComponent<Animator>().SetTrigger("isBall");

        
        // rb2d.freezeRotation = false;
        // rb2d.AddForce(new Vector2(200, 0), ForceMode2D.Impulse);
        //yield return new WaitForSeconds(1.8f);
        ////Time.timeScale = 0;
        //rb2d.AddForce(new Vector2(-100, -100), ForceMode2D.Impulse);
        //rb2d.gravityScale = 20;

        yield return new WaitForSeconds(1.1f);

        player.GetComponent<SonicController>().isAnimated = false;
        rb2d.AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
        rb2d.isKinematic = false;
        // player.transform.position = transporter.transform.position;
        player.transform.parent = null;
        player.GetComponent<Animator>().SetTrigger("isRunning");
        // player.GetComponent<SonicController>().currSpeed = 0.5f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
