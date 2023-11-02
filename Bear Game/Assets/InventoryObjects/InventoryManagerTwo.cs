using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerTwo : MonoBehaviour
{
    public static InventoryManagerTwo Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.name;

            //assigning the item stuff
        }
    }
}
