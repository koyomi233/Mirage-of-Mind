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

    public void InitContent(int index, GameObject prefab)
    {
        this.index = index;
        gameObject.name = "Content" + index;
        CreateAllItems(index, prefab);
    }

    private void CreateAllItems(int count, GameObject prefab)   
    {
        for(int i = 0; i < count; i++)
        {
            GameObject.Instantiate<GameObject>(prefab, m_Transform);
        }
    }
}
