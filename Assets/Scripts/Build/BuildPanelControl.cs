using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelControl : MonoBehaviour
{
    private Transform m_Transform;
    private Transform BG_Transform;
    private GameObject prefab_Item;
    private GameObject prefab_Material;
    private Text itemName;
    private List<Sprite> icons = new List<Sprite>();

    private bool showUI = true;
    private List<Item> itemList = new List<Item>();
    private float scrollNum = 90000.0f;                                    // Record the number of scroll number
    private int index = 0;
    private Item currentItem = null;
    private Item targetItem = null;

    private string[] itemNames = new string[] { "", "[Others]", "[Roof]", "[Stairs]", "[Window]", "[Door]", "[Wall]", "[Floor]", "[Foundation]" };
    private List<Sprite[]> materialIcons = new List<Sprite[]>();
    private int zIndex = 20;                                               // Initialized rotation of material UI 

    private List<string[]> materialIconName = new List<string[]>();

    private void Start()
    {
        Init();
        LoadIcons();
        LoadMaterialIcons();
        SetMaterialIconName();
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
                MouseScrollWheel();
            }
        }
    }

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        BG_Transform = m_Transform.Find("WheelBG");
        prefab_Item = Resources.Load<GameObject>("Build/Prefab/Item");
        prefab_Material = Resources.Load<GameObject>("Build/Prefab/MaterialBG");
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

    private void LoadMaterialIcons()
    {
        materialIcons.Add(null);
        materialIcons.Add(new Sprite[] { MaterialIcon("Ceiling Light"), MaterialIcon("Pillar_Wood"), MaterialIcon("Wooden Ladder") });
        materialIcons.Add(new Sprite[] { null, MaterialIcon("Roof_Metal"), null });
        materialIcons.Add(new Sprite[] { MaterialIcon("Stairs_Wood"), MaterialIcon("L Shaped Stairs_Wood"), null });
        materialIcons.Add(new Sprite[] { null, MaterialIcon("Window_Wood"), null });
        materialIcons.Add(new Sprite[] { null, MaterialIcon("Wooden Door"), null });
        materialIcons.Add(new Sprite[] { MaterialIcon("Wall_Wood"), MaterialIcon("Doorway_Wood"), MaterialIcon("Window Frame_Wood") });
        materialIcons.Add(new Sprite[] { null, MaterialIcon("Floor_Wood"), null });
        materialIcons.Add(new Sprite[] { null, MaterialIcon("Platform_Wood"), null });
    }

    // Set the name of icons
    private void SetMaterialIconName()
    {
        materialIconName.Add(null);
        materialIconName.Add(new string[] { "Ceiling Light", "Pillar Wood", "Wooden Ladder" });
        materialIconName.Add(new string[] { "", "Roof Metal", "" });
        materialIconName.Add(new string[] { "Stairs Wood", "L Shaped Stairs Wood", "" });
        materialIconName.Add(new string[] { "", "Window Wood", "" });
        materialIconName.Add(new string[] { "", "Wooden Door", "" });
        materialIconName.Add(new string[] { "Wall Wood", "Doorway Wood", "Window Frame Wood" });
        materialIconName.Add(new string[] { "", "Floor Wood", "" });
        materialIconName.Add(new string[] { "", "Platform Wood", "" });

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
                for(int j = 0; j < materialIcons[i].Length; j++)
                {
                    zIndex += 13;
                    if(materialIcons[i][j] != null)
                    {
                        GameObject material = GameObject.Instantiate<GameObject>(prefab_Material, BG_Transform);
                        material.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, zIndex));
                        material.GetComponent<Transform>().Find("Icon").GetComponent<Image>().sprite = materialIcons[i][j];
                        material.GetComponent<Transform>().Find("Icon").GetComponent<Transform>().rotation = Quaternion.Euler(Vector3.zero);
                        material.GetComponent<Transform>().SetParent(item.GetComponent<Transform>());
                        item.GetComponent<Item>().MaterialListAdd(material);
                    }
                }
                item.GetComponent<Item>().HideIconBG();
            }
        }
        currentItem = itemList[0];
        SetTextValue();
    }

    // Mouse right click operation
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

    // Mouse scroll wheel operation
    private void MouseScrollWheel()
    {
        scrollNum += Input.GetAxis("Mouse ScrollWheel") * 5;
        index = Mathf.Abs((int)scrollNum);

        targetItem = itemList[index % itemList.Count];
        if (targetItem != currentItem)
        {
            targetItem.ShowIconBG();
            currentItem.HideIconBG();
            currentItem = targetItem;
            SetTextValue();
        }
    }

    private void SetTextValue()
    {
        itemName.text = itemNames[index % itemNames.Length];
    }

    // Load a particular material icon by name
    private Sprite MaterialIcon(string name)
    {
        return Resources.Load<Sprite>("Build/MaterialIcon/" + name);
    }
}
