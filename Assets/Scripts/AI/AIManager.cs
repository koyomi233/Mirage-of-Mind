using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIManagerType
{
    BOAR,
    CANNIBAL,
    NULL
}

public enum AIType
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
    private Transform[] posTransform;
    private List<Vector3> posList = new List<Vector3>();

    private int index = 0;

    public AIManagerType AIManagerType { get { return aiManagerType; } set { aiManagerType = value; } }

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        prefab_Boar = Resources.Load<GameObject>("AI/Boar");
        prefab_Cannibal = Resources.Load<GameObject>("AI/Cannibal");
        posTransform = m_Transform.GetComponentsInChildren<Transform>(true);
        GameObject.Find("FPSController").GetComponent<PlayerController>().DeathDelegate += Death;

        for (int i = 1; i < posTransform.Length; i++)
        {
            posList.Add(posTransform[i].position);
        }

        CreateAIByEnum();
    }

    private void CreateAIByEnum()
    {
        if(aiManagerType == global::AIManagerType.BOAR)
        {
            CreateAI(prefab_Boar, AIType.BOAR);
        }
        else if(aiManagerType == global::AIManagerType.CANNIBAL)
        {
            CreateAI(prefab_Cannibal, AIType.CANNIBAL);
        }
    }

    private void CreateAI(GameObject prefab_AI, AIType aiType)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject ai = GameObject.Instantiate<GameObject>(prefab_AI, m_Transform.position, Quaternion.identity, m_Transform);
            ai.GetComponent<AI>().Dir = posList[i];
            ai.GetComponent<AI>().PosList = posList;
            ai.GetComponent<AI>().Life = 300;
            ai.GetComponent<AI>().Attack = 100;
            ai.GetComponent<AI>().M_AIType = aiType;

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
            ai.GetComponent<AI>().M_AIType = AIType.BOAR;
        }
        else if (aiManagerType == global::AIManagerType.CANNIBAL)
        {
            ai = GameObject.Instantiate<GameObject>(prefab_Cannibal, m_Transform.position, Quaternion.identity, m_Transform);
            ai.GetComponent<AI>().M_AIType = AIType.CANNIBAL;
        }

        ai.GetComponent<AI>().Dir = posList[index];
        ai.GetComponent<AI>().PosList = posList;
        ai.GetComponent<AI>().Life = 600;
        ai.GetComponent<AI>().Attack = 100;

        index++;
        index = index % posList.Count;

        AIList.Add(ai);
    }

    // Kill all AI when game over
    private void Death()
    {
        for(int i = 0; i < AIList.Count; i++)
        {
            GameObject.Destroy(AIList[i]);
        }
    }
}
