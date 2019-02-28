using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resource tool class
/// </summary>
public sealed class ResourcesTools 
{
    // Load Assets from folders
    public static Dictionary<string, Sprite> LoadFolderAssets(string folderName, Dictionary<string, Sprite> dic)
    {
        Sprite[] tempSprite = Resources.LoadAll<Sprite>(folderName);
        for (int i = 0; i < tempSprite.Length; i++)
        {
            dic.Add(tempSprite[i].name, tempSprite[i]);
        }

        return dic;
    }

    // Get Assets by file name
    public static Sprite GetAssets(string fileName, Dictionary<string, Sprite> dic)
    {
        Sprite temp = null;
        dic.TryGetValue(fileName, out temp);
        return temp;
    }
}
