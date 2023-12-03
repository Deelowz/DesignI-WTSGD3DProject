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
        inventoryManagerTwo = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManagerTwo>();
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
                if (inventoryManagerTwo.CheckKey())
                {
                    locked = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                }
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
