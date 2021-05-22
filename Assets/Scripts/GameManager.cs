using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float dashSpeedRate = 2.0f;
    public Transform startTrans;
    public bool isDashMode = false;
    public int gameLevel = 1;
    public AudioClip[] backgroundClips;

    private bool isGameStart = false;
    private bool isGameOver = false;
    private int levelRate = 20;    // the game level will be increased every 20s
    private bool isDashStart = false;
    private int totalLevels = 30;
    private PlayerController playerControllerScript;
    private int gameScore = 0;
    private float scoreRate = 0.5f; //0.5 second means 1 point
    private float startTime;
    private float lerpSpeed = 2.0f;
    private float dashStartTime = 0.0f;
    private float dashTime = 0.0f;
    private TextMeshProUGUI playerScore;
    private TextMeshProUGUI gameInfor;
    private Button restart;
    private AudioSource backgroundSource;

    // Start is called before the first frame update
    // 1. Assigning values to variables
    // 2. Setting the game at initialization mode
    // 3. Loading game
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerScore = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
        gameInfor = GameObject.Find("GameInfor").GetComponent<TextMeshProUGUI>();
        restart = GameObject.Find("Restart").GetComponent<Button>();
        isGameStart = false;
        isGameOver = true;
        backgroundSource = GetComponent<AudioSource>();
        restart.gameObject.SetActive(false);
        restart.onClick.AddListener(RestartGame);
        StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    // When the game is in progeress, the game manager will do 3 tasks per frame
    // 1. update the game score
    // 2. increase the game difficulty when the player meets the requirements
    // 3. randomely play next background music when a background music stop
    void Update()
    {
        if (GameInProgress())
        {
            GameScoreMechanic();
            DifficultyIncreasing();
            BackgroundSoundPlay();
        }
    }

    // executing the increasing of the game's difficulty
    private void DifficultyIncreasing()
    {
        int potentialLevel = Mathf.FloorToInt(Time.time - startTime) / levelRate + 1;
        if (potentialLevel == totalLevels)  // when the player reaches the win level, the game will execute the game win behavior.
        {
            GameWinBehavior();
        }
        // when the potential game level is more than the current game level, the game will show a new round information and execute it at "SpawnManager"
        else if (potentialLevel != gameLevel)
        {
            gameLevel = potentialLevel;
            gameInfor.text = "ROUND " + gameLevel;
            StartCoroutine(UpdateGameInfor());
        }
    }

    //Game loading process before offically start.
    private IEnumerator PlayIntro()
    {
        //Game infomation text show "Ready"
        gameInfor.text = "READY";
        //The hero will walk in the game from startPos to endPos
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
        //The hero arrives the offically startting position and the game starts
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 1.0f);
        playerControllerScript.GetComponent<Animator>().SetFloat("Multiplier", 1.0f);
        isGameStart = true;
        isGameOver = false;
        startTime = Time.time;
        dashStartTime = Time.time;
        gameInfor.text = "GO ROUND " + gameLevel;   //Game Information text show "GO"
        StartCoroutine(UpdateGameInfor());
    }

    //Game information text will automaticely clean it's information after 2 seconds.
    private IEnumerator UpdateGameInfor()
    {
        yield return new WaitForSeconds(2);
        gameInfor.text = "";
    }


    //executing game score calculation
    //1. calculate the game score based on time (dashSpeedRate times nromal during the dash mode)
    //2. show game score at the game score text
    private void GameScoreMechanic()
    {
        if (isDashMode && !isDashStart)
        {
            dashStartTime = Time.time;
            isDashStart = true;
        }
        // dividing scoreRate is used for conversing the total time to score
        // the dashTime is used for storing the total time that the player plays game in dash mode.
        if (isDashMode)
            gameScore = Mathf.FloorToInt(((Time.time - dashStartTime) * dashSpeedRate + dashStartTime - startTime + dashTime) / scoreRate);
        else
            gameScore = Mathf.FloorToInt((dashTime * (dashSpeedRate - 1) + (Time.time - startTime)) / scoreRate);

        if (!isDashMode && isDashStart)
        {
            dashTime += Time.time - dashStartTime;
            dashStartTime = 0.0f;
            isDashStart = false;
        }
        playerScore.SetText("Score: " + gameScore);
    }

    //return whether the game is in progress based on start and over flag.
    public bool GameInProgress()
    {
        return isGameStart && !isGameOver;
    }

    //executing game over
    //1. set game over flag
    //2. show game over informatoin 
    //3. stop the background music
    public void GameOverBehavior()
    {
        isGameOver = true;
        gameInfor.text = "GAME OVER";
        restart.gameObject.SetActive(true);
        backgroundSource.Stop();
    }

    //executing game restarting
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //executing randomly playing a game background music.
    private void BackgroundSoundPlay()
    {
        if (!backgroundSource.isPlaying)
        {
            int randomIndex = Random.Range(0, backgroundClips.Length);
            backgroundSource.clip = backgroundClips[randomIndex];
            backgroundSource.Play();
        }
    }

    //executing game win(minimum viable product)
    //1. set game over flag
    //2. show your win informatoin 
    //3. stop the background music
    private void GameWinBehavior()
    {
        isGameOver = true;
        gameInfor.text = "YOU ROCK";
        restart.gameObject.SetActive(true);
        backgroundSource.Stop();
    }
}
