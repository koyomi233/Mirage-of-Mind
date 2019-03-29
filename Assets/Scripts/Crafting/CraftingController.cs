using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    private Transform m_Transform;
    private Transform bg_Transform;
    private Image m_Image;
    private Button m_CraftBtn;
    private Button m_CraftAllBtn;

    private int tempId;
    private string tempSpriteName;

    private GameObject prefab_InventoryItem;

    public GameObject Prefab_InventoryItem { set { prefab_InventoryItem = value; } }

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Image = m_Transform.Find("GoodItem/ItemImage").GetComponent<Image>();
        m_CraftBtn = m_Transform.Find("Craft").GetComponent<Button>();
        m_CraftAllBtn = m_Transform.Find("CraftAll").GetComponent<Button>();
        bg_Transform = m_Transform.Find("GoodItem").GetComponent<Transform>();

        m_CraftBtn.onClick.AddListener(CraftingItem);
        m_Image.gameObject.SetActive(false);

        InitButton();
    }

    public void Init(int id, string fileName)
    {
        m_Image.gameObject.SetActive(true);
        m_Image.sprite = Resources.Load<Sprite>("Item/" + fileName);
        tempId = id;
        tempSpriteName = fileName;
    }

    private void InitButton()
    {
        m_CraftBtn.interactable = false;
        m_CraftBtn.transform.Find("Text").GetComponent<Text>().color = Color.gray;
        m_CraftAllBtn.interactable = false;
        m_CraftAllBtn.transform.Find("Text").GetComponent<Text>().color = Color.gray;
    }

    public void ActiveButton()
    {
        m_CraftBtn.interactable = true;
        m_CraftBtn.transform.Find("Text").GetComponent<Text>().color = Color.white;
    }

    // Generate an item
    private void CraftingItem()
    {
        Debug.Log("合成！！！！！");
        GameObject item = GameObject.Instantiate<GameObject>(prefab_InventoryItem, bg_Transform);
        item.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
        item.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        item.GetComponent<InventoryItemController>().initItem(tempId, tempSpriteName, 1, 1);

        InitButton();

        SendMessageUpwards("CraftingOK");
    }
}
