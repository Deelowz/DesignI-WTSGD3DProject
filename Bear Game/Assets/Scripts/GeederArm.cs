using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeederArm : MonoBehaviour
{
    public Animator animator;
    public InventoryManagerTwo inventoryManagerTwo;

    public bool isSwiping = false;
    public float coolDown = 3f;

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
            //animator.Play("Swipe");
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
        }
    }
}
