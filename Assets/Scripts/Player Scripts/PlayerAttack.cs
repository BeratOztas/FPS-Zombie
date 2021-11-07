using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;
    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomAnimator;
    private Camera mainCamera;
    private GameObject crosshair;
    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;
    [SerializeField]
    private Transform arrow_Bow_Start_Position;
   

    void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();
        zoomAnimator = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
        mainCamera = Camera.main;
    }

    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }
    void WeaponShoot()
    {
        //if selected weapon is assault rifle
        if (weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            //if we press and hold 
            //if Time.time greater than nextTimeToFire 
            if(Input.GetMouseButton(0)&& Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weapon_Manager.GetCurrentSelectedWeapon().shootAnimation();
                BulletFired();

            }

        }//assault if
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //handle axe
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().shootAnimation();
                }
                //handle shoot
                if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().shootAnimation();
                    BulletFired();
                }
                else
                {
                    //we have spear or bow
                    if (is_Aiming)
                    {
                        weapon_Manager.GetCurrentSelectedWeapon().shootAnimation();
                        if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowAndSpear(true);
                        }
                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowAndSpear(false);
                        }


                    }//is aiming
                    

                }

            }//mouse if

        }//else
       

    }//weapon shoot
    
    void ZoomInAndOut()
    {
        //we are going to aim with our camera
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomAnimator.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);
                               
            }
            if (Input.GetMouseButtonUp(1))
            {
                zoomAnimator.Play(AnimationTags.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);

            }


        }//if we need to zoom weapon
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }




        }//weapon self aim


    }//zoom in and out
    void ThrowArrowAndSpear(bool isArrow)
    {
        //Instantiating arrow
        if (isArrow) {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_Start_Position.position;
            arrow.GetComponent<ArrowSpear>().Launch(mainCamera);


        }
        else
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_Start_Position.position;
            spear.GetComponent<ArrowSpear>().Launch(mainCamera);

        }


    }//throw and spear
    void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position,mainCamera.transform.forward,out hit))
        {
            if(hit.transform.gameObject.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }
}//class

