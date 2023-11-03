using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventoryManager : MonoBehaviour
{
    public GameObject[] chestInventorySlot;

    public InventoryManagerTwo inventoryManagerTwo;

    public GameObject inventoryPanel;

    public bool chestOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (chestOpen) // Toggles inventory, moving it instead of disabling the panel.
            {
                transform.localPosition = new Vector2(0, 1000);
                inventoryPanel.transform.localPosition = new Vector2(298.85f, 74);
                chestOpen = false;
            }
        }
    }

    public void MoveInventory()
    {
        inventoryPanel.transform.localPosition = new Vector2(298.85f, - 981);
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < chestInventorySlot.Length; i++) // Goes through and unselects any selected buttons.
        {
            chestInventorySlot[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = chestInventorySlot[i].GetComponent<ItemController>().Item.icon; // Updates all the icons.
            chestInventorySlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true; // Sets all the buttons to interactable.
        }
    }

    public void TakeItem(GameObject item)
    {
        inventoryManagerTwo.PickupItem(item);
    }
}
