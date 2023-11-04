using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerationTest : MonoBehaviour
{
    public Item[] commonItem;
    public Item[] uncommonItem;
    public Item[] rareItem;
    public Item emptySlot;

    public Item[] chestInventory;

    public int chestSlots = 15;

    public ChestInventoryManager chestInventoryScript;


    // Start is called before the first frame update
    void Start()
    {
        FillChest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest()
    {
        chestInventoryScript.transform.localPosition = new Vector2(-440, 0); // Moves chest inventory into view.
        chestInventoryScript.chestOpen = true;

        for (int i = 0; i < chestInventoryScript.chestInventorySlot.Length; i++) // Puts items into chest inventory panel.
        {
            chestInventoryScript.chestInventorySlot[i].GetComponent<ItemController>().Item = chestInventory[i];
        }

        chestInventoryScript.lootGenerationTest = this; // Sends it this specific instance of the script.
        chestInventoryScript.UpdateSlots();
        chestInventoryScript.MoveInventory();
    }


    public void FillChest()
    {
        for (int i = 0; i < chestSlots; i++)
        {
            char tier = RandomSpawnTier();

            switch (tier)
            {
                case 'r':
                    chestInventory[i] = rareItem[Random.Range(0, rareItem.Length)];
                    break;
                case 'u':
                    chestInventory[i] = uncommonItem[Random.Range(0, uncommonItem.Length)];
                    break;
                case 'c':
                    chestInventory[i] = commonItem[Random.Range(0, commonItem.Length)];
                    break;
                case 'e':
                    chestInventory[i] = emptySlot;
                    break;
                default:
                    //Debug.Log("Something went wrong with chest randomization.");
                    break;
            }
        }
    }


    public char RandomSpawnTier()
    {
        float p = Random.Range(0f, 1f);
        //Debug.Log(p);

        if (p <= 0.03125)
        {
            print("RARE");
            return 'r';
        }
        else if (p > 0.03125 && p <= 0.09375)
        {
            print("UNCOMMON");
            return 'u';
        }
        else if (p > 0.09375 && p <= 0.25)
        {
            print("COMMON");
            return 'c';
        }
        else
        {
            print("EMPTY");
            return 'e';
        }
    }
}
