using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomFollower : MonoBehaviour
{
    public Vector3 currentRoom;
    public Vector3 target;
    public float speed = 1;
    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, speed * Time.deltaTime);
    }


    public void TransitionToRoom(Vector3 newRoomFloor)
    {
        target = new Vector3 (newRoomFloor.x, 25f, newRoomFloor.z - 7);
    }
}
