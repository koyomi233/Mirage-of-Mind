using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    private Transform m_Transform;
    private Image hp_Bar;
    private Image vit_Bar;

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        hp_Bar = m_Transform.Find("HP/Bar").GetComponent<Image>();
        vit_Bar = m_Transform.Find("VIT/Bar").GetComponent<Image>();
    }

    public void SetHP(int hp)
    {
        hp_Bar.fillAmount = hp * 0.001f;
    }

    public void SetVIT(int vit)
    {
        vit_Bar.fillAmount = vit * 0.01f;
    }
}
