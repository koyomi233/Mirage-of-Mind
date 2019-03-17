using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create mark of bullet
/// </summary>
public class BulletMark : MonoBehaviour
{
    private Texture2D m_BulletMark;                     // Bullet texture
    private Texture2D m_MainTexture;                    // Model texture
    private Texture2D m_MainTextureBackup;              // Back up model texture

    [SerializeField]
    private MaterialType materialType;                  // Material of the model

    private Queue<Vector2> bulletMarkQuene = null;      // Queue of bullet mark
    
    void Start()
    {
        switch (materialType)
        {
            case MaterialType.Metal:
                m_BulletMark = Resources.Load<Texture2D>("Gun/BulletMarks/Bullet Decal_Metal");
                break;
            case MaterialType.Stone:
                m_BulletMark = Resources.Load<Texture2D>("Gun/BulletMarks/Bullet Decal_Stone");
                break;
            case MaterialType.Wood:
                m_BulletMark = Resources.Load<Texture2D>("Gun/BulletMarks/Bullet Decal_Wood");
                break;
        }
        m_MainTexture = (Texture2D)gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        m_MainTextureBackup = GameObject.Instantiate<Texture2D>(m_MainTexture);

        bulletMarkQuene = new Queue<Vector2>();
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
                float x = uv.x * m_MainTexture.width - m_BulletMark.width / 2 + i;
                float y = uv.y * m_MainTexture.height - m_BulletMark.height / 2 + j;

                // Get the color of bullet mark texture
                Color color = m_BulletMark.GetPixel(i, j);

                // Merge textures of model and bullet
                if(color.a > 0.2f)
                {
                    m_MainTexture.SetPixel((int)x, (int)y, color);
                }
            }
        }
        m_MainTexture.Apply();
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
                    float x = uv.x * m_MainTexture.width - m_BulletMark.width / 2 + i;
                    float y = uv.y * m_MainTexture.height - m_BulletMark.height / 2 + j;

                    // Get the color of original texture
                    Color color = m_MainTextureBackup.GetPixel((int) x, (int) y);

                    m_MainTexture.SetPixel((int)x, (int)y, color);
                }
            }
            m_MainTexture.Apply();
        }
    }
}
