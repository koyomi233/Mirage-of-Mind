using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarPanelController : MonoBehaviour
{
    public static ToolBarPanelController Instance;

    private ToolBarPanelView m_ToolBarPanelView;
    private ToolBarPanelModel m_ToolBarPanelModel;

    private List<GameObject> slotList = null;                   // Store all slots in bar
    private GameObject currentActive = null;                    // Store the active slot

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
        CreatAllSlot();
    }

    private void Init()
    {
        m_ToolBarPanelView = gameObject.GetComponent<ToolBarPanelView>();
        m_ToolBarPanelModel = gameObject.GetComponent<ToolBarPanelModel>();

        slotList = new List<GameObject>();
    }

    // Generate all slots
    private void CreatAllSlot()
    {
        for(int i = 0; i < 9; i++)
        {
            GameObject slot = GameObject.Instantiate<GameObject>(m_ToolBarPanelView.Prefab_ToolBarSlot, m_ToolBarPanelView.Grid_Transform);
            slot.GetComponent<ToolBarSlotController>().Init(m_ToolBarPanelView.Prefab_ToolBarSlot.name + i, i + 1);
            slotList.Add(slot);
        }
    }

    // Store the active slot and item
    private void SaveActiveSlot(GameObject activeSlot)
    {
        if (currentActive != null && currentActive != activeSlot)
        {
            currentActive.GetComponent<ToolBarSlotController>().Normal();
            currentActive = null;
        }
        currentActive = activeSlot;
    }

    public void SaveActiveSlotByKey(int keyNum)
    {
        if (currentActive != null && currentActive != slotList[keyNum])
        {
            currentActive.GetComponent<ToolBarSlotController>().Normal();
            currentActive = null;
        }
        currentActive = slotList[keyNum];
        currentActive.GetComponent<ToolBarSlotController>().SlotClick();
    }
}
