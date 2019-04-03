using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create mark of bullet
/// </summary>
[RequireComponent(typeof(ObjectPool))]
public class BulletMark : MonoBehaviour
{
    private ObjectPool pool;

    private Transform effectParent;                     // Effect resources management

    private Texture2D m_BulletMark;                     // Bullet texture
    private Texture2D m_MainTexture;                    // Model texture
    private Texture2D m_MainTextureBackup_1;            // Back up model texture
    private Texture2D m_MainTextureBackup_2;
    private GameObject prefab_Effect;                   // Effect for bullet

    [SerializeField] private MaterialType materialType;                  // Material of the model

    private Queue<Vector2> bulletMarkQuene = null;      // Queue of bullet mark

    [SerializeField] private int hp;                    // Test

    public int HP
    {
        get { return hp; }
        set {
            hp = value;
            if(hp <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }
    } 

    void Start()
    {
        switch (materialType)
        {
            case MaterialType.Metal:
                ResourcesLoad("Bullet Decal_Metal", "Bullet Impact FX_Metal", "Effect_Metal_Parent");
                break;
            case MaterialType.Stone:
                ResourcesLoad("Bullet Decal_Stone", "Bullet Impact FX_Stone", "Effect_Stone_Parent");
                break;
            case MaterialType.Wood:
                ResourcesLoad("Bullet Decal_Wood", "Bullet Impact FX_Wood", "Effect_Wood_Parent");
                break;
        }

        if (gameObject.GetComponent<ObjectPool>() == null)
        {
            pool = gameObject.AddComponent<ObjectPool>();
        }
        else
        {
            pool = gameObject.GetComponent<ObjectPool>();
        }

        if (gameObject.name == "Conifer")
        {
            m_MainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[2].mainTexture;
        }
        else
        {
            m_MainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        }
        
        m_MainTextureBackup_1 = GameObject.Instantiate<Texture2D>(m_MainTexture);
        m_MainTextureBackup_2 = GameObject.Instantiate<Texture2D>(m_MainTexture);
        gameObject.GetComponent<MeshRenderer>().material.mainTexture = m_MainTextureBackup_1;

        bulletMarkQuene = new Queue<Vector2>();
    }

    private void ResourcesLoad(string bulletMark, string effect, string parent)
    {
        m_BulletMark = Resources.Load<Texture2D>("Gun/BulletMarks/" + bulletMark);
        prefab_Effect = Resources.Load<GameObject>("Effects/Gun/" + effect);
        effectParent = GameObject.Find("TempObject/" + parent).GetComponent<Transform>();
    }

    public void CreateBulletMark(RaycastHit hit)
    {
        // UV coordinate
        Vector2 uv = hit.textureCoord;
        bulletMarkQuene.Enqueue(uv);

        for (int i = 0; i < m_BulletMark.width; i++)                                        // x axis
        {
            for (int j = 0; j < m_BulletMark.height; j++)                                   // y axis
            {
                float x = uv.x * m_MainTextureBackup_1.width - m_BulletMark.width / 2 + i;
                float y = uv.y * m_MainTextureBackup_1.height - m_BulletMark.height / 2 + j;

                // Get the color of bullet mark texture
                Color color = m_BulletMark.GetPixel(i, j);

                // Merge textures of model and bullet
                if(color.a > 0.2f)
                {
                    m_MainTextureBackup_1.SetPixel((int)x, (int)y, color);
                }
            }
        }
        m_MainTextureBackup_1.Apply();

        // Play effect
        PlayEffect(hit);

        Invoke("RemoveBulletMark", 5.0f);
    }

    // Remove marks of bullet
    private void RemoveBulletMark()
    {
        if(bulletMarkQuene.Count > 0)
        {
            Vector2 uv = bulletMarkQuene.Dequeue();
            for (int i = 0; i < m_BulletMark.width; i++)                                        // x axis
            {
                for (int j = 0; j < m_BulletMark.height; j++)                                   // y axis
                {
                    float x = uv.x * m_MainTextureBackup_1.width - m_BulletMark.width / 2 + i;
                    float y = uv.y * m_MainTextureBackup_1.height - m_BulletMark.height / 2 + j;

                    // Get the color of original texture
                    Color color = m_MainTextureBackup_2.GetPixel((int) x, (int) y);

                    m_MainTexture.SetPixel((int)x, (int)y, color);
                }
            }
            m_MainTextureBackup_1.Apply();
        }
    }

    private void PlayEffect(RaycastHit hit)
    {
        GameObject effect = null;

        // Check object pool
        if (pool.Data())
        {
            // Use object in pool
            effect = pool.GetObject();
            effect.GetComponent<Transform>().position = hit.point;
            effect.GetComponent<Transform>().rotation = Quaternion.LookRotation(hit.normal);
        }
        else
        {
            // Add new object to pool
            effect = GameObject.Instantiate<GameObject>(prefab_Effect, hit.point, Quaternion.LookRotation(hit.normal), effectParent);
            effect.name = "Effect_" + materialType;
        }

        StartCoroutine(Delay(effect, 1));
    }

    private IEnumerator Delay(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(obj);
    }
}
