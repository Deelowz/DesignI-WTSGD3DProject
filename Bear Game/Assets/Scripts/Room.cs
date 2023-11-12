using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject roomFloor;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TransitionRoom()
    {
        mainCamera.GetComponent<CameraRoomFollower>().TransitionToRoom(roomFloor.transform.position);
    }
}
