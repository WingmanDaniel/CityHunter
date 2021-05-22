using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{

    private float moveSpeed = 5.0f;
    private GameManager gameMangerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameMangerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        AutoMoveLeft();
    }

    private void AutoMoveLeft()
    {
        if(MoveAvaliableCheck())
        {
            float speed = gameMangerScript.isDashMode ? moveSpeed * gameMangerScript.dashSpeedRate : moveSpeed;
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
    }

    private bool MoveAvaliableCheck()
    {
        return gameMangerScript.GameInProgress();
    }
}
