using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private Transform m_Transform;
    private Image icon;
    private Image icon_BG;
    public List<GameObject> materialList = new List<GameObject>();

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        icon = m_Transform.Find("Icon").GetComponent<Image>();
        icon_BG = gameObject.GetComponent<Image>();
    }

    public void Init(string name, Quaternion quaternion, bool showIcon, Sprite sprite, bool showBG)
    {
        gameObject.name = name;
        m_Transform.rotation = quaternion;
        m_Transform.Find("Icon").rotation = Quaternion.Euler(Vector3.zero);
        icon.enabled = showIcon;
        icon.sprite = sprite;
        icon_BG.enabled = showBG;
    }

    public void ShowIconBG()
    {
        icon_BG.enabled = true;
        ShowAndHide(true);
    }

    public void HideIconBG()
    {
        icon_BG.enabled = false;
        ShowAndHide(false);
    }

    public void MaterialListAdd(GameObject material)
    {
        materialList.Add(material);
    }

    // Show or hide the material list
    private void ShowAndHide(bool flag)
    {
        if (materialList == null) return;
        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i].SetActive(flag);
        }
    }
}
