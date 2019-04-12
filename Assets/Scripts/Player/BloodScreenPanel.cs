using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreenPanel : MonoBehaviour
{
    private Transform m_Transform;
    private Image m_Image;

    private byte alpha = 0;

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Image = gameObject.GetComponent<Image>();
    }

    public void SetImageAlpha()
    {
        alpha += 15;
        Color32 color = new Color32(255, 255, 255, alpha);
        m_Image.color = color;
    }
}
