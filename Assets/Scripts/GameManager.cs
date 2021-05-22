using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool isGameStart = false;
    public bool isGameOver = false;
    public bool isDashMode = false;
    public float dashSpeedRate = 2.0f;

    private PlayerController playerControllerScript;

    private int gameScore;
    private int scoreRate = 2;
    private float startTime;

    private float dashStartTime = 0;
    private float dashTime = 0;
    private TextMeshProUGUI playerScore;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        isGameStart = true;
        isGameOver = false;
        startTime = Time.time;
        playerScore = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameInProgress())
            GameScoreMechanic();
    }


    private void GameScoreMechanic()
    {
        if (isDashMode && dashStartTime == 0)
        {
            dashStartTime = Time.time;
        }

        if(isDashMode)
            gameScore = Mathf.FloorToInt(((Time.time - dashStartTime) * dashSpeedRate + (dashTime + dashStartTime) - startTime) / scoreRate);
        else
            gameScore = Mathf.FloorToInt((dashTime + (Time.time - startTime)) / scoreRate);
        if (!isDashMode && dashStartTime != 0)
        {
            dashTime += Time.time - dashStartTime;
            dashStartTime = 0;
        }
        playerScore.SetText("Score: " + gameScore);
    }
    public bool GameInProgress()
    {
        return isGameStart && !isGameOver;
    }
}
