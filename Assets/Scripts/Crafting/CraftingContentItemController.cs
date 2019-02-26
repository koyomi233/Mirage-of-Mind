using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingContentItemController : MonoBehaviour
{
    private Transform m_Transform;
    private Button m_Button;

    private Text m_Text;
    private GameObject m_BG;

    private int id;
    private string name;

    public int Id { get { return id; } }

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Button = gameObject.GetComponent<Button>();

        m_Text = m_Transform.Find("Text").GetComponent<Text>();
        m_BG = m_Transform.Find("BG").gameObject;
        m_BG.SetActive(false);

        m_Button.onClick.AddListener(ItemButtonClick);
    }


    public void Init(CraftingContentItem item)
    {
        this.id = item.ItemID;
        this.name = item.ItemName;
        gameObject.name = "Item" + id;
        m_Text.text = " " + name;
    }

    public void NormalItem()
    {
        m_BG.SetActive(false);
    }

    public void ActiveItem()
    {
        m_BG.SetActive(true);
    }

    private void ItemButtonClick()
    {
        SendMessageUpwards("ResetItemState", this);
    }
}
