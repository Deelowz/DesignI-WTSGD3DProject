using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 3f;

    public bool dashing;



    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;

    private float dashCooldownTime;
    private Vector3 dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        dashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (!dashing)
            {
                controller.Move(direction * speed * Time.deltaTime);
            }
            else
            {
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            }
        }


        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTime == 0)
        {
            dashing = true;
            
            dashDirection = direction;
            dashCooldownTime = dashCooldown;

            Invoke("DashDone", dashTime);
        }
    }

    private void FixedUpdate()
    {
        if (dashCooldownTime >= 0)
        {
            dashCooldownTime -= Time.deltaTime;
            if (dashCooldownTime <= 0)
            {
                dashCooldownTime = 0;
            }
        }
    }

    public void DashDone()
    {
        dashing = false;
    }
    public float GetHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public float GetVerticalInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public bool IsDashing()
    {
        return dashing;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "room")
        {
            other.GetComponent<Room>().TransitionRoom();
        }
    }
}
