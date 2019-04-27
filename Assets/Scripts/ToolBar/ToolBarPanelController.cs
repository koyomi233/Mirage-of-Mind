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
    private GameObject currentActiveModel = null;               // Store the active model
    private int currentKeyCode = -1;                            // Store the current slot

    private Dictionary<GameObject, GameObject> toolBarDic = null;

    public GameObject CurrentActiveModel { get { return currentActiveModel; } }

    public ToolBarSlotController ToolBarSlotController
    {
        get => default;
        set
        {
        }
    }

    public ToolBarPanelModel ToolBarPanelModel
    {
        get => default;
        set
        {
        }
    }

    public ToolBarPanelView ToolBarPanelView
    {
        get => default;
        set
        {
        }
    }

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
        toolBarDic = new Dictionary<GameObject, GameObject>();
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
        if (slotList[keyNum].GetComponent<Transform>().Find("InventoryItem") == null)
        {
            return;
        }

        if (currentActive != null && currentActive != slotList[keyNum])
        {
            currentActive.GetComponent<ToolBarSlotController>().Normal();
            currentActive = null;
        }
        currentActive = slotList[keyNum];
        currentActive.GetComponent<ToolBarSlotController>().SlotClick();

        if (currentKeyCode == keyNum && currentActiveModel != null)                           // Drop down weapon
        {
            currentActiveModel.SetActive(false);
            currentActiveModel = null;
        }
        else                                                                                  // Switch weapon
        {
            FindInventoryItem();
        }
        // Store the current slot
        currentKeyCode = keyNum;
    }

    // Call GunFactory class
    private void FindInventoryItem()
    {
        Transform m_temp = currentActive.GetComponent<Transform>().Find("InventoryItem");
        StartCoroutine("CallGunFactory", m_temp);
    }

    private IEnumerator CallGunFactory(Transform m_temp)
    {
        if (m_temp != null)
        {
            // Hide the current model
            if (currentActiveModel != null)
            {
                currentActiveModel.GetComponent<GunControllerBase>().Holster();
                yield return new WaitForSeconds(0.8f);
                currentActiveModel.SetActive(false);
            }
            GameObject temp = null;
            toolBarDic.TryGetValue(m_temp.gameObject, out temp);
            if (temp == null)
            {
                temp = GunFactory.Instance.CreateGun(m_temp.GetComponent<Image>().sprite.name, m_temp.gameObject);
                toolBarDic.Add(m_temp.gameObject, temp);
            }
            else
            {
                if (currentActive.GetComponent<ToolBarSlotController>().SelfState)
                    temp.SetActive(true);
            }
            currentActiveModel = temp;
        }
    }
}
