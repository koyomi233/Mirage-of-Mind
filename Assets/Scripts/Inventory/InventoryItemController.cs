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

    private Image m_Image;
    private Text m_Text;
    private int id;

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

    // Initiate items
    public void initItem(int id, string name, int num)
    {
        this.id = id;
        m_Image.sprite = Resources.Load<Sprite>("Item/" + name);
        m_Text.text = num.ToString();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        self_parent = m_RectTransform.parent;
        m_RectTransform.SetParent(parent);
        m_CanvasGroup.blocksRaycasts = false;
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
            }
            // Put back 
            if(target.tag != "InventorySlot")
            {
                m_RectTransform.SetParent(self_parent);
            }
            // Exchange
            if (target.tag == "InventoryItem")
            {
                Transform tempTransform = target.GetComponent<Transform>();
                m_RectTransform.SetParent(tempTransform.parent);
                tempTransform.SetParent(self_parent);
                tempTransform.localPosition = Vector3.zero;
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
    }

    // Reset the size of sprite when move the item
    private void ResetSpriteSize(RectTransform rectTransform, float width, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
    }
}
