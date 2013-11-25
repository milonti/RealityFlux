﻿using UnityEngine;
using System.Collections;

public class SFireballBehavior : MonoBehaviour {
	
	public float speed = 300;
	
	public CharacterController fireC;
	
	public float life;
	
	public GameObject explosionParticles;
	public GameObject enemy;
	public GameObject control;
	
	public AudioClip body_hit_sound;
	
	public bool dest = false;
	public float dTime = 0;
	
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		
		if (!dest) {
			fireC.Move(transform.forward * speed * Time.deltaTime);
			//Vector3 direction=(enemy.transform.position-fireC.transform.localPosition);
			//direction.Normalize();
			//Quaternion test=new Quaternion(0,0,0,0);
			//test.SetLookRotation(direction,fireC.transform.up);
			//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
			//fireC.transform.rotation = Quaternion.Lerp(fireC.transform.rotation,test, Time.deltaTime);
			shittyCollisionDetection(5);
			life+=Time.deltaTime;
			if(life >=3) {
				dest = true;
				renderer.enabled = false;
			}
		}
		if(dest) dTime += Time.deltaTime;
		if(dTime >= 5) Destroy(gameObject); 
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	public void setControl(GameObject e){
		control=e;	
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		//WizardGUIScript.addHealth(-5);
		//GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		if(hit.collider.name.Equals("Hitbox")){
				//insert hit something report here.
				//Destroy(gameObject);
			}
		
		dest = true;
	}
	
	public void shittyCollisionDetection(int lose){
		if(Vector3.Distance(fireC.transform.position,control.transform.position)<10){
			Debug.Log("hit something");
			control.audio.PlayOneShot(body_hit_sound);
			control.GetComponent<CarpetControl>().detract(lose);
			dest = true;
			renderer.enabled = false;
		}
		
	}
	
}
