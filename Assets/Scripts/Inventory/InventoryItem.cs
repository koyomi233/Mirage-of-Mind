using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem 
{
    private string itemName;
    private int itemNum;

    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public int ItemNum
    {
        get { return itemNum; }
        set { itemNum = value; }
    }

    public InventoryItem() { }
    public InventoryItem(string itemName, int itemNum)
    {
        this.ItemName = itemName;
        this.ItemNum = itemNum;
    }

    public override string ToString()
    {
        return string.Format("Itme's name: {0}, Item's number: {1}", this.itemName, this.itemNum);
    }
}
