﻿using UnityEngine;
using System.Collections;

public class SFireballBehavior : MonoBehaviour {
	
	public float speed = 300;
	
	public CharacterController fireC;
	
	public float life;
	
	public GameObject explosionParticles;
	public GameObject enemy;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		life+=Time.deltaTime;
		if(life >=3) Destroy(gameObject);
		fireC.Move(transform.forward * speed * Time.deltaTime);
		Vector3 direction=(enemy.transform.position-fireC.transform.localPosition);
		direction.Normalize();
		//Quaternion test=new Quaternion(0,0,0,0);
		//test.SetLookRotation(direction,fireC.transform.up);
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		//fireC.transform.rotation = Quaternion.Lerp(fireC.transform.rotation,test, Time.deltaTime);
		
		//life+=Time.deltaTime;
		
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		if(!hit.collider.name.Equals("Terrain") && !hit.collider.name.Equals("Fireball(Clone)")){
			hit.collider.rigidbody.AddForceAtPosition(transform.forward * speed * 10, hit.point);
		}
		Destroy(gameObject);
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
}
