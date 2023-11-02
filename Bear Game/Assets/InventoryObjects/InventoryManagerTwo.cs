using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Contracts;

public class InventoryManagerTwo : MonoBehaviour
{
    public bool[] inventorySlotFilled;
    public GameObject[] inventorySlot;

    public bool[] equippedSlotFilled;
    public GameObject[] equippedSlot;


    public TMP_Text itemName;
    public TMP_Text itemDescription;

    public TMP_Text interactButton;
    public GameObject descriptionPanel;
    public GameObject selectedButton;

    public Item emptyItem;

    public int health = 100;
    public float armor = 0;
    public int healthBuff = 0;
    public int meleeDamage = 0;
    public int rangedDamage = 0;


    void Start()
    {
        UpdateSlots();
        UpdateStats();
    }

    public void ItemSelect(UnityEngine.UI.Button button)
    {
        Item itemInfo = button.transform.GetComponent<ItemController>().Item; // Gets the button's script.

        selectedButton = button.gameObject;

        UpdateSlots(); // Makes all buttons interactable before disables specifically the clicked one.

        button.interactable = false; // "Selects" the button.

        itemName.text = itemInfo.itemName; // Updates item name text.
        itemDescription.text = itemInfo.itemDescription; // Updates item description text.

        if (itemInfo.type == 5) // Checks if the slot is empty so it can potentially hide the description box.
        {
            descriptionPanel.SetActive(false);
        }
        else if (itemInfo.type == 4)
        {
            descriptionPanel.SetActive(true);
            interactButton.text = "Use";
        }
        else
        {
            descriptionPanel.SetActive(true);
            interactButton.text = "Equip";
        }
    }

    public void EquippedItemSelect(UnityEngine.UI.Button button)
    {
        Item itemInfo = button.transform.GetComponent<ItemController>().Item; // Gets the button's script.

        selectedButton = button.gameObject;

        UpdateSlots(); // Makes all buttons interactable before disables specifically the clicked one.

        button.interactable = false; // "Selects" the button.

        itemName.text = itemInfo.itemName; // Updates item name text.
        itemDescription.text = itemInfo.itemDescription; // Updates item description text.

        if (itemInfo.type == 5) // Checks if the slot is empty so it can potentially hide the description box.
        {
            descriptionPanel.SetActive(false);
        }
        else
        {
            descriptionPanel.SetActive(true);
            interactButton.text = "Unequip";
        }
    }

    public void PickupItem(GameObject item)
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (!inventorySlotFilled[i]) // Checks for an empty slot.
            {
                inventorySlotFilled[i] = true; // If one is found, sets the slot to filled.
                inventorySlot[i].GetComponent<ItemController>().Item = item.GetComponent<ItemController>().Item; // Makes the inventory slot hold the item.
                item.GetComponent<ItemController>().Item = emptyItem; // Makes the old slot empty.
                item.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<ItemController>().Item.icon; // Sets the old slot icon to empty.
                descriptionPanel.SetActive(false);
                UpdateSlots();
                return;
            }
        }
    }

    public void DropItem()
    {
        selectedButton.GetComponent<ItemController>().Item = emptyItem;
        UpdateSlots();
        descriptionPanel.SetActive(false);

        UpdateStats();

        // Drop something on ground later.
    }

    public void InteractItem()
    {
        if (interactButton.text == "Equip") // Checks if it is an equippable item.
        {
            int type = selectedButton.GetComponent<ItemController>().Item.type;

            if (!equippedSlotFilled[type]) // Equips the item to the correct slot based on the Item's "type" value. 
            {
                equippedSlotFilled[type] = true;
                equippedSlot[type].GetComponent<ItemController>().Item = selectedButton.GetComponent<ItemController>().Item;
                equippedSlot[type].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = selectedButton.GetComponent<ItemController>().Item.icon;
                selectedButton.GetComponent<ItemController>().Item = emptyItem;
                descriptionPanel.SetActive(false);

                UpdateStats(); // Updates the stats.
            }
            else
            {
                // Slot is full.
            }
        }
        else if (interactButton.text == "Unequip")
        {
            PickupItem(selectedButton); // Just picks up the item, putting it in the inventory if there's room.
            UpdateStats();
        }
        else if (interactButton.text == "Use")
        {
            if (health < 100) // Checks that the player isn't at full health already.
            {
                health += selectedButton.GetComponent<ItemController>().Item.value; //Heals player!

                selectedButton.GetComponent<ItemController>().Item = emptyItem;
                UpdateSlots();
                descriptionPanel.SetActive(false);
            }
        }

        UpdateSlots();
    }

    public void UpdateStats() // Updates all the stats to reflect whatever is equipped.
    {
        healthBuff = equippedSlot[0].GetComponent<ItemController>().Item.value;
        armor = equippedSlot[1].GetComponent<ItemController>().Item.value;
        meleeDamage = equippedSlot[2].GetComponent<ItemController>().Item.value;
        rangedDamage = equippedSlot[3].GetComponent<ItemController>().Item.value;
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++) // Goes through and unselects any selected buttons.
        {
            inventorySlot[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = inventorySlot[i].GetComponent<ItemController>().Item.icon; // Updates all the icons.
            inventorySlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true; // Sets all the buttons to interactable.

            if (inventorySlot[i].GetComponent<ItemController>().Item.itemName == "Empty") // Checks for any empty slots.
            {
                inventorySlotFilled[i] = false; // Updates those slots to be empty in terms of the boolean value.
            }
        }

        for (int i = 0; i < equippedSlotFilled.Length; i++) // Goes through and unselects any selected buttons.
        {
            equippedSlot[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = equippedSlot[i].GetComponent<ItemController>().Item.icon; // Updates all the icons.
            equippedSlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true; // Sets all the buttons to interactable.

            if (equippedSlot[i].GetComponent<ItemController>().Item.itemName == "Empty") // Checks for any empty slots.
            {
                equippedSlotFilled[i] = false; // Updates those slots to be empty in terms of the boolean value.
            }
        }
    }
}
