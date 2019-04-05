using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIManagerType
{
    BOAR,
    CANNIBAL,
    NULL
}

public class AIManager : MonoBehaviour
{
    private Transform m_Transform;
    private GameObject prefab_Boar;
    private GameObject prefab_Cannibal;
    private AIManagerType aiManagerType = AIManagerType.NULL;
    private List<GameObject> AIList = new List<GameObject>();

    public AIManagerType AIManagerType { get { return aiManagerType; } set { aiManagerType = value; } }

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        prefab_Boar = Resources.Load<GameObject>("AI/Boar");
        prefab_Cannibal = Resources.Load<GameObject>("AI/Cannibal");

        CreateAIByEnum();
    }

    private void CreateAIByEnum()
    {
        if(aiManagerType == global::AIManagerType.BOAR)
        {
            CreateAI(prefab_Boar);
        }
        else if(aiManagerType == global::AIManagerType.CANNIBAL)
        {
            CreateAI(prefab_Cannibal);
        }
    }

    private void CreateAI(GameObject prefab_AI)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject ai = GameObject.Instantiate<GameObject>(prefab_AI, m_Transform.position, Quaternion.identity, m_Transform);
            AIList.Add(ai);
        }
    }

    private void AIDeath(GameObject ai)
    {
        AIList.Remove(ai);
        StartCoroutine("CreateOneAI");
    }

    private IEnumerator CreateOneAI()
    {
        GameObject ai = null;
        yield return new WaitForSeconds(3);
        if (aiManagerType == global::AIManagerType.BOAR)
        {
            ai = GameObject.Instantiate<GameObject>(prefab_Boar, m_Transform.position, Quaternion.identity, m_Transform);
        }
        else if (aiManagerType == global::AIManagerType.CANNIBAL)
        {
            ai = GameObject.Instantiate<GameObject>(prefab_Cannibal, m_Transform.position, Quaternion.identity, m_Transform);
        }
        AIList.Add(ai);
    }
}
