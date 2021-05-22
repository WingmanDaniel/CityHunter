using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem dirtSplatter;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    private float jumpStrength = 7.0f;
    private float doubleJumpStrength = 3.0f;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private float doulePressRate = 0.3f;
    private float jumpStartTime = 0;
    private float jumpSpeedAni = 0.8f;
    private float doubleJumPSpeedAni = 0.5f;
    private float runningSpeedAni = 1.0f;


    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        dirtSplatter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.GameInProgress())
        {
            JumpControl();
            SpeedingControl();
        }

    }

    // jumnp control by space key input
    private void JumpControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumping && !isDoubleJumping)
            {
                Debug.Log(Time.time - jumpStartTime);
                if (Time.time - jumpStartTime <= doulePressRate)
                {
                    Debug.Log("Double Space Input");
                    playerRb.AddForce(Vector3.up * doubleJumpStrength, ForceMode.Impulse);
                    playerAnimator.speed = doubleJumPSpeedAni;
                    isDoubleJumping = true;
                }
            }
            else if (!isJumping)
            {
                Debug.Log("Space Input");
                playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                isJumping = true;
                jumpStartTime = Time.time;
                //Stop the dirt splatter particle;
                dirtSplatter.Stop();
                //update animation from running to jumping
                playerAnimator.SetTrigger("Jump_trig");
                playerAnimator.speed = jumpSpeedAni;

            }

        }
    }


    private void SpeedingControl()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (SpeedingAvaliableCheck())
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

    private bool SpeedingAvaliableCheck()
    {
        return !isJumping && !gameManagerScript.isDashMode;
    }

    private float GetRunningSpeed()
    {
        return (gameManagerScript.isDashMode ? runningSpeedAni * gameManagerScript.dashSpeedRate: runningSpeedAni);
    }

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
                GameOver(1);
            else
                GameOver(2);
        }

    }

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

    private void GameOver(int type)
    {
        gameManagerScript.GameOverBehavior();
        dirtSplatter.Stop();
        playerAnimator.SetBool("Death_b", true);
        playerAnimator.SetInteger("DeathType_int", type);
        Debug.Log("Game Over");
    }


}
