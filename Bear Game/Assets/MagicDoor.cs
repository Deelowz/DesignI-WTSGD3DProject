using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDoor : MonoBehaviour
{
    // Start is called before the first frame update

    public InventoryManagerTwo inventoryManagerTwo;

    public bool emerald = false;
    public bool ruby = false;
    public bool sapphire = false;

    private void OnMouseDown()
    {
        inventoryManagerTwo.GemDoorCheck();
    }

    public void RevealGem(string name)
    {
        if (name == "Emerald")
        {
            emerald = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (name == "Ruby")
        {
            ruby = true;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (name == "Sapphire")
        {
            sapphire = true;
            transform.GetChild(2).gameObject.SetActive(true);
        }

        if (emerald == true && ruby == true && sapphire == true)
        {
            //PLAY CUTSCENE
        }
    }
}
