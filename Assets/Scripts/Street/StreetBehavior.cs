using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetBehavior : MonoBehaviour
{
    private Vector3 initializationPos;
    private BoxCollider streetBoxCollider;

    // Start is called before the first frame update
    // 1. Assigning values to variables
    void Start()
    {
        initializationPos = transform.position;
        streetBoxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    // 1. call a re location method when the relacal statement meets the requirements
    void Update()
    {
        if (RelocationCheck())
            Relocation();
    }

    //check whether the relocation situation meets the requirements
    private bool RelocationCheck()
    {
        
        float distance = Vector3.Distance(transform.position, initializationPos);
        return (distance >= streetBoxCollider.size.x * transform.localScale.x / 4);
    }

    //execute the relocation
    private void Relocation()
    {
        transform.position = initializationPos;
    }

}
