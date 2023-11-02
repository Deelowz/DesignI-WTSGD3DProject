using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryVersionOne : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < inventorySlot.Length; i++) // Goes through and unselects any selected buttons.
        //{
        //        inventorySlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
        //}
        //UpdateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemSelect(GameObject button)
    {
        Item buttonScript = button.GetComponent<Item>(); // Gets the button's script.
        selectedButton = button; // Makes sure this script holds the value of the selected button gameObject.

        UpdateSlots();

        button.GetComponent<UnityEngine.UI.Button>().interactable = false; // "Selects" the button.
        itemName.text = buttonScript.itemName; // Updates item information.
        itemDescription.text = buttonScript.itemDescription; // Updates item information.


        if (buttonScript.itemName == string.Empty) // Checks if the slot is empty so it can potentially hide the description box.
        {
            descriptionPanel.SetActive(false);
        }
        else
        {
            descriptionPanel.SetActive(true);
        }


        //if (buttonScript.equipped) // Changes the equip button based on the item type and its interaction.
        //{
        //    interactButton.text = "Unequip";
        //}
        //else if (buttonScript.consumable)
        //{
        //    interactButton.text = "Use";
        //}
        //else
        //{
        //    interactButton.text = "Equip";
        //}
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < inventorySlot.Length; i++) // Goes through and unselects any selected buttons.
        {
            inventorySlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void PickupItem(GameObject incomingItem)
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlotFilled[i])
            {
                inventorySlot[i].GetComponent<Item>().itemName = incomingItem.GetComponent<Item>().itemName;
                inventorySlot[i].GetComponent<Item>().itemDescription = incomingItem.GetComponent<Item>().itemDescription;
                //inventorySlot[i].GetComponent<Item>().consumable = incomingItem.GetComponent<Item>().consumable;
                //inventorySlot[i].GetComponent<UnityEngine.UI.Image>().sprite = incomingItem.GetComponent<Item>().sprite;

                Destroy(incomingItem);
            }
        }
    }
}
