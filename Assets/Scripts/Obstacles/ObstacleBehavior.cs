using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    private float boundaryZLeft = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // 1. automatically destroy itself when its position is out of the bourdary
    void Update()
    {
        if(transform.position.z <= boundaryZLeft)
        {
            Destroy(gameObject);
        }
    }
}
