using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{

    private float moveSpeed = 5.0f;
    private GameManager gameMangerScript;

    // Start is called before the first frame update
    // 1. Assigning values to variables
    void Start()
    {
        gameMangerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    // 1. call the left movement method
    void Update()
    {
        AutoMoveLeft();
    }

    //execute the left movement of the object
    private void AutoMoveLeft()
    {
        if (MoveAvaliableCheck())
        {
            float speed = gameMangerScript.isDashMode ? moveSpeed * gameMangerScript.dashSpeedRate : moveSpeed;
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
    }

    //check the statement of requirements whether the movement should be executed
    private bool MoveAvaliableCheck()
    {
        return gameMangerScript.GameInProgress();
    }
}
