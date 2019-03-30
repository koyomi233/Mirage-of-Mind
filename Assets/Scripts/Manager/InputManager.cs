using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InputManager : MonoBehaviour
{
    private bool inventoryState = false;

    private FirstPersonController m_FirstPersonController;

    private void Start()
    {
        InventoryPanelController.Instance.UIPanelHide();
        FindInit();
    }

    void Update()
    {
        InventoryPanelKey();
        if(inventoryState == false)
        {
            ToolBarPanelKey();
        }   
    }

    private void FindInit()
    {
        m_FirstPersonController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
    }

    // Key to control backpack
    private void InventoryPanelKey()
    {
        if (Input.GetKeyDown(GameConst.InventoryPanelKey))
        {
            if (inventoryState)                 // Close pack
            {
                inventoryState = false;
                InventoryPanelController.Instance.UIPanelHide();
                m_FirstPersonController.enabled = true;
                //m_GunControllerBase.enabled = true;
                //frontSight.SetActive(true);
                if (ToolBarPanelController.Instance.CurrentActiveModel != null)
                    ToolBarPanelController.Instance.CurrentActiveModel.SetActive(true);
            }
            else                                // Open pack
            {
                inventoryState = true;
                InventoryPanelController.Instance.UIPanelShow();
                m_FirstPersonController.enabled = false;
                if (ToolBarPanelController.Instance.CurrentActiveModel != null)
                    ToolBarPanelController.Instance.CurrentActiveModel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //m_GunControllerBase.enabled = false;
                //frontSight.SetActive(false);
            }
        }
    }

    // Key to control tool bar
    private void ToolBarPanelKey()
    {
        ToolBarKey(GameConst.ToolBarPanelKey_1, 0);
        ToolBarKey(GameConst.ToolBarPanelKey_2, 1);
        ToolBarKey(GameConst.ToolBarPanelKey_3, 2);
        ToolBarKey(GameConst.ToolBarPanelKey_4, 3);
        ToolBarKey(GameConst.ToolBarPanelKey_5, 4);
        ToolBarKey(GameConst.ToolBarPanelKey_6, 5);
        ToolBarKey(GameConst.ToolBarPanelKey_7, 6);
        ToolBarKey(GameConst.ToolBarPanelKey_8, 7);
        ToolBarKey(GameConst.ToolBarPanelKey_9, 8);
    }

    private void ToolBarKey(KeyCode keycode, int keyNum)
    {
        if (Input.GetKeyDown(keycode))
        {
            ToolBarPanelController.Instance.SaveActiveSlotByKey(keyNum);
        }
    }
}
