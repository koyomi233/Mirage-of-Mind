using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// Backpack data control
/// </summary>

public class InventoryPanelModel : MonoBehaviour
{
    void Awake()
    {
        
    }

    // Obtain list by searching JSON file name
    public List<InventoryItem> GetJsonList(string fileName)
    {
        return JsonTools.LoadJsonFile<InventoryItem>(fileName);
    }
}
