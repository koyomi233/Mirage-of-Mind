using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool inventoryState = false;

    private void Start()
    {
        InventoryPanelController.Instance.UIPanelHide();
    }

    void Update()
    {
        InventoryPanelKey();
        ToolBarPanelKey();
    }

    // Key to control backpack
    private void InventoryPanelKey()
    {
        if (Input.GetKeyDown(GameConst.InventoryPanelKey))
        {
            if (inventoryState)
            {
                inventoryState = false;
                InventoryPanelController.Instance.UIPanelHide();
            }
            else
            {
                inventoryState = true;
                InventoryPanelController.Instance.UIPanelShow();
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
