using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    private Transform m_Transform;
    private Image m_Image;
    private Button m_CraftBtn;
    private Button m_CraftAllBtn;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Image = m_Transform.Find("GoodItem/ItemImage").GetComponent<Image>();
        m_CraftBtn = m_Transform.Find("Craft").GetComponent<Button>();
        m_CraftAllBtn = m_Transform.Find("CraftAll").GetComponent<Button>();

        m_Image.gameObject.SetActive(false);
    }

    public void Init(string fileName)
    {
        m_Image.gameObject.SetActive(true);
        m_Image.sprite = Resources.Load<Sprite>("Item/" + fileName);
    }
}
