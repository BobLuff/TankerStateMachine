using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM {

    public enum FSMState
    {
     //   [Description("this means facing to UP (Negtive Y)")]
        None,
        Patrol,
        Chase,
        Atttack,
        Dead,

    }

    public FSMState curState;

    private float curSpeed;

    private float curRotSpeed;

    public GameObject Bullet;

    private bool bDead;
    private int health;


    protected override void Initialize()
    {
        // base.Initialize();
        curState = FSMState.Patrol;
        curSpeed = 150f;
        curRotSpeed = 2f;
        bDead = false;
        elapsedTime = 0f;
        shootRate = 3f;
        health = 100;

        pointList = GameObject.FindGameObjectsWithTag("Wandarpoint");

        FindNextPoint();

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");

        playerTransform = objPlayer.transform;
        if(!playerTransform)
        {
            print("Player  not exist!");
            turret = gameObject.transform.GetChild(0).transform;
            bulletSpawnpoint = turret.GetChild(0).transform;
        }
    }
    protected void FindNextPoint()
    {
        print("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;
        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;

        if(IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }   
        
    }

    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);
        if (xPos <= 50 && zPos <= 50)
            return true;
        return false;
    }


    protected void UpdateChaseState()
    {
        destPos = playerTransform.position;

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if(dist<=200f)
        {
            curState = FSMState.Atttack;
        }
        else if(dist>=300f)
        {
            curState = FSMState.Patrol;
        }
        transform.Translate(Vector3.forward*Time.deltaTime * curSpeed);

    }    

    protected void UpdateAttackState()
    {
        destPos = playerTransform.position;

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if(dist>=200f&&dist<300f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
            curState = FSMState.Atttack;

        }
        else if(dist>=300f)
        {
            curState = FSMState.Patrol;
        }
        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

        ShootBullet();
    }

    private void ShootBullet()
    {
        if(elapsedTime>=shootRate)
        {
            Instantiate(Bullet, bulletSpawnpoint.position, bulletSpawnpoint.rotation);
            elapsedTime = 0f;
        }
    }

    protected void UpdateDeadState()
    {
        if(!bDead)
        {
            bDead = true;
            Explode();
        }
    }

    protected void Explode()
    {
        float rndX = Random.Range(10f, 30f);
        float rndZ = Random.Range(10f, 30f);
        for(int i=8; i<3;i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10000f, transform.position - new Vector3(rndX, 10f, rndZ), 20f, 10f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 10f, rndZ));
        }
        Destroy(gameObject, 1.5f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}
