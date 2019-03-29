using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Backpack controller
/// </summary>

public class InventoryPanelController : MonoBehaviour, IUIPanelShowAndHide
{
    public static InventoryPanelController Instance;

    // To achieve objects model and view
    private InventoryPanelModel m_InventoryPanelModel;
    private InventoryPanelView m_InventoryPanelView;

    private int slotNum = 30;
    private List<GameObject> slotList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

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
            tempSlot.name = "InventorySlot_" + i;
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
            temp.GetComponent<InventoryItemController>().initItem(tempList[i].ItemId, tempList[i].ItemName, tempList[i].ItemNum, tempList[i].ItemBar);
        }   
    }

    // Put materials back to inventory when change 
    public void AddItems(List<GameObject> itemList)
    {
        int tempIndex = 0;
        for(int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransform = slotList[i].transform.Find("InventoryItem");
            if (tempTransform == null && tempIndex < itemList.Count)
            {
                itemList[tempIndex].transform.SetParent(slotList[i].transform);
                itemList[tempIndex].GetComponent<InventoryItemController>().InInventory = true;
                tempIndex++;
            }
        }
    }

    public void SendDragMaterialsItem(GameObject item)
    {
        CraftingPanelController.Instance.DragMaterialsItem(item);
    }

    public void UIPanelShow()
    {
        gameObject.SetActive(true);
    }

    public void UIPanelHide()
    {
        gameObject.SetActive(false);
    }
}
