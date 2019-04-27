using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem 
{
    private int itemId;
    private string itemName;
    private int itemNum;
    private int itemBar;

    public int ItemId
    {
        get { return itemId; }
        set { itemId = value; }
    }

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

    public int ItemBar
    {
        get { return itemBar; }
        set { itemBar = value; }
    }

    public InventoryPanelModel InventoryPanelModel
    {
        get => default;
        set
        {
        }
    }

    public InventoryItem() { }
    public InventoryItem(int itemId, string itemName, int itemNum)
    {
        this.ItemId = itemId;
        this.ItemName = itemName;
        this.ItemNum = itemNum;
    }

    public override string ToString()
    {
        return string.Format("Item's id: {0}, Itme's name: {1}, Item's number: {2}", this.itemId, this.itemName, this.itemNum);
    }
}
