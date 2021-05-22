using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isGameStart = false;
    private bool isGameOver = false;
    public bool isDashMode = false;
    public int gameLevel = 1;
    private int levelRate = 20;    // the game level will be increased every 20s
    private bool isDashStart = false;
    public float dashSpeedRate = 2.0f;
    public Transform startTrans;

    private PlayerController playerControllerScript;

    private int gameScore = 0;
    private float scoreRate = 0.5f; //0.5 second means 1 point
    private float startTime;
    private float lerpSpeed = 5.0f;
    private float dashStartTime = 0.0f;
    private float dashTime = 0.0f;
    private TextMeshProUGUI playerScore;
    private TextMeshProUGUI gameInfor;
    private Button restart;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        isGameStart = false;
        isGameOver = true;
        playerScore = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
        gameInfor = GameObject.Find("GameInfor").GetComponent<TextMeshProUGUI>();
        restart = GameObject.Find("Restart").GetComponent<Button>();
        restart.gameObject.SetActive(false);
        restart.onClick.AddListener(RestartGame);
        StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInProgress())
        {
            GameScoreMechanic();
            DifficultyIncearing();
        }
    }

    private void DifficultyIncearing()
    {
        int potentialLevel = Mathf.FloorToInt(Time.time - startTime) / levelRate + 1;
        if (potentialLevel != gameLevel)
        {
            gameLevel = potentialLevel;
            gameInfor.text = "ROUND " + gameLevel;
            StartCoroutine(UpdateGameInfor());
        }
    }

    //The player will walk in the game then run
    private IEnumerator PlayIntro()
    {
        //Game infomation text show "Ready"
        gameInfor.text = "READY";
        Vector3 startPos = playerControllerScript.transform.position;
        Vector3 endPos = startTrans.transform.position;

        float journeyLength = Vector3.Distance(startPos, endPos);

        float tempStartTime = Time.time;

        float distanceCovered = (Time.time - tempStartTime) * lerpSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 0.3f);
        playerControllerScript.GetComponent<Animator>().SetFloat("Multiplier", 0.3f);
        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - tempStartTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            yield return null;
        }

        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 1.0f);
        playerControllerScript.GetComponent<Animator>().SetFloat("Multiplier", 1.0f);
        isGameStart = true;
        isGameOver = false;
        startTime = Time.time;
        dashStartTime = Time.time;
        gameInfor.text = "GO ROUND " + gameLevel;

        StartCoroutine(UpdateGameInfor());
    }


    private IEnumerator UpdateGameInfor()
    {
        //remove the "GO" information after 2 seconds of the game starting
        yield return new WaitForSeconds(2);
        gameInfor.text = "";
    }
    private void GameScoreMechanic()
    {
        if (isDashMode && !isDashStart)
        {
            dashStartTime = Time.time;
            isDashStart = true;
        }

        if (isDashMode)
            gameScore = Mathf.FloorToInt(((Time.time - dashStartTime) * dashSpeedRate + dashStartTime - startTime + dashTime) / scoreRate);
        else
            gameScore = Mathf.FloorToInt((dashTime + (Time.time - startTime)) / scoreRate);

        if (!isDashMode && isDashStart)
        {
            dashTime += Time.time - dashStartTime;
            dashStartTime = 0.0f;
            isDashStart = false;
        }
        playerScore.SetText("Score: " + gameScore);
    }
    public bool GameInProgress()
    {
        return isGameStart && !isGameOver;
    }

    public void GameOverBehavior()
    {
        isGameOver = true;
        gameInfor.text = "GAME OVER";
        restart.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
