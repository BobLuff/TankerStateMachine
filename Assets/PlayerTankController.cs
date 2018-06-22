using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : MonoBehaviour {
   

    public GameObject Bullet;

    private Transform Turret;
    public Transform bulletSpawnpoint;
    private float curSpeed, targetSpeed, rotSpeed;
    private float turretRotSpeed = 10.0f;
    private float maxForwardSpeed = 300f;
    private float maxBackwardSpeed = -300f;

    protected float shootRate = 1f;
    protected float elapsedTime=0f;


	// Use this for initialization
	void Start () {
        rotSpeed = 150f;
        Turret = gameObject.transform.GetChild(0).transform;
      //  bulletSpawnpoint = gameObject.transform.GetChild(1).transform;
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateWeapon();
        UpdateControl();
		
	}

    void UpdateWeapon()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
           
          
            Debug.Log(elapsedTime);
            if(elapsedTime>=shootRate)
            {
                Debug.Log("Weapon:" + Bullet);
                elapsedTime = 0.0f;
                Instantiate(Bullet, bulletSpawnpoint.position, bulletSpawnpoint.rotation);


            }
        }
    }


    void UpdateControl()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position += new Vector3(0, 0, 0));

        Ray RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        float HitDist = 0;

        if (playerPlane.Raycast(RayCast,out HitDist))
        {
            Vector3 RayHitPoint = RayCast.GetPoint(HitDist);
            Quaternion targetRotation = Quaternion.LookRotation(RayHitPoint - transform.position);
            Turret.transform.rotation = Quaternion.Slerp(Turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
        }

        if(Input.GetKey(KeyCode.W))
        {
            targetSpeed = maxForwardSpeed;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            targetSpeed = maxBackwardSpeed;
        }
        else
        {
            targetSpeed = 0;
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotSpeed * Time.deltaTime, 0.0f);

        } else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0.0f);
        }

        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, 7.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

   
}
