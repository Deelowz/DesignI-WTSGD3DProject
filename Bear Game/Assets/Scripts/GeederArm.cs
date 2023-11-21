using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeederArm : MonoBehaviour
{
    public Animator animator;
    public Animator movementAnimator;
    public InventoryManagerTwo inventoryManagerTwo;

    public GameObject arm;


    public bool isSwiping = false;
    private float coolDown = 5f;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Swipe()
    {
        if (!isSwiping)
        {
            isSwiping = true;

            animator.Play("Swipe");
            movementAnimator.Play("ArmSwipeMove");

            Invoke(nameof(ResetSwipe), coolDown);
        }

    }

    private void ResetSwipe()
    {
        isSwiping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManagerTwo.ItemStolen();
            Swipe();
        }
    }
}
