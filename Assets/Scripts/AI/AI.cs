using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private void Death()
    {
        GameObject.Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Death();
            SendMessageUpwards("AIDeath", gameObject);
        }
    }
}
