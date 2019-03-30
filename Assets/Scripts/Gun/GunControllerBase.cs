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
    private float durable_2;

    private GameObject toolBarIcon;

    // Component
    private AudioClip audio;
    private GameObject effect;
    private GunViewBase m_GunViewBase;
    private Ray ray;
    private RaycastHit hit;

    private bool canShoot = true;

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

    public GameObject ToolBarIcon { get { return toolBarIcon; } set { toolBarIcon = value; } }

    // Component's attribute
    public GunViewBase M_GunViewBase { get { return m_GunViewBase; } set { m_GunViewBase = value; } }
    public AudioClip Audio { get { return audio; } set { audio = value; } }
    public GameObject Effect { get { return effect; } set { effect = value; } }
    public Ray MyRay { get { return ray; } set { ray = value; } }
    public RaycastHit Hit { get { return hit; } set { hit = value; } }

    protected virtual void Start()
    {
        durable_2 = Durable;
        m_GunViewBase = gameObject.GetComponent<GunViewBase>();
        LoadAudio();
        Init();
    }

    void Update()
    {
        MouseControl();
        ShootReady();
    }

    private void UpdateUI()
    {
        toolBarIcon.GetComponent<InventoryItemController>().UpdateUI(Durable / durable_2);
    }

    // Play sound
    protected void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(Audio, m_GunViewBase.GunPoint.position);
    }

    // Control using mouse
    protected void MouseControl()
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

    protected virtual void MouseButtonLeftDown()
    {
        m_GunViewBase.M_Animator.SetTrigger("Fire");
        PlayAudio();
        Shoot();
        UpdateUI();
    }
    protected void MouseButtonRightDown()
    {
        m_GunViewBase.M_Animator.SetBool("HoldPose", true);
        m_GunViewBase.EnterHoldPose();
        m_GunViewBase.FrontSight.gameObject.SetActive(false);
    }
    protected void MouseButtonRightUp()
    {
        m_GunViewBase.M_Animator.SetBool("HoldPose", false);
        m_GunViewBase.ExistHoldPose();
        m_GunViewBase.FrontSight.gameObject.SetActive(true);
    }

    // Ready aim fire
    protected void ShootReady()
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
    protected IEnumerator Delay(ObjectPool pool, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(obj);
    }

    protected void CanShoot(int state)
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
    protected abstract void Init();

    // Load audio resources
    protected abstract void LoadAudio();

    // Fire
    protected abstract void Shoot();
}
