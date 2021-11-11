using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    Animator player_Animator;
    [SerializeField]
    GameObject main_Camera;
    [SerializeField]
    Slider player_HpBer;
    [SerializeField]
    Text ammoText;
    [SerializeField]
    AudioSource playerFootStep;
    [SerializeField]
    AudioSource weaponSound;
    [SerializeField]
    AudioClip relodingSE, fireSE, triggerSE;
    [SerializeField]
    AudioClip WalkFootStepSE, RunFootStepSE;
    [SerializeField]
    AudioSource damageVoise, hitSound;
    [SerializeField]
    AudioClip damageVoiseSE, hitSE;
 
    float player_Walk_Speed = 0.07f;
    [SerializeField]
    float ysensityvity;
    [SerializeField]
    float xsensitivity;
    [SerializeField]
    float player_Hp = 100;
    
    Quaternion cameraRot;
    Quaternion characterRot;

    int ammoClip = 60;
    int ammunition = 360;
    int amountNeed;
    float xMove, zMove;
    bool cursorLock = true;

    const int MAX_AMMUNITION = 360;
    const int MAX_AMMOCLIP = 60;
    const float MIN_X = -90f;
    const float MAX_X = 90f;
    const float MAX_PLAYRE_HP = 100f;


    public int AmmoClip
    {
        get { return ammoClip; }
    }
    public float PlayerHP
    {
        get { return player_Hp; }
    }
   
    void Start()
    {
        GameState.canShoot = true;
        cameraRot = main_Camera.transform.localRotation;
        characterRot = transform.localRotation;

        SetHp();

    }

    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * xsensitivity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);

        cameraRot = ClampRotation(cameraRot);   

        main_Camera.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;
        UpdateCursorLock();

        if (Input.GetMouseButton(0) && GameState.canShoot)
        {
            if(ammoClip > 0)
            {
                GameState.canShoot = false;
                player_Animator.SetTrigger("Fire");
                ammoClip--;
                SetAmmoText();
             
            }
            else
            {
                TriggerSE();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            amountNeed = MAX_AMMOCLIP - ammoClip;
            int ammoAvailable = amountNeed < ammunition ? amountNeed : ammunition;
            if (amountNeed != 0 && ammunition != 0)
            {
                player_Animator.SetTrigger("Reload");
             
                SetAmmoClip(ammoAvailable);
                SetAmmoText();
            }  
        }
        if (Mathf.Abs(xMove) > 0 || Mathf.Abs(zMove) > 0)
        {
            if (!player_Animator.GetBool("Walk"))
            {
                player_Animator.SetBool("Walk", true);
                PlayerWalkFootStep(WalkFootStepSE);
            }   
        }
        else if (player_Animator.GetBool("Walk"))
        {
            player_Animator.SetBool("Walk", false);
            StopFootStep();
        }
        if (zMove > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            if (!player_Animator.GetBool("Run"))
            {
                player_Animator.SetBool("Run", true);
                player_Walk_Speed = 0.12f;

                PlayerRunFootStep(RunFootStepSE);
            }
        }
        else if (player_Animator.GetBool("Run"))
        {
            player_Animator.SetBool("Run", false);
            player_Walk_Speed = 0.07f;
            StopFootStep();
        }


    }
    private void FixedUpdate()
    {
        xMove = 0;
        zMove = 0;

        xMove = Input.GetAxisRaw("Horizontal") * player_Walk_Speed;
        zMove = Input.GetAxisRaw("Vertical") * player_Walk_Speed;

        transform.position += main_Camera.transform.forward * zMove + main_Camera.transform.right * xMove;
    }

  
    public void SetAmmoText()
    {
        ammoText.text = ammoClip + "/" + ammunition;
    }
    public void SetHp()
    {
        player_HpBer.value = MAX_PLAYRE_HP;
        ammoText.text = MAX_AMMOCLIP + "/" + MAX_AMMUNITION;
    }

   
    public void SetAmmoClip(int bl)
    {
        ammunition -= bl;
        ammoClip += bl;
    }
    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void CalculationPlayerHp(float damage)
    {
        player_Hp = (int)Mathf.Clamp(player_Hp - damage, 0, MAX_PLAYRE_HP);
        
    }
    public void UpdatePlayerHp()
    {
        player_HpBer.value = player_Hp;
    }
 
 

    public void TakeHit(float damage)
    {
        VoiseSE();
        CalculationPlayerHp(damage);
        UpdatePlayerHp();

        GameState.gameStateInstance.CheckGameState();
    }
    public Quaternion ClampRotation(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;
        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;
        angleX = Mathf.Clamp(angleX, MIN_X, MAX_X);
        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        return q;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "bullet")
        {
            ammunition += 10;
            ammunition = (int)Mathf.Clamp(ammunition, 0, MAX_AMMUNITION);
            SetAmmoText();
            Destroy(other.gameObject);
        } 
    }
    public void CanShoot()
    {
        GameState.canShoot = true;
    }
    public void PlayerWalkFootStep(AudioClip clip)
    {
        playerFootStep.loop = true;
        playerFootStep.pitch = 1f;
        playerFootStep.clip = clip;
        playerFootStep.Play();
    }
    public void PlayerRunFootStep(AudioClip clip)
    {
        playerFootStep.loop = true;
        playerFootStep.pitch = 1.3f;
        playerFootStep.clip = clip;
        playerFootStep.Play();
    }
    public void StopFootStep()
    {
        playerFootStep.Stop();
        playerFootStep.loop = false;
        playerFootStep.pitch = 1f;
    }
    public void FireSE()
    {
        weaponSound.clip = fireSE;
        weaponSound.Play();
    }
    public void RelodinSE()
    {
        weaponSound.clip = relodingSE;
        weaponSound.Play();
    }
    public void TriggerSE()
    {
        weaponSound.clip = triggerSE;
        weaponSound.Play();
    }
    public void VoiseSE()
    {
        damageVoise.Stop();
        damageVoise.clip = damageVoiseSE;
        damageVoise.Play();
    }
    public void HitSE()
    {
        hitSound.Stop();
        hitSound.clip = hitSE;
        hitSound.Play();    
    }

}
