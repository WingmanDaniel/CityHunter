using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    private Vector3 initializationPos;
    private BoxCollider streetBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        initializationPos = transform.position;
        streetBoxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RelocationCheck())
            Relocation();
    }

    private bool RelocationCheck()
    {

        float distance = Vector3.Distance(transform.position, initializationPos);
        //Debug.Log(distance);
        return (distance >= streetBoxCollider.size.x * transform.localScale.x / 2);
    }

    private void Relocation()
    {
        transform.position = initializationPos;
    }
}
