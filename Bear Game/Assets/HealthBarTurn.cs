using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarTurn : MonoBehaviour
{

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Makes UI face camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
