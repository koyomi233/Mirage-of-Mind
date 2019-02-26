using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingContentItem
{
    private int itemID;
    private string itemName;

    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }

    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }
}
