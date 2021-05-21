using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem dirtSplatter;

    private Rigidbody playerRb;
    private Animator playerAnimator;
    private float jumpStrength  = 7.0f;
    private float doubleJumpStrength = 3.0f;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private float doulePressRate = 0.3f;
    private float jumpStartTime = 0;
    private float jumpSpeedAni = 0.8f;
    private float doubleJumPSpeedAni = 0.5f;
    private float runningSpeedAni = 1.0f;
    private bool isDashMode = false;
    private float dashSpeedRate = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        dirtSplatter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        JumpControl();
        SpeedingControl();
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
            else if(!isJumping)
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
                isDashMode = true;
                playerAnimator.speed = GetRunningSpeed();
            }
        }
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            isDashMode = false;
            playerAnimator.speed = GetRunningSpeed();
        }
    }

    private bool SpeedingAvaliableCheck()
    {
        return !isJumping && !isDashMode;
    }

    private float GetRunningSpeed()
    {
        return  (isDashMode ? runningSpeedAni * dashSpeedRate : runningSpeedAni);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Street") && isJumping)
        {
            isJumping = false;
            isDoubleJumping = false;
            //repplay the dirt splatter particle;
            dirtSplatter.Play();
            playerAnimator.speed = GetRunningSpeed();
        }
    }


}
