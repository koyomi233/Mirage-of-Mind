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

    // Main menu (Item)
    private int index = 0;
    private Item currentItem = null;
    private Item targetItem = null;
    private float scrollNum = 90000.0f;                                    // Record the number of scroll number

    // Secondary menu (Material)
    private int index_Material = 0;
    private MaterialItem currentMaterial = null;
    private MaterialItem targetMaterial = null;
    private float scrollNum_Material = 0.0f;                               // Record the number of scroll number

    private string[] itemNames = new string[] {
        "", "[Others]", "[Roof]", "[Stairs]", "[Window]",
        "[Door]", "[Wall]", "[Floor]", "[Foundation]"
    };
    private List<Sprite[]> materialIcons = new List<Sprite[]>();
    private int zIndex = 20;                                               // Initialized rotation of material UI 

    private List<string[]> materialIconName = new List<string[]>();
    private List<GameObject[]> materialModels = new List<GameObject[]>();

    private bool isItemControl = true;                                     // Control item or material

    // Build model control
    private Transform player_Transform;
    private GameObject tempBuildModel = null;                              // Prefab
    private GameObject buildModel = null;                                  // Model
    private Camera envCamera = null;
    private Ray ray;
    private RaycastHit hit;
    
    private void Start()
    {
        Init();
        LoadIcons();
        LoadMaterialIcons();
        SetMaterialIconName();
        LoadMaterialsModels();
        CreateItems();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(isItemControl == false)
            {
                currentMaterial.Normal();
                isItemControl = true;
            }
            else
            {
                ShowOrHidePanel();
            }
        }

        // Mouse ScrollWheel for main menu
        if (showUI && isItemControl)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                MouseScrollWheelItem();
            }
        }

        // Mouse ScrollWheel for secondary menu
        if (showUI && isItemControl == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                MouseScrollWheelMaterial();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (targetItem == null) return;
            if (targetItem.materialList.Count == 0)
            {
                SetLeftKeyNull();
                HideUI();
                return;
            }
            if (tempBuildModel == null) isItemControl = false;

            if(tempBuildModel != null && showUI)
            {
                HideUI();
            }

            // Check if the model can be placed
            if (buildModel != null && buildModel.GetComponent<Platform>().CanPut == false) return;

            if (buildModel != null && buildModel.GetComponent<Platform>().CanPut == true)
            {
                buildModel.GetComponent<Platform>().Normal();
                GameObject.Destroy(buildModel.GetComponent<Platform>());
            }

            // Create build model
            if (tempBuildModel != null)
            {
                buildModel = GameObject.Instantiate<GameObject>(tempBuildModel, player_Transform.position + new Vector3(0, 0, 10), Quaternion.identity);
                isItemControl = true;
            }
        }

        SetModelPosition();
    }

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        BG_Transform = m_Transform.Find("WheelBG");
        prefab_Item = Resources.Load<GameObject>("Build/Prefab/Item");
        prefab_Material = Resources.Load<GameObject>("Build/Prefab/MaterialBG");
        itemName = m_Transform.Find("WheelBG/ItemName").GetComponent<Text>();
        player_Transform = GameObject.Find("FPSController").GetComponent<Transform>();
        envCamera = GameObject.Find("EnvCamera").GetComponent<Camera>();
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
        materialIconName.Add(new string[] { "Roof Metal" });
        materialIconName.Add(new string[] { "Stairs Wood", "L Shaped Stairs Wood" });
        materialIconName.Add(new string[] { "Window Wood" });
        materialIconName.Add(new string[] { "Wooden Door" });
        materialIconName.Add(new string[] { "Wall Wood", "Doorway Wood", "Window Frame Wood" });
        materialIconName.Add(new string[] { "Floor Wood" });
        materialIconName.Add(new string[] { "Platform Wood" });

    }

    // Load material models
    private void LoadMaterialsModels()
    {
        materialModels.Add(null);
        materialModels.Add(new GameObject[] { BuildModel("Ceiling_Light"), BuildModel("Pillar"), BuildModel("Ladder") });
        materialModels.Add(new GameObject[] { BuildModel("Roof") });
        materialModels.Add(new GameObject[] { BuildModel("Stairs"), BuildModel("L_Shaped_Stairs") });
        materialModels.Add(new GameObject[] { BuildModel("Window") });
        materialModels.Add(new GameObject[] { BuildModel("Door") });
        materialModels.Add(new GameObject[] { BuildModel("Wall"), BuildModel("Doorway"), BuildModel("Window_Frame") });
        materialModels.Add(new GameObject[] { BuildModel("Floor") });
        materialModels.Add(new GameObject[] { BuildModel("Platform") });
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
            if (tempBuildModel != null) tempBuildModel = null;
            if (targetMaterial != null) targetMaterial.Normal();
        }
    }

    // Hide UI panel 
    private void HideUI()
    {
        BG_Transform.gameObject.SetActive(false);
        showUI = false;
    }

    // Mouse scroll wheel operation for main menu
    private void MouseScrollWheelItem()
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

    // Mouse scroll wheel operation for secondary menu
    private void MouseScrollWheelMaterial()
    {
        scrollNum_Material += Input.GetAxis("Mouse ScrollWheel") * 5;
        index_Material = Mathf.Abs((int)scrollNum_Material);

        targetItem = itemList[index % itemList.Count];
        targetMaterial = targetItem.materialList[index_Material % targetItem.materialList.Count].GetComponent<MaterialItem>();

        if (targetMaterial != currentMaterial)
        {
            tempBuildModel = materialModels[index % itemList.Count][index_Material % targetItem.materialList.Count];
            targetMaterial.Highlight();
            if(currentMaterial != null)
            {
                currentMaterial.Normal();
            }
            currentMaterial = targetMaterial;
            SetTextValueMaterial();
        }
    }

    // Set the content of items
    private void SetTextValue()
    {
        itemName.text = itemNames[index % itemNames.Length];
    }

    // Set the content of materials
    private void SetTextValueMaterial()
    {
        itemName.text = materialIconName[index % itemList.Count][index_Material % targetItem.materialList.Count];
    }

    // Load a particular material icon by name
    private Sprite MaterialIcon(string name)
    {
        return Resources.Load<Sprite>("Build/MaterialIcon/" + name);
    }

    private GameObject BuildModel(string name)
    {
        return Resources.Load<GameObject>("Build/Prefabs/" + name);
    }

    // Use ray to set models position
    private void SetModelPosition()
    {
        ray = envCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 15, ~(1 << 13)))
        {
            if (buildModel != null)
            {
                if (buildModel.GetComponent<Platform>().Attach == false)
                {
                    buildModel.GetComponent<Transform>().position = hit.point;
                }
                if (Vector3.Distance(hit.point, buildModel.GetComponent<Transform>().position) > 1)
                {
                    buildModel.GetComponent<Platform>().Attach = false;
                }
            }
        }
    }

    // Set buildModel null when click left key
    private void SetLeftKeyNull()
    {
        if (tempBuildModel != null) tempBuildModel = null;
        if(buildModel != null)
        {
            GameObject.Destroy(buildModel);
            buildModel = null;
        }
    }
}
