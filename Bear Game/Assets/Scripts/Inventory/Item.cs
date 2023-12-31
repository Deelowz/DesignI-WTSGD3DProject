using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string itemDescription;
    public int value;
    public Sprite icon;

    public int type;

    //---StackableTest-----

    public bool stackable = false;
    public int amount = 1;

}
