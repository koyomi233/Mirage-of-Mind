using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform Model
/// </summary>
public class Platform : MonoBehaviour
{
    private Transform m_Transform;

    private bool canPut = true;                         // Whether the model can put at the current position
    private bool attach = false;                        // Whether models can attach to each other

    public bool CanPut { get { return canPut; } }
    public bool Attach { get { return attach; } set { attach = value; } }

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        if (canPut)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void Normal()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Terrain")
        {
            canPut = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag != "Terrain")
        {
            canPut = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag != "Terrain")
        {
            canPut = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Platform")
        {
            attach = true;
            m_Transform.position = other.gameObject.GetComponent<Transform>().position + new Vector3(3.3f, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            attach = false;
        }
    }
}
