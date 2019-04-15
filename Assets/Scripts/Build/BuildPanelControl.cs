using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelControl : MonoBehaviour
{
    private Transform m_Transform;
    private Transform BG_Transform;
    private GameObject prefab_Item;
    private Text itemName;
    private List<Sprite> icons = new List<Sprite>();

    private bool showUI = true;
    private List<Item> itemList = new List<Item>();
    private float scrollNum = 90000.0f;                                    // Record the number of scroll number
    private int index = 0;
    private Item currentItem = null;
    private Item targetItem = null;

    private string[] itemNames = new string[] { "", "Others", "Roof", "Stairs", "Window", "Door", "Wall", "Floor", "Foundation" };

    private void Start()
    {
        Init();
        LoadIcons();
        CreateItems();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShowOrHidePanel();
        }

        if (showUI)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                scrollNum += Input.GetAxis("Mouse ScrollWheel") * 5;
                index = Mathf.Abs((int)scrollNum);

                targetItem = itemList[index % itemList.Count];
                if(targetItem != currentItem)
                {
                    targetItem.ShowIconBG();
                    currentItem.HideIconBG();
                    currentItem = targetItem;
                    SetTextValue();
                }
            }
        }
    }

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        BG_Transform = m_Transform.Find("WheelBG");
        prefab_Item = Resources.Load<GameObject>("Build/Prefab/Item");
        itemName = m_Transform.Find("WheelBG/ItemName").GetComponent<Text>();
    }

    private void LoadIcons()
    {
        icons.Add(null);
        icons.Add(Resources.Load<Sprite>("Build/Icon/Question Mark"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Roof_Category"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Stairs_Category"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Window_Category"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Door_Category"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Wall_Category"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Floor_Category"));
        icons.Add(Resources.Load<Sprite>("Build/Icon/Foundation_Category"));
    }

    private void CreateItems()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject item = GameObject.Instantiate<GameObject>(prefab_Item, BG_Transform);
            itemList.Add(item.GetComponent<Item>());
            if (icons[i] == null)
            {
                item.GetComponent<Item>().Init("Item", Quaternion.Euler(new Vector3(0, 0, i * 40)), false, null, true);
            }
            else
            {
                item.GetComponent<Item>().Init("Item", Quaternion.Euler(new Vector3(0, 0, i * 40)), true, icons[i], false);
            }
        }
        currentItem = itemList[0];
        SetTextValue();
    }

    private void ShowOrHidePanel()
    {
        if (showUI)
        {
            BG_Transform.gameObject.SetActive(false);
            showUI = false;
        }
        else
        {
            BG_Transform.gameObject.SetActive(true);
            showUI = true;
        }
    }

    private void SetTextValue()
    {
        itemName.text = itemNames[index % itemNames.Length];
    }
}
