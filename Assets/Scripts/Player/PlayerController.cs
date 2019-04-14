using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public delegate void PlayerDeathDelegate();

public class PlayerController : MonoBehaviour
{
    public event PlayerDeathDelegate DeathDelegate;

    private Transform m_Transform;
    private FirstPersonController FPS;
    private PlayerInfoPanel m_PlayerInfoPanel;
    private BloodScreenPanel m_BloodScreenPanel;
    private AudioSource m_AudioSource;

    [SerializeField] private int hp = 1000;
    [SerializeField] private int vit = 100;

    private int index = 0;                                                // timekeeper
    private bool breath = false;                                          // Whether start to breath
    private bool isDeath = false;                                         // Whether player is dead

    public int HP { get { return hp; } set { hp = value; } }
    public int VIT { get { return vit; } set { vit = value; } }

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        FPS = gameObject.GetComponent<FirstPersonController>();
        m_PlayerInfoPanel = GameObject.Find("Canvas/MainPanel/PlayerInfoPanel").GetComponent<PlayerInfoPanel>();
        m_BloodScreenPanel = GameObject.Find("Canvas/MainPanel/BloodScreen").GetComponent<BloodScreenPanel>();
        m_AudioSource = AudioManager.Instance.AddAudioSourceComponent(gameObject, ClipName.PlayerBreathingHeavy, false);

        StartCoroutine("RestoreVIT");
    }

    void Update()
    {
        ReduceVIT();
        Debug.Log("Vitality: " + vit);
    }

    // HP decline
    public void ReduceHP(int value)
    {
        if(isDeath == false)
        {
            this.HP -= value;
            AudioManager.Instance.PlayAudioClipByName(ClipName.PlayerHurt, m_Transform.position);
            m_PlayerInfoPanel.SetHP(this.HP);
            m_BloodScreenPanel.SetImageAlpha();
        }
        
        if(this.HP <= 0 && isDeath == false)
        {
            PlayerDeath();
        }
    }

    // VIT decline
    public void ReduceVIT()
    {
        if (FPS.M_PlayerState == PlayerState.WALK)
        {
            index++;
            if(index >= 20)
            {
                this.VIT -= 1;
                ResetSpeed();
                index = 0;
            }
        }
        if(FPS.M_PlayerState == PlayerState.RUN)
        {
            index++;
            if(index >= 20)
            {
                this.VIT -= 2;
                ResetSpeed();
                index = 0;
            }
        }
        if(this.VIT <= 60 && breath == false)
        {
            m_AudioSource.Play();
            breath = true;
        }
        m_PlayerInfoPanel.SetVIT(this.VIT);
    }

    private IEnumerator RestoreVIT()
    {
        Vector3 tempPos;
        while (true)
        {
            tempPos = m_Transform.position;
            yield return new WaitForSeconds(1);
            if (this.VIT <= 95 && m_Transform.position == tempPos)
            {
                this.VIT += 5;
                if(this.VIT > 60 && breath == true)
                {
                    m_AudioSource.Stop();
                    breath = false;
                }
                m_PlayerInfoPanel.SetVIT(this.VIT);
                ResetSpeed();
            }
        }
    }

    // Reset the speed of player
    private void ResetSpeed()
    {
        FPS.M_WalkSpeed = 5 * (this.VIT * 0.01f);
        FPS.M_RunSpeed = 10 * (this.VIT * 0.01f);
    }

    private void PlayerDeath()
    {
        isDeath = true;
        AudioManager.Instance.PlayAudioClipByName(ClipName.PlayerDeath, m_Transform.position);
        m_Transform.GetComponent<FirstPersonController>().enabled = false;
        GameObject.Find("Managers").GetComponent<InputManager>().enabled = false;
        DeathDelegate();
        StartCoroutine("JumpScene");
    }

    // Jump to reset scene when player is dead
    private IEnumerator JumpScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("ResetScene");
    }
}
