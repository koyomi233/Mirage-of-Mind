using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crafting Module Controller
/// </summary>

public class CraftingPanelController : MonoBehaviour
{
    private CraftingPanelModel m_CraftingPanelModel;
    private CraftingPanelView m_CraftingPanelView;

    private int tabsNum = 4;
    private List<GameObject> tabList;
    private List<GameObject> contentsList;

    void Start()
    {
        Init();

        CreateAllTabs();
        CreateAllContents();
        ResetTabsAndContents(0);
    }

    // Initialization 
    private void Init()
    {
        m_CraftingPanelModel = gameObject.GetComponent<CraftingPanelModel>();
        m_CraftingPanelView = gameObject.GetComponent<CraftingPanelView>();

        tabList = new List<GameObject>();
        contentsList = new List<GameObject>();
    }

    // Generate all tabs
    private void CreateAllTabs()
    {
        for (int i = 0; i < tabsNum; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(m_CraftingPanelView.Prefab_TabsItem, m_CraftingPanelView.Tabs_Transform);
            run.GetComponent<CraftingTabItemController>().InitItem(i);
            tabList.Add(run);
        }
    }

    // Generate all contents
    private void CreateAllContents()
    {
        for(int i = 0; i < tabsNum; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(m_CraftingPanelView.Prefab_Content, m_CraftingPanelView.Contents_Transform);
            run.GetComponent<CraftingContentController>().InitContent(i, m_CraftingPanelView.Prefab_ContentItem);
            contentsList.Add(run);
        }
    }

    // Reset tabs and text field
    private void ResetTabsAndContents(int index)
    {
        for (int i = 0; i < tabList.Count; i++)
        {
            tabList[i].GetComponent<CraftingTabItemController>().NormalTab();
            contentsList[i].SetActive(false);
        }
        tabList[index].GetComponent<CraftingTabItemController>().ActiveTab();
        contentsList[index].SetActive(true);
    }

}
