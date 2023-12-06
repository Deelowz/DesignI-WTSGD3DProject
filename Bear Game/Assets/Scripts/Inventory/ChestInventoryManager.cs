using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventoryManager : MonoBehaviour
{
    public GameObject[] chestInventorySlot;

    public InventoryManagerTwo inventoryManagerTwo;
    public LootGenerationTest lootGenerationTest;

    public GameObject inventoryPanel;

    public bool chestOpen = false;

    public Item emptySlot;
    public int currentSlotNumber;

    public GameObject currentChest;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (chestOpen)
            {
                transform.localPosition = new Vector2(0, 1000); // Moves chest panel away.
                inventoryPanel.transform.localPosition = new Vector2(298.85f, 74); // Moves inventory panel back to main inventory panel.
                chestOpen = false;
            }
        }
    }

    public void CloseInventory()
    {
        if(currentChest != null)
        {
            currentChest.transform.GetComponent<Animator>().Play("CloseChest");
            Invoke("ParticlesStop", 0.7f);
        }
        transform.localPosition = new Vector2(0, 1000); // Moves chest panel away.
        inventoryPanel.transform.localPosition = new Vector2(298.85f, 74); // Moves inventory panel back to main inventory panel, which is moved into view.
        chestOpen = false; // Moves inventory panel away.
    }

    public void ParticlesStop()
    {
        currentChest.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void MoveInventory()
    {
        inventoryPanel.transform.localPosition = new Vector2(298.85f, - 1150);
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < chestInventorySlot.Length; i++) // Goes through and unselects any selected buttons.
        {
            chestInventorySlot[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = chestInventorySlot[i].GetComponent<ItemController>().Item.icon; // Updates all the icons.
            chestInventorySlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true; // Sets all the buttons to interactable.
        }
    }
    

    public void GiveSlotNumber(int slot)
    {
        currentSlotNumber = slot;
    }
    public void TakeItem(GameObject item)
    {
        inventoryManagerTwo.PickupItem(item);

        if (item.GetComponent<ItemController>().Item.itemName == "Empty")
        {
            lootGenerationTest.chestInventory[currentSlotNumber] = emptySlot;
        }
    }
}
