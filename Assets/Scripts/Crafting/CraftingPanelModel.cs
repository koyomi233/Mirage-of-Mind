using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// Crafting Module Model
/// </summary>

public class CraftingPanelModel : MonoBehaviour
{
    Dictionary<int, CraftingMapItem> mapItemDic = null;

    public CraftingMapItem CraftingMapItem
    {
        get => default;
        set
        {
        }
    }

    public CraftingContentItem CraftingContentItem
    {
        get => default;
        set
        {
        }
    }

    void Awake()
    {
        mapItemDic = LoadMapContents("CraftingMapJsonData");
    }

    // Get the name of a tab's icon
    public string[] GetTabsIconName()
    {
        string[] names = new string[] { "Icon_House", "Icon_Weapon" };
        return names;
    }

    // Generate data by getting name
    public List<List<CraftingContentItem>> GetJsonByName(string name)
    {
        List<List<CraftingContentItem>> temp = new List<List<CraftingContentItem>>();
        string jsonStr = Resources.Load<TextAsset>("JsonData/" + name).text;

        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            List<CraftingContentItem> tempList = new List<CraftingContentItem>();
            JsonData jd = jsonData[i]["Type"];
            for (int j = 0; j < jd.Count; j++)
            {
                tempList.Add(JsonMapper.ToObject<CraftingContentItem>(jd[j].ToJson()));
            }
            temp.Add(tempList);
        }
        return temp;
    }

    // Load composite maps through JSON
    private Dictionary<int, CraftingMapItem> LoadMapContents(string name)
    {
        Dictionary<int, CraftingMapItem> temp = new Dictionary<int, CraftingMapItem>();
        string jsonStr = Resources.Load<TextAsset>("JsonData/" + name).text;
        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        for(int i = 0; i < jsonData.Count; i++)
        {
            int mapId = int.Parse(jsonData[i]["MapId"].ToString());
            string tempStr = jsonData[i]["MapContents"].ToString();
            string[] mapContent = tempStr.Split(',');
            int mapCount = int.Parse(jsonData[i]["MaterialsCount"].ToString());
            string mapName = jsonData[i]["MapName"].ToString();

            CraftingMapItem item = new CraftingMapItem(mapId, mapContent, mapCount, mapName);
            temp.Add(mapId, item);
        }

        return temp;
    }

    // Get the corresponding composite map by ID
    public CraftingMapItem GetItemById(int id)
    {
        CraftingMapItem item = null;
        mapItemDic.TryGetValue(id, out item);
        return item;
    }
}
