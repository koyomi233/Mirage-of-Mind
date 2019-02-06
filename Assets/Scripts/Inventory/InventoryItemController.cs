using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Item controller
/// </summary>

public class InventoryItemController : MonoBehaviour
{
    private Transform m_Transform;

    private Image m_Image;
    private Text m_Text;

    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Image = gameObject.GetComponent<Image>();
        m_Text = m_Transform.Find("Num").gameObject.GetComponent<Text>();
    }

    // Initiate items
    public void initItem(string name, int num)
    {
        m_Image.sprite = Resources.Load<Sprite>("Item/" + name);
        m_Text.text = num.ToString();
    }
}
