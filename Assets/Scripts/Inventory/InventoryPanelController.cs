using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Backpack controller
/// </summary>

public class InventoryPanelController : MonoBehaviour
{
    // To achieve objects model and view
    private InventoryPanelModel m_InventoryPanelModel;
    private InventoryPanelView m_InventoryPanelView;

    private int slotNum = 24;
    private List<GameObject> slotList = new List<GameObject>();

    void Start()
    {
        m_InventoryPanelModel = gameObject.GetComponent<InventoryPanelModel>();
        m_InventoryPanelView = gameObject.GetComponent<InventoryPanelView>();
        CreateAllSlot();
        CreateAllItem();
    }

    // Generate all slots
    private void CreateAllSlot()
    {
        for(int i = 0; i < slotNum; i++)
        {
            GameObject tempSlot = GameObject.Instantiate<GameObject>(m_InventoryPanelView.Prefab_Slot, m_InventoryPanelView.GetGridTransform);
            slotList.Add(tempSlot);
        }
    }

    // Generate all items
    private void CreateAllItem()
    {
        List<InventoryItem> tempList = m_InventoryPanelModel.GetJsonList("InventoryJsonData");
        
        for (int i = 0; i < tempList.Count; i++)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(m_InventoryPanelView.Prefab_Item, slotList[i].GetComponent<Transform>());
            temp.GetComponent<InventoryItemController>().initItem(tempList[i].ItemName, tempList[i].ItemNum);
        }   
    }
}
