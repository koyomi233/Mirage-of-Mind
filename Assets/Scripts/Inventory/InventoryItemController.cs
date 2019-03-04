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
    private Transform parent;
    private Transform self_parent;
    private CanvasGroup m_CanvasGroup;

    private Image m_Image;                         // item icon
    private Text m_Text;                           // item number
    private int id;                                // self id
    private int num = 0;                           
    private bool isDrag = false;                   // drag status
    private bool inInventory = true;               // whether in inventory

    public int Num
    {
        get { return num; }
        set {
            num = value;
            m_Text.text = num.ToString();
        }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

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
        m_RectTransform = gameObject.GetComponent<RectTransform>();
        m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
        m_Image = gameObject.GetComponent<Image>();
        m_Text = m_RectTransform.Find("Num").gameObject.GetComponent<Text>();

        gameObject.name = "InventoryItem";
        //parent = m_RectTransform.parent.parent.parent.parent;
        parent = GameObject.Find("InventoryPanel").GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isDrag == true)
        {
            SplitMaterials();
        }
    }

    // Initiate items
    public void initItem(int id, string name, int num)
    {
        this.id = id;
        m_Image.sprite = Resources.Load<Sprite>("Item/" + name);
        this.num = num;
        m_Text.text = num.ToString();
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
        if(target != null)
        {
            // Put in the specified location of inventory slot
            if(target.tag == "InventorySlot")
            {
                m_RectTransform.SetParent(target.transform);
                ResetSpriteSize(m_RectTransform, 45, 45);
                inInventory = true;
            }
            // Put back 
            if(target.tag != "InventorySlot")
            {
                m_RectTransform.SetParent(self_parent);
            }
            // Exchange
            if (target.tag == "InventoryItem")
            {
                if (inInventory && target.GetComponent<InventoryItemController>().InInventory)
                {
                    if(Id == target.GetComponent<InventoryItemController>().Id)
                    {
                        MergeMaterials(target.GetComponent<InventoryItemController>());
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
                    if (Id == target.GetComponent<InventoryItemController>().Id && 
                        target.GetComponent<InventoryItemController>().inInventory)
                    {
                        MergeMaterials(target.GetComponent<InventoryItemController>());
                    }
                }
            }
            // Put in the specified location of crafting slot
            if (target.tag == "CraftingSlot")
            {
                if (target.GetComponent<CraftingSlotController>().IsOpen)
                {
                    if(id == target.GetComponent<CraftingSlotController>().Id)
                    {
                        m_RectTransform.SetParent(target.transform);
                        ResetSpriteSize(m_RectTransform, 40, 40);
                        m_RectTransform.localScale = Vector3.one;
                        inInventory = false;
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
        }
        else
        {
            // Reset
            m_RectTransform.SetParent(self_parent);
        }
        m_CanvasGroup.blocksRaycasts = true;
        m_RectTransform.localPosition = Vector3.zero;
        isDrag = false;
    }

    // Reset the size of sprite when move the item
    private void ResetSpriteSize(RectTransform rectTransform, float width, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
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
}
