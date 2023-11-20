using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool locked = false;
    public bool open = false;
    public InventoryManagerTwo inventoryManagerTwo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!open)
        {
            if (locked)
            {
                //if(hasKeys)
            }

            else
            {
                OpenDoor();
            }
        }
    }

    public void OpenDoor()
    {
        open = true;
        transform.GetComponent<Animator>().Play("DoorOpen");
    }
}
