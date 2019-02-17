using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Content Controller
/// </summary>

public class CraftingContentController : MonoBehaviour
{
    private Transform m_Transform;

    private int index = -1;

    void Awake ()
    {
        m_Transform = gameObject.GetComponent<Transform>();
    }

    public void InitContent(int index, GameObject prefab, List<string> strList)
    {
        this.index = index;
        gameObject.name = "Content" + index;
        CreateAllItems(prefab, strList);
    }

    private void CreateAllItems(GameObject prefab, List<string> strList)   
    {
        for(int i = 0; i < strList.Count; i++)
        {
            GameObject run = GameObject.Instantiate<GameObject>(prefab, m_Transform);
            run.GetComponent<CraftingContentItemController>().Init(strList[i]);
        }
    }
}
