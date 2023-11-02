using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public Item Item;

    private void Pickup()
    {
        InventoryManagerTwo.Instance.Add(Item);
        Destroy(gameObject);
    }


    private void OnMouseDown()
    {
        Pickup();
    }
}
