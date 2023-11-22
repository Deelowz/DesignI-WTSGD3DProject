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
        animator.SetBool("isWalking", inputMagnitude >= 0.1f);
        animator.SetBool("isIdle", inputMagnitude < 0.1f);
        animator.SetBool("isDashing", playerMovement.IsDashing());
    }
}
