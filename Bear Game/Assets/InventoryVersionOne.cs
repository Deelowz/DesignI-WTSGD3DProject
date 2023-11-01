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
    public GameObject dropButton; // Might not need this since it doesn't change based on the type of item selected.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemSelect(GameObject button)
    {
        Item buttonScript = button.GetComponent<Item>();

        for (int i = 0; i < inventorySlot.Length; i++) // Goes through and unselects any selected buttons.
        {
            inventorySlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        button.GetComponent<UnityEngine.UI.Button>().interactable = false; // "Selects" the button.
        itemName.text = buttonScript.itemName; // Updates item information.
        itemDescription.text = buttonScript.itemDescription; // Updates item information.

        
        if (!buttonScript.equipped) // Makes button equip/unequip based on if it is equipped or not.
        {
            interactButton.text = "Equip";
        }
        else if (buttonScript.equipped)
        {
            interactButton.text = "Unequip";
        }
    }
}
