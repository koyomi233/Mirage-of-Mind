using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// JSON tools class
/// </summary>
public sealed class JsonTools : MonoBehaviour
{
    // Load JSON file through genericity
    public static List<T> LoadJsonFile<T>(string fileName)
    {
        List<T> tempList = new List<T>();
        string tempJsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;

        JsonData jsonData = JsonMapper.ToObject(tempJsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            T item = JsonMapper.ToObject<T>(jsonData[i].ToJson());
            tempList.Add(item);
        }

        return tempList;
    }
}
