using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotController : MonoBehaviour
{
    private Transform m_Transform;
    private Image m_Image;

    private bool isOpen = false;
    private int id = -1;

    public bool IsOpen { get { return isOpen; } }
    public int Id { get { return id; } }

    void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Image = m_Transform.Find("Item").GetComponent<Image>();
        m_Image.gameObject.SetActive(false);
        m_Image.gameObject.AddComponent<CanvasGroup>();
    }

    public void Init(Sprite sprite, string id)
    {
        m_Image.gameObject.SetActive(true);
        m_Image.sprite = sprite;

        m_Image.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        isOpen = true;

        this.id = int.Parse(id);
    }

    public void Reset()
    {
        m_Image.gameObject.SetActive(false);
    }
}
