using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crafting Module Controller
/// </summary>

public class CraftingPanelController : MonoBehaviour
{
    private Transform m_Transform;

    private CraftingPanelModel m_CraftingPanelModel;
    private CraftingPanelView m_CraftingPanelView;
    private CraftingController m_CraftingController;

    private int tabsNum = 2;
    private int slotsNum = 25;
    private List<GameObject> tabList;
    private List<GameObject> contentsList;
    private List<GameObject> slotsList;

    private int currentIndex = -1;

    void Start()
    {
        Init();

        CreateAllTabs();
        CreateAllContents();
        ResetTabsAndContents(0);
        CreateAllSlots();
        
    }

    // Initialization 
    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        m_CraftingPanelModel = gameObject.GetComponent<CraftingPanelModel>();
        m_CraftingPanelView = gameObject.GetComponent<CraftingPanelView>();
        m_CraftingController = m_Transform.Find("Right").GetComponent<CraftingController>();

        tabList = new List<GameObject>();
        contentsList = new List<GameObject>();
        slotsList = new List<GameObject>();
    }

    // Generate all tabs
    private void CreateAllTabs()
    {
        for (int i = 0; i < tabsNum; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(m_CraftingPanelView.Prefab_TabsItem, m_CraftingPanelView.Tabs_Transform);
            Sprite temp = m_CraftingPanelView.GetSpriteByName(m_CraftingPanelModel.GetTabsIconName()[i]);
            run.GetComponent<CraftingTabItemController>().InitItem(i, temp);
            tabList.Add(run);
        }
    }

    // Generate all contents
    private void CreateAllContents()
    {
        List<List<CraftingContentItem>> tempList = m_CraftingPanelModel.GetJsonByName("CraftingContentsJsonData");

        for (int i = 0; i < tabsNum; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(m_CraftingPanelView.Prefab_Content, m_CraftingPanelView.Contents_Transform);
            run.GetComponent<CraftingContentController>().InitContent(i, m_CraftingPanelView.Prefab_ContentItem, tempList[i]);
            contentsList.Add(run);
        }
    }

    // Reset tabs and text field
    private void ResetTabsAndContents(int index)
    {
        if (currentIndex == index) return;
        for (int i = 0; i < tabList.Count; i++)
        {
            tabList[i].GetComponent<CraftingTabItemController>().NormalTab();
            contentsList[i].SetActive(false);
        }
        tabList[index].GetComponent<CraftingTabItemController>().ActiveTab();
        contentsList[index].SetActive(true);
        currentIndex = index;
    }

    // Generate all slots
    private void CreateAllSlots()
    {
        for(int i = 0; i < slotsNum; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(m_CraftingPanelView.Prefab_Slot, m_CraftingPanelView.Center_Transform);
            run.name = "Slot" + i;
            slotsList.Add(run);
        }
    }

    // Add data to slots 
    private void CreateSlotContents(int id)
    {
        CraftingMapItem temp = m_CraftingPanelModel.GetItemById(id);
        if(temp != null)
        {
            ResetSlotsContents();
            ResetMaterials();

            for (int j = 0; j < temp.MapContents.Length; j++)
            {
                if (temp.MapContents[j] != "0")
                {
                    Sprite sp = m_CraftingPanelView.GetMaterialIconSpriteByName(temp.MapContents[j]);
                    slotsList[j].GetComponent<CraftingSlotController>().Init(sp, temp.MapContents[j]);
                }
            }
            // Finally show the generated item
            m_CraftingController.Init(temp.MapName);
        }
    }

    // Clear contents of slots
    private void ResetSlotsContents()
    {
        for (int i = 0; i < slotsList.Count; i++)
        {
            slotsList[i].GetComponent<CraftingSlotController>().Reset();
        }
    }

    // Reset materials in crafting panel
    private void ResetMaterials()
    {
        List<GameObject> materialsList = new List<GameObject>();
        for (int i = 0; i < slotsList.Count; i++)
        {
            Transform tempTransform = slotsList[i].transform.Find("InventoryItem");
            if (tempTransform != null)
            {
                materialsList.Add(tempTransform.gameObject);
            }
        }
        InventoryPanelController.Instance.AddItems(materialsList);
    }
}
