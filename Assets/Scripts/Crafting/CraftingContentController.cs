using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Content Controller
/// </summary>

public class CraftingContentController : MonoBehaviour
{
    private Transform m_Transform;

    private CraftingContentItemController current = null;

    private int index = -1;

    public CraftingContentItemController CraftingContentItemController
    {
        get => default;
        set
        {
        }
    }

    void Awake ()
    {
        m_Transform = gameObject.GetComponent<Transform>();
    }

    public void InitContent(int index, GameObject prefab, List<CraftingContentItem> strList)
    {
        this.index = index;
        gameObject.name = "Content" + index;
        CreateAllItems(prefab, strList);
    }

    private void CreateAllItems(GameObject prefab, List<CraftingContentItem> strList)   
    {
        for(int i = 0; i < strList.Count; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(prefab, m_Transform);
            run.GetComponent<CraftingContentItemController>().Init(strList[i]);
        }
    }

    // Switch item state of content
    private void ResetItemState(CraftingContentItemController item)
    {
        if (item == current) return;
        Debug.Log(item.Id);
        if(current != null)
        {
            current.NormalItem();
        }
        item.ActiveItem();
        current = item;
        SendMessageUpwards("CreateSlotContents", item.Id);
    }
}
