using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tab Controller
/// </summary>

public class CraftingTabItemController : MonoBehaviour
{
    private Transform m_Transform;
    private Button m_Button;
    private GameObject m_ButtonBG;
    private Image m_Icon;

    private int index = -1;

    void Awake ()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Button = gameObject.GetComponent<Button>();
        m_ButtonBG = m_Transform.Find("Center_BG").gameObject;
        m_Icon = m_Transform.Find("Icon").GetComponent<Image>();

        m_Button.onClick.AddListener(ButtonOnClick);
    }

    // Initialize items
    public void InitItem(int index)
    {
        this.index = index;
        gameObject.name = "Tab" + index;
    }

    // Normal state of tab
    public void NormalTab()
    {
        if (m_ButtonBG.activeSelf == false)
        {
            m_ButtonBG.SetActive(true);
        }
    }

    // Active state of tab
    public void ActiveTab()
    {
        m_ButtonBG.SetActive(false);
    }

    private void ButtonOnClick()
    {
        SendMessageUpwards("ResetTabsAndContents", index);
    }

}
