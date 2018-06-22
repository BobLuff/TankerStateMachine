using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject Explosion;
    public float Speed = 600f;
    public float LifeTime = 3f;
    public int damage = 50;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, LifeTime);
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Speed * Time.deltaTime;
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
       // collision.contacts[1];
        Instantiate(Explosion, contact.point, Quaternion.identity);
        Destroy(gameObject);
    }
}
