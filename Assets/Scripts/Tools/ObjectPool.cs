using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pool
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> pool = null;

    private void Awake()
    {
        pool = new Queue<GameObject>();
    }

    public void AddObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public GameObject GetObject()
    {
        GameObject temp = null;
        if (pool.Count > 0)
        {
            temp = pool.Dequeue();
            temp.SetActive(true);
        }
        return temp;
    }

    public bool Data()
    {
        if (pool.Count > 0)
            return true;
        else
            return false;
    }
}
