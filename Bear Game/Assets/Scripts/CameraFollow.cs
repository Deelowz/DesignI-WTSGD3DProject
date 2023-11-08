using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public LayerMask mask;

    private Vector3 offset; 

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        RaycastHit hit;
        if (Physics.Linecast(player.position, targetPosition, out hit, mask))
        {
            transform.position = hit.point + hit.normal * 0.2f;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
    }
}
