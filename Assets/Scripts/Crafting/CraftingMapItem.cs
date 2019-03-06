using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JSON file: CraftingMapJsonData
/// </summary>
public class CraftingMapItem 
{
    private int mapId;                    // Map ID
    private string[] mapContents;         // Map contents
    private int mapCount;                 // Number of materials needed
    private string mapName;               // Map name

    public int MapId
    {
        get { return mapId; }
        set { mapId = value; }
    }

    public string[] MapContents
    {
        get { return mapContents; }
        set { mapContents = value; }
    }

    public int MaterialsCount
    {
        get { return mapCount; }
        set { mapCount = value; }
    }

    public string MapName
    {
        get { return mapName; }
        set { mapName = value; }
    }

    public CraftingMapItem(int mapId, string[] mapContents, int mapCount, string mapName)
    {
        this.mapId = mapId;
        this.mapContents = mapContents;
        this.mapName = mapName;
        this.mapCount = mapCount;
    }

    public override string ToString()
    {
        return string.Format("ID: {0}, Map: {1}, Name: {2}, Count: {3}", this.mapId, this.mapContents.Length, this.mapName, this.mapCount);
    }
}
