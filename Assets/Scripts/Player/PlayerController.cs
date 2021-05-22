using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public AudioClip jumpClip;
    public AudioClip crashClip;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    private float jumpStrength = 14.0f;
    private float doubleJumpStrength = 6.0f;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private float doulePressRate = 0.3f;
    private float jumpStartTime = 0;
    private float jumpSpeedAni = 0.8f;
    private float doubleJumPSpeedAni = 0.5f;
    private float runningSpeedAni = 1.0f;
    private AudioSource playerAudio;
    private GameManager gameManagerScript;
    private ParticleSystem ExplosionSmoke;
    private ParticleSystem dirtSplatter;

    // Start is called before the first frame update
    // 1. Assigning values to variables
    // 2. Setting the game at initialization mode
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        ExplosionSmoke = GameObject.Find("FX_Explosion_Smoke").GetComponent<ParticleSystem>();
        dirtSplatter = GameObject.Find("DirtSplatter").GetComponent<ParticleSystem>();
        dirtSplatter.Play();

    }

    // Update is called once per frame
    // 1. monitor the jump control during the game in progresss
    // 2. monitor the dash control during the game in progress
    void Update()
    {
        if (gameManagerScript.GameInProgress())
        {
            JumpControl();
            dashControl();
        }

    }

    // executing jump if meeting the statements
    // 1. a space pressing will make the hero jump
    // 2. double pressing space key within doulePressRate(0.3) seconds will trigger a double jumping
    private void JumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumping && !isDoubleJumping)
            {
                //Debug.Log(Time.time - jumpStartTime);
                if (Time.time - jumpStartTime <= doulePressRate)
                {
                   // Debug.Log("Double Space Input");
                    playerRb.AddForce(Vector3.up * doubleJumpStrength, ForceMode.Impulse);
                    playerAnimator.speed = doubleJumPSpeedAni;
                    playerAudio.clip = jumpClip;
                    playerAudio.PlayDelayed(0.1f);
                    isDoubleJumping = true;
                }
            }
            else if (!isJumping)
            {
                //Debug.Log("Space Input");
                playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                isJumping = true;
                jumpStartTime = Time.time;
                //Stop the dirt splatter particle;
                dirtSplatter.Stop();
                //update animation from running to jumping
                playerAnimator.SetTrigger("Jump_trig");
                playerAnimator.speed = jumpSpeedAni;
                playerAudio.clip = jumpClip;
                playerAudio.Play();
            }

        }
    }

    // executing the dash behavior of the hero
    // 1. the hero will dash while the right arrow key is pressing
    // 2. the hero will stop the dash mode when the right arrow is released
    private void dashControl()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (DashAvaliableCheck())
            {
                gameManagerScript.isDashMode = true;
                playerAnimator.speed = GetRunningSpeed();
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            gameManagerScript.isDashMode = false;
            playerAnimator.speed = GetRunningSpeed();
        }
    }

    // check whether the hero can enter a dash mode
    // 1. not jumping
    // 2. not in a dash mode
    private bool DashAvaliableCheck()
    {
        return !isJumping && !gameManagerScript.isDashMode;
    }

    // calculate the runing speed
    // 1. normal mode: runningSpeedAni
    // 2. dash mode: runningSpeedAni * gameManagerScript.dashSpeedRate
    private float GetRunningSpeed()
    {
        return (gameManagerScript.isDashMode ? runningSpeedAni * gameManagerScript.dashSpeedRate: runningSpeedAni);
    }

    // detect the collision 
    // steet: recovering to runing mode after jump
    // obstacle: truggered game over behaviors
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Street") && isJumping)
        {
            RecoverAfterJump();
        }

        if (collision.gameObject.CompareTag("Obstacle") && gameManagerScript.GameInProgress())
        {
            // if the hero hitts the front of the obstacle, lay backword, otherwise, lay forward. 
            if (collision.gameObject.transform.position.z >= transform.position.z)
                GameOver(1);    //1 means backward
            else
                GameOver(2);    //2 means foreward
        }
    }


    // recovering to runing mode after jump
    private void RecoverAfterJump()
    {
        if (gameManagerScript.GameInProgress())
        {
            isJumping = false;
            isDoubleJumping = false;
            //replay the dirt splatter particle;
            dirtSplatter.Play();
            playerAnimator.speed = GetRunningSpeed();
        }

    }

    // execute game over behaviors
    // 1. the hero shows death animation based on the type parameter
    // 2. stop the dirt splatter particle
    // 3. play the crash sounds and crash particle
    private void GameOver(int type)
    {
        gameManagerScript.GameOverBehavior();
        dirtSplatter.Stop();
        playerAnimator.SetBool("Death_b", true);
        playerAnimator.SetInteger("DeathType_int", type);
        playerAudio.clip = crashClip;
        playerAudio.Play();
        ExplosionSmoke.Play();
    }


}
