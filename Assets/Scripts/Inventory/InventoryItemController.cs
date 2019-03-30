using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Item controller
/// </summary>

public class InventoryItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform m_RectTransform;
    private CanvasGroup m_CanvasGroup;

    private Image m_Image;                         // item icon
    private Image m_Bar;                         // item durability
    private Text m_Text;                           // item number
    private int id;                                // self id
    private int num = 0;                           // number of item
    private bool isDrag = false;                   // drag status
    private bool inInventory = true;               // whether in inventory
    private int bar = 0;                           // whether need durability. 0: need, 1: don't need

    private Transform parent;                      // temporary parent of item
    private Transform self_parent;                 // original parent of item

    public int Num
    {
        get { return num; }
        set {
            num = value;
            m_Text.text = num.ToString();
        }
    }

    public int Id { get { return id; } set { id = value; } }

    public bool InInventory
    {
        get { return inInventory; }
        set {
            inInventory = value;
            m_RectTransform.localPosition = Vector3.zero;
            m_RectTransform.localScale = Vector3.one;
            ResetSpriteSize(m_RectTransform, 45, 45);
        }
    }

    private void Awake()
    {
        FindInit();
    }

    void Update()
    {
        // Click mouse button 1 and split materials
        if (Input.GetMouseButtonDown(1) && isDrag == true)
        {
            SplitMaterials();
        }
    }

    // Update the UI value
    public void UpdateUI(float value)
    {
        if(value <= 0)
        {
            gameObject.GetComponent<Transform>().parent.GetComponent<ToolBarSlotController>().Normal();
            GameObject.Destroy(gameObject);
        }
        m_Bar.fillAmount = value;
    }

    // Initialization of Find
    private void FindInit()
    {
        m_RectTransform = gameObject.GetComponent<RectTransform>();
        m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
        m_Image = gameObject.GetComponent<Image>();
        m_Text = m_RectTransform.Find("Num").gameObject.GetComponent<Text>();
        m_Bar = m_RectTransform.Find("Bar").gameObject.GetComponent<Image>();

        gameObject.name = "InventoryItem";
        //parent = m_RectTransform.parent.parent.parent.parent;
        parent = GameObject.Find("Canvas").GetComponent<Transform>();
    }

    // Initiate items
    public void initItem(int id, string name, int num, int bar)
    {
        this.id = id;
        m_Image.sprite = Resources.Load<Sprite>("Item/" + name);
        this.num = num;
        this.bar = bar;
        m_Text.text = num.ToString();
        BarOrNumber();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        self_parent = m_RectTransform.parent;
        m_RectTransform.SetParent(parent);
        m_CanvasGroup.blocksRaycasts = false;
        isDrag = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_RectTransform, 
            eventData.position, eventData.enterEventCamera, out position);
        m_RectTransform.position = position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;

        ItemDrag(target);

        // Reset attributes
        m_CanvasGroup.blocksRaycasts = true;
        m_RectTransform.localPosition = Vector3.zero;
        isDrag = false;
    }

    // Reset the size of sprite when move the item
    private void ResetSpriteSize(RectTransform rectTransform, float width, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
        m_RectTransform.localScale = Vector3.one;
    }

    // Material split
    private void SplitMaterials()
    {
        // Copy the item you drag
        GameObject temp = GameObject.Instantiate<GameObject>(gameObject);
        RectTransform tempTransform = temp.GetComponent<RectTransform>();
        tempTransform.SetParent(self_parent);
        tempTransform.localPosition = Vector3.zero;
        tempTransform.localScale = Vector3.one;

        // Split number
        int tempCount = num;
        int tempB = tempCount / 2;
        int tempA = tempCount - tempB;

        //Update number
        temp.GetComponent<InventoryItemController>().Num = tempB;
        Num = tempA;

        temp.GetComponent<CanvasGroup>().blocksRaycasts = true;
        temp.GetComponent<InventoryItemController>().Id = Id;
    }

    private void MergeMaterials(InventoryItemController target)
    {
        target.Num = target.Num + Num;
        RectTransform targetTransform = target.GetComponent<RectTransform>();
        targetTransform.SetParent(self_parent);
        targetTransform.localPosition = Vector3.zero;
        targetTransform.localScale = Vector3.one;
        GameObject.Destroy(gameObject);
    }

    // Conditions when drag items
    private void ItemDrag(GameObject target)
    {
        if (target != null)
        {
            #region Movement in inventory slot
            // Put in the specified location of inventory slot
            if (target.tag == "InventorySlot")
            {
                m_RectTransform.SetParent(target.transform);
                ResetSpriteSize(m_RectTransform, 45, 45);
                inInventory = true;
            }
            // Put back 
            if (target.tag != "InventorySlot")
            {
                m_RectTransform.SetParent(self_parent);
            }
            #endregion

            #region Exchange item
            if (target.tag == "InventoryItem")
            {
                InventoryItemController iic = target.GetComponent<InventoryItemController>();
                if (inInventory && iic.InInventory)
                {
                    if (Id == iic.Id)
                    {
                        MergeMaterials(iic);
                    }
                    else
                    {
                        Transform tempTransform = target.GetComponent<Transform>();
                        m_RectTransform.SetParent(tempTransform.parent);
                        tempTransform.SetParent(self_parent);
                        tempTransform.localPosition = Vector3.zero;
                    }
                }
                else
                {
                    if (Id == iic.Id && iic.inInventory)
                    {
                        MergeMaterials(iic);
                    }
                }
            }
            #endregion

            #region Put into the specified location of crafting slot
            if (target.tag == "CraftingSlot")
            {
                if (target.GetComponent<CraftingSlotController>().IsOpen)
                {
                    if (id == target.GetComponent<CraftingSlotController>().Id)
                    {
                        m_RectTransform.SetParent(target.transform);
                        ResetSpriteSize(m_RectTransform, 40, 40);
                        inInventory = false;
                        // Inform the controller of crafting panel
                        InventoryPanelController.Instance.SendDragMaterialsItem(gameObject);
                    }
                    else
                    {
                        m_RectTransform.SetParent(self_parent);
                    }
                }
                else
                {
                    m_RectTransform.SetParent(self_parent);
                }
            }
            #endregion
        }
        else
        {
            // Reset
            m_RectTransform.SetParent(self_parent);
        }
    }

    private void BarOrNumber()
    {
        if(bar == 0)
        {
            m_Bar.gameObject.SetActive(false);
        }
        else
        {
            m_Text.gameObject.SetActive(false);
        }
    }
}
