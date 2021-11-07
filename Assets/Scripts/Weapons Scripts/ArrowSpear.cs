using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpear : MonoBehaviour
{
   
    private Rigidbody myBody;
    public float speed = 30f;
    public float deactive_Timer = 3f;
    public float damage = 15F;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }
     void Start()
    {
        Invoke("DeactiveGameObject", deactive_Timer);
    }
    public void Launch(Camera mainCamera)
    {
        //it 'll move in the camera forward position.
        myBody.velocity = mainCamera.transform.forward * speed;
        //to protect our arrow rotation we use lookat method
        transform.LookAt(transform.position + myBody.velocity);


    }

    void DeactiveGameObject()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
    //if arrow trigger any object
     void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.ENEMY_TAG)
        {
            target.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
        
    }

}
