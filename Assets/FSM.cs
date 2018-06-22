using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {
    protected Transform playerTransform;

    protected Vector3 destPos;

    protected GameObject[] pointList;


    protected float shootRate;
    protected float elapsedTime;

    public Transform turret { get; set; }
    public Transform bulletSpawnpoint { get; set; }

    protected virtual void Initialize()
    {

    }

    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }



	// Use this for initialization
	void Start () {
        Initialize();
		
	}
	
	// Update is called once per frame
	void Update () {
        FSMUpdate();
		
	}
    private void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
