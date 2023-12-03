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
    public GameObject dropButton;
    public GameObject descriptionPanel;
    public GameObject selectedButton;

    public CombatVersionOne combatVersionOne;
    public ChestInventoryManager chestInventoryManager;

    public Item emptyItem;

    public int health = 100;
    public float armor = 0;
    public int healthBuff = 0;
    public int meleeDamage = 0;
    public int rangedDamage = 0;

    public bool inventoryOpen = false;

    public MagicDoor magicDoor;


    void Start()
    {
        UpdateSlots();
        UpdateStats();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (chestInventoryManager.chestOpen == true)
            {
                chestInventoryManager.CloseInventory();
            }
            else
            {
                if (inventoryOpen) // Toggles inventory, moving it instead of disabling the panel.
                {
                    transform.localPosition = new Vector2(0, 1000);
                    inventoryOpen = false;
                    UpdateSlots();
                    descriptionPanel.SetActive(false);
                }
                else
                {
                    transform.localPosition = new Vector2(0, 25);
                    inventoryOpen = true;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && inventoryOpen) // Closes inventory with escape as well.
        {
            transform.localPosition = new Vector2(0, 1000);
            inventoryOpen = false;
            UpdateSlots();
            descriptionPanel.SetActive(false);
        }
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
        else if (itemInfo.type == 4) // Healing items
        {
            descriptionPanel.SetActive(true);
            interactButton.text = "Use";
        }
        else if (itemInfo.type == 6) // Gemstones for door. Disables the ability to interact or drop them because they're necessary.
        {
            descriptionPanel.SetActive(true);
            dropButton.SetActive(false);
            interactButton.transform.parent.gameObject.SetActive(false);
        }
        else // equippable items like weapons, armor, etc
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
            if (item.GetComponent<ItemController>().Item.stackable == true)
            {
                // Checks if the item already exists in the player's inventory and stacks it.
                if (inventorySlot[i].GetComponent<ItemController>().Item.itemName == (item.GetComponent<ItemController>().Item.itemName))
                {
                    inventorySlot[i].GetComponent<ItemController>().Item.amount += 1;
                    item.GetComponent<ItemController>().Item = emptyItem;
                    item.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<ItemController>().Item.icon;
                    descriptionPanel.SetActive(false);
                    UpdateSlots();
                    return;
                }
                else if (item.GetComponent<ItemController>().Item.id == equippedSlot[3].GetComponent<ItemController>().Item.id && item != equippedSlot[3]) // Checks if the player has the type of rock equipped.
                {
                    equippedSlot[3].GetComponent<ItemController>().Item.amount += 1;
                    item.GetComponent<ItemController>().Item = emptyItem;
                    item.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<ItemController>().Item.icon;
                    descriptionPanel.SetActive(false);
                    UpdateSlots();
                    return;
                }

                // If it does not exist, puts it in the inventory like normal.
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
            else // Puts it in the inventory like normal if the item is not stackable.
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

                //New rock throw stuff.
                //combatVersionOne.totalThrows = equippedSlot[type].GetComponent<ItemController>().Item.amount; // When you equip a rock, your total throws become the amount of that rock you have.


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
            if (combatVersionOne.healthSlider.value < combatVersionOne.healthSlider.maxValue) // Checks that the player isn't at full health already.
            {
                combatVersionOne.HealHealth(selectedButton.GetComponent<ItemController>().Item.value); //Heals player!

                selectedButton.GetComponent<ItemController>().Item = emptyItem;
                UpdateSlots();
                descriptionPanel.SetActive(false);
            }
        }

        UpdateSlots();
    }

    public void UpdateStats() // Updates all the stats to reflect whatever is equipped.
    {
        combatVersionOne.HealthBoost(equippedSlot[0].GetComponent<ItemController>().Item.value); // Adds healthboost
        combatVersionOne.armor = equippedSlot[1].GetComponent<ItemController>().Item.value; // Armor
        combatVersionOne.armorSlider.value = equippedSlot[1].GetComponent<ItemController>().Item.value; // Updates the armor slider.

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

                inventorySlot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                if (inventorySlot[i].GetComponent<ItemController>().Item.stackable)
                {
                    inventorySlot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inventorySlot[i].GetComponent<ItemController>().Item.amount.ToString(); // Updates the text.
                }
            }
        }

        for (int i = 0; i < equippedSlotFilled.Length; i++) // Goes through and unselects any selected equipped buttons.
        {
            equippedSlot[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = equippedSlot[i].GetComponent<ItemController>().Item.icon; // Updates all the icons.
            equippedSlot[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true; // Sets all the buttons to interactable.

            if (equippedSlot[i].GetComponent<ItemController>().Item.itemName == "Empty") // Checks for any empty slots.
            {
                equippedSlotFilled[i] = false; // Updates those slots to be empty in terms of the boolean value.

                equippedSlot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                if (equippedSlot[i].GetComponent<ItemController>().Item.stackable)
                {
                    equippedSlot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = equippedSlot[i].GetComponent<ItemController>().Item.amount.ToString(); // Updates the text.
                }
            }
        }


        // Important, sets the buttons back to active.
        dropButton.SetActive(true);
        interactButton.transform.parent.gameObject.SetActive(true);

    }


    public void GemDoorCheck() // checks for gemstones
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].GetComponent<ItemController>().Item.type == 6 && inventorySlot[i].GetComponent<ItemController>().Item.name != "Key")
            {
                selectedButton = inventorySlot[i];
                magicDoor.RevealGem(inventorySlot[i].GetComponent<ItemController>().Item.name);
                DropItem();

                return;
            }
        }
    }

    public bool CheckKey() // checks fora key and whatnot
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].GetComponent<ItemController>().Item.type == 6 && inventorySlot[i].GetComponent<ItemController>().Item.name == "Key")
            {
                selectedButton = inventorySlot[i];
                DropItem();
                return true;
            }
        }

        return false;
    }


    public void ItemStolen()
    {
        // Checks to make sure your inventory isn't empty
        if (inventorySlotFilled[0] || inventorySlotFilled[1] || inventorySlotFilled[2] || inventorySlotFilled[3] || inventorySlotFilled[4] || inventorySlotFilled[5] || inventorySlotFilled[6] || inventorySlotFilled[7] || inventorySlotFilled[8] || inventorySlotFilled[9] || inventorySlotFilled[10] || inventorySlotFilled[11] || inventorySlotFilled[12] || inventorySlotFilled[13] || inventorySlotFilled[14] || equippedSlotFilled[0] || equippedSlotFilled[1] || equippedSlotFilled[2] || equippedSlotFilled[3])
        {
            int chosenSlot = Random.Range(0, 18); // Chooses random inventory slot.
            Debug.Log(chosenSlot);

            if (chosenSlot > 3) // Checks if the slot is going to be inventory (5-19) or equipped item slots (1-4).
            {
                chosenSlot -= 4; // Subtracts 4 to adjust for choosing one of the 15 inventory slots.
                if (inventorySlot[chosenSlot].GetComponent<ItemController>().Item.itemName == "Empty" || inventorySlot[chosenSlot].GetComponent<ItemController>().Item.type == 6) // Checks if it is empty and reruns method if it is until it's an actual item. Also checks if it is key item.
                {
                    ItemStolen();
                }
                else if (inventorySlot[chosenSlot].GetComponent<ItemController>().Item.stackable == true)
                {
                    selectedButton = inventorySlot[chosenSlot]; // Selects it.
                                                                // Play sound effect.

                    inventorySlot[chosenSlot].GetComponent<ItemController>().Item.amount--;
                    UpdateSlots();

                    if (inventorySlot[chosenSlot].GetComponent<ItemController>().Item.amount <= 0)
                    {
                        inventorySlot[chosenSlot].GetComponent<ItemController>().Item.amount = 1;
                        DropItem(); // Drops it.
                    }

                }
                else
                {
                    selectedButton = inventorySlot[chosenSlot]; // Selects it.
                                                                // Play sound effect.
                    DropItem(); // Drops it.
                }
            }
            else if (chosenSlot > -1 && chosenSlot < 4)
            {
                if (equippedSlot[chosenSlot].GetComponent<ItemController>().Item.itemName == "Empty") // Checks if it is empty and reruns method if it is until it's an actual item.
                {
                    ItemStolen();
                }
                else if (equippedSlot[chosenSlot].GetComponent<ItemController>().Item.stackable == true)
                {
                    selectedButton = equippedSlot[chosenSlot]; // Selects it.
                                                               // Play sound effect.

                    equippedSlot[chosenSlot].GetComponent<ItemController>().Item.amount--;
                    UpdateSlots();

                    if (equippedSlot[chosenSlot].GetComponent<ItemController>().Item.amount <= 0)
                    {
                        equippedSlot[chosenSlot].GetComponent<ItemController>().Item.amount = 1;
                        DropItem(); // Drops it.
                    }

                }
                else
                {
                    selectedButton = equippedSlot[chosenSlot]; // Selects it.
                                                               // Play sound effect.
                    DropItem(); // Drops it.
                }
            }
            else
            {

            }
        }
    }
}
