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

    public List<InventoryItem> GetJsonList(string fileName)
    {
        List<InventoryItem> tempList = new List<InventoryItem>();
        string tempJsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;

        JsonData jsonData = JsonMapper.ToObject(tempJsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            InventoryItem inventoryItem = JsonMapper.ToObject<InventoryItem>(jsonData[i].ToJson());
            tempList.Add(inventoryItem);
        }

        return tempList;
    }
}
