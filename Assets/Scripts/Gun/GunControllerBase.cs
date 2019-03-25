using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller of Guns
/// </summary>
public abstract class GunControllerBase : MonoBehaviour
{
    // Field
    [SerializeField] private GunType gunWeaponType;
    [SerializeField] private int id;
    [SerializeField] private int damage;
    [SerializeField] private int durable;

    // Component
    private AudioClip audio;
    private GameObject effect;
    private GunViewBase m_GunViewBase;
    private Ray ray;
    private RaycastHit hit;

    // Field attribute
    public GunType GunWeaponType { get { return gunWeaponType; } set { gunWeaponType = value; } }
    public int Id { get { return id; } set { id = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Durable
    {
        get { return durable; }
        set
        {
            durable = value;
            if (durable <= 0)
            {
                GameObject.Destroy(gameObject);
                GameObject.Destroy(m_GunViewBase.FrontSight.gameObject);
            }
        }
    }

    private bool canShoot = true;

    // Component's attribute
    public GunViewBase M_GunViewBase { get { return m_GunViewBase; } set { m_GunViewBase = value; } }
    public AudioClip Audio { get { return audio; } set { audio = value; } }
    public GameObject Effect { get { return effect; } set { effect = value; } }
    public Ray MyRay { get { return ray; } set { ray = value; } }
    public RaycastHit Hit { get { return hit; } set { hit = value; } }

    public virtual void Start()
    {
        m_GunViewBase = gameObject.GetComponent<GunViewBase>();
        LoadAudio();
        LoadEffect();
        Init();
    }

    void Update()
    {
        MouseControl();
        ShootReady();
    }

    // Play sound
    public void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(Audio, m_GunViewBase.GunPoint.position);
    }

    // Control using mouse
    private void MouseControl()
    {
        // Shoot
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            MouseButtonLeftDown();
        }
        // Aim
        if (Input.GetMouseButton(1))
        {
            MouseButtonRightDown();
        }
        // Reset
        if (Input.GetMouseButtonUp(1))
        {
            MouseButtonRightUp();
        }
    }

    private void MouseButtonLeftDown()
    {
        m_GunViewBase.M_Animator.SetTrigger("Fire");
        PlayEffect();
        PlayAudio();
        Shoot();
    }
    private void MouseButtonRightDown()
    {
        m_GunViewBase.M_Animator.SetBool("HoldPose", true);
        m_GunViewBase.EnterHoldPose();
        m_GunViewBase.FrontSight.gameObject.SetActive(false);
    }
    private void MouseButtonRightUp()
    {
        m_GunViewBase.M_Animator.SetBool("HoldPose", false);
        m_GunViewBase.ExistHoldPose();
        m_GunViewBase.FrontSight.gameObject.SetActive(true);
    }

    // Ready aim fire
    public void ShootReady()
    {
        ray = new Ray(m_GunViewBase.GunPoint.position, m_GunViewBase.GunPoint.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(m_GunViewBase.M_EnvCamera, hit.point);
            m_GunViewBase.FrontSight.position = uiPos;
        }
        else
        {
            hit.point = Vector3.zero;
            Debug.Log("no hit message");
        }
    }

    // Delay
    public IEnumerator Delay(ObjectPool pool, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(obj);
    }

    private void CanShoot(int state)
    {
        if(state == 0)
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
    }

    // Initialize all components
    public abstract void Init();

    // Load audio resources
    public abstract void LoadAudio();

    // Load effect resources
    public abstract void LoadEffect();

    // Fire
    public abstract void Shoot();

    // Play effect
    public abstract void PlayEffect();
}
