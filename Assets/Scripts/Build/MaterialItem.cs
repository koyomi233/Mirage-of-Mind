using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialItem : MonoBehaviour
{
    private Transform m_Transform;
    private Image icon_Image;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        icon_Image = m_Transform.Find("Icon").GetComponent<Image>();
    }

    public void Highlight()
    {
        icon_Image.color = Color.red;
    }

    public void Normal()
    {
        icon_Image.color = Color.white;
    }
}
