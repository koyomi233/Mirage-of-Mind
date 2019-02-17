using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// Crafting Module Model
/// </summary>

public class CraftingPanelModel : MonoBehaviour
{
    void Awake()
    {
        
    }

    // Get the name of a tab's icon
    public string[] GetTabsIconName()
    {
        string[] names = new string[] { "Icon_House", "Icon_Weapon" };
        return names;
    }

    // Generate data by getting name
    public List<List<string>> GetJsonByName(string name)
    {
        List<List<string>> temp = new List<List<string>>();
        string jsonStr = Resources.Load<TextAsset>("JsonData/" + name).text;

        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            List<string> tempList = new List<string>();
            JsonData jd = jsonData[i]["Type"];
            for (int j = 0; j < jd.Count; j++)
            {
                tempList.Add(jd[j]["ItemName"].ToString());
            }
            temp.Add(tempList);
        }
        return temp;
    }
}
