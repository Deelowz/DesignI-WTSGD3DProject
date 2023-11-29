using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    private PlayerMovement playerMovement; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorParameters();
    }

    void UpdateAnimatorParameters()
    {
        float horizontal = playerMovement.GetHorizontalInput();
        float vertical = playerMovement.GetVerticalInput();

        // Calculate the magnitude of the input vector
        float inputMagnitude = new Vector2(horizontal, vertical).magnitude;

        // Update the Animator parameters
        animator.SetFloat("speed", inputMagnitude);
        //animator.SetBool("isWalking", inputMagnitude >= 0.1f);
        animator.SetBool("isIdle", inputMagnitude < 0.1f);
        animator.SetBool("isDashing", playerMovement.IsDashing());

        if (inputMagnitude >= 0.1f)
        {
            //Debug.Log(transform.eulerAngles.y);
            //animator.SetBool("isWalking", true);
            //animator.SetBool("isWalking", true);
            //animator.SetBool("isWalking", true);
            //animator.SetBool("isWalking", true);

            if (transform.eulerAngles.y > 270 || transform.eulerAngles.y < 45) // Facing "UP"
            {
                //animator.SetBool("isWalking", true);
                animator.Play("walk");
                Debug.Log("up");
            }
            else if (transform.eulerAngles.y > 45 && transform.eulerAngles.y < 135) // Facing "RIGHT"
            {
                animator.Play("idle");
                //animator.SetBool("isWalking", false);
                Debug.Log("right");
            }
            else if (transform.eulerAngles.y > 135 && transform.eulerAngles.y < 225) // Facing "DOWN"
            {
                animator.Play("idle");
                //animator.SetBool("isWalking", false);
                Debug.Log("down");
            }
            else if (transform.eulerAngles.y > 225 && transform.eulerAngles.y < 270) // Facing "LEFT"
            {
                animator.Play("idle");
                //animator.SetBool("isWalking", false);
                Debug.Log("left");
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
