using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform m_Transform;
    private GameObject m_BuildingPlan;
    private GameObject m_WoodSpear;

    private GameObject currentWeapon;
    private GameObject targetWeapon;

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_BuildingPlan = m_Transform.Find("PersonCamera/Building Plan").gameObject;
        m_WoodSpear = m_Transform.Find("PersonCamera/Wooden Spear").gameObject;

        m_WoodSpear.SetActive(false);

        currentWeapon = m_BuildingPlan;
        targetWeapon = null;
    }

    void Update()
    {
        // Use map
        if (Input.GetKeyDown(KeyCode.M))
        {
            targetWeapon = m_BuildingPlan;
            Changed();
        }
        // Use spear
        if (Input.GetKeyDown(KeyCode.K))
        {
            targetWeapon = m_WoodSpear;
            Changed();
        }
    }

    private void Changed()
    {
        currentWeapon.GetComponent<Animator>().SetTrigger("Holster");
        StartCoroutine("DelayTime");
    }

    IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(1);
        currentWeapon.SetActive(false);
        targetWeapon.SetActive(true);
        currentWeapon = targetWeapon;
    }
}
