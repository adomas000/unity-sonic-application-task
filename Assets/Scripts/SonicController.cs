using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SonicController : MonoBehaviour
{
    public int lives = 3;
   
    public float speed = 10f;
    public float currSpeed = 0f;
    public float currVelocity = 0f;
    public float acceleration = 0.01f;
    public float deacceleration = 0.1f;
    public float maxSpeed = 20f;
    public float JumpHeight = 10f;
    public bool isJumping = false;
    public bool isAnimated = false;

    public int score = 0;

    public float inputAxis = 0;

    public GameObject gameStartSpawn;
    public GameObject livesCounter;
    public GameObject scoreValueObj;
    public GameObject mainCam;

    private Text livesCounterText;
    private Text scoreText;

    private Vector2 previousPosition = new Vector2(0,0);


    // Ground checks
    public bool isGrounded = true;
    public GameObject groundedCheck;
    private GroundedCheck gc;

    // Animation
    public bool flipSprite = false; 
    private SpriteRenderer sr;
    private Animator anim;

    // Testing
    public bool isKinematic = false;


    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gc = groundedCheck.GetComponent<GroundedCheck>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        livesCounterText = livesCounter.GetComponent<Text>();
        scoreText = scoreValueObj.GetComponent<Text>();

        transform.position = new Vector2(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        foreach(ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.tag == "Enemy")
            {
                if (transform.position.x < contact.collider.transform.position.x)
                {
                    Debug.Log("Died from the left");
                    if (!Convert.ToBoolean(--lives))
                    {
                        mainCam.transform.position = new Vector3(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y, -10);
                        transform.position = new Vector2(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y);
                        lives = 3;
                    }
                    currSpeed = 0;
                    rb2d.AddForce(new Vector2(-300, 20), ForceMode2D.Impulse);
                }
                else
                {
                    if (!Convert.ToBoolean(--lives))
                    {
                        mainCam.transform.position = new Vector3(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y, -10);
                        transform.position = new Vector2(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y);
                        lives = 3;
                    }
                    Debug.Log("Died from the right");
                    currSpeed = 0;
                    rb2d.AddForce(new Vector2(300, 28), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    void FixedUpdate()
    {
        Vector2 NoMovement = new Vector2(0f, 0f);

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        isGrounded = gc.isGrounded;
        // Max speed limit
        if (rb2d.velocity.x > 0 && rb2d.velocity.x > maxSpeed) rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        else if (rb2d.velocity.x < 0 && rb2d.velocity.x < -maxSpeed) rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);

        livesCounterText.text = lives.ToString();
        scoreText.text = score.ToString();

        //currVelocity = transform.position.x - previousPosition.x;
        //previousPosition = transform.position;


        if (inputAxis > 0 || isAnimated)
        {
            flipSprite = false;
            // Stop player if we are moving to another direction
            if (previousPosition.x > transform.position.x) currSpeed = 0;
            //if (!wasMovingRight) currSpeed = 0;
            //if (rb2d.velocity.x < maxSpeed)


            //rb2d.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
            //rb2d.velocity = new Vector2(rb2d.velocity.x + speed, rb2d.velocity.y);
            previousPosition = transform.position;
            if (currSpeed < maxSpeed)
                currSpeed += acceleration;
            transform.Translate(Vector3.right * currSpeed);
            
            
        }
        else if (inputAxis < 0)
        {
            flipSprite = true;
            // Stop player if we are moving to another direction
            if (previousPosition.x < transform.position.x) currSpeed = 0;
            //if (wasMovingRight) currSpeed = 0;
            //if (rb2d.velocity.x > -maxSpeed)


            //rb2d.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse);
            //rb2d.velocity = new Vector2(rb2d.velocity.x - speed, rb2d.velocity.y);
            previousPosition = transform.position;
            if (currSpeed < maxSpeed)
                currSpeed += acceleration;
            transform.Translate(Vector3.left * currSpeed);
                
            
            //rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
        } else
        {
            // Was going right
            if (previousPosition.x < transform.position.x && currSpeed > 0)
            {
                // anim.SetTrigger("isIdle");
                currSpeed -= deacceleration;
                transform.Translate(Vector3.right * currSpeed);
            } else if (previousPosition.x > transform.position.x && currSpeed > 0) // left
            {
                // anim.SetTrigger("isIdle");
                currSpeed -= deacceleration;
                transform.Translate(Vector3.left * currSpeed);
            } else
            {
                currSpeed = 0;
            }
        }

        if (transform.position.y < -50)
        {
            mainCam.transform.position = new Vector3(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y, -10);
            transform.position = new Vector2(gameStartSpawn.transform.position.x, gameStartSpawn.transform.position.y);
            lives = 3;
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            HandleUpButton();
        }
        
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetKey(KeyCode.A))
            {
                inputAxis = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inputAxis = 1;
            }
            else
            {
                inputAxis = 0;
            }
        } 
       
        // Animation
        //var tmpCurrVelocity = Mathf.Abs(rb2d.velocity.x);
        var tmpCurrVelocity = currSpeed;
        if (isAnimated)
        {
            // nothing
        }
        else if (tmpCurrVelocity == 0)
        {
            sr.flipY = false;
            anim.SetTrigger("isIdle");
        }
        else if (tmpCurrVelocity < maxSpeed / 2)
        {
            anim.ResetTrigger("isIdle");
            anim.ResetTrigger("isRunning");
            anim.SetTrigger("isWalking");
        }
        else if (tmpCurrVelocity < maxSpeed)
        {
            anim.ResetTrigger("isWalking");
            anim.ResetTrigger("isRolling");
            anim.SetTrigger("isRunning");
        }
        //else if (tmpCurrVelocity < (maxSpeed * 0.75))
        //{
        //    anim.ResetTrigger("isRunning");
        //    anim.ResetTrigger("isBall");
        //    anim.SetTrigger("isRolling");
        //}
        //else if (tmpCurrVelocity <= maxSpeed)
        //{
        //    anim.ResetTrigger("isRolling");
        //    anim.SetTrigger("isBall");
        //}
    }

    private void LateUpdate()
    {
        //if (rb2d.velocity.x > 0)
        //{
        //    sr.flipX = false;
        //}
        //else if (rb2d.velocity.x < 0)
        //{
        //    sr.flipX = true;
        //}
        sr.flipX = flipSprite;
    }

    public void HandleUpButton()
    {
        if (isGrounded && rb2d.velocity.y <= 0)
        {
            anim.SetTrigger("isRolling");
            rb2d.AddForce(new Vector2(0, JumpHeight), ForceMode2D.Impulse);
        }
    }

}
