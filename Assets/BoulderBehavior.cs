using UnityEngine;
using System.Collections;

public class BoulderBehavior : MonoBehaviour {
	
	public float speed = 15;
	
	public CharacterController boulderC;
	
	public float life=2;
	
	public GameObject explosionParticles;
	public GameObject enemy;
	public GameObject control;
	public GameObject sphere;
	public int collisions;
	
	public AudioClip body_hit_sound;
	
	// Use this for initialization
	void Start () {
		collisions=0;
		sphere=GameObject.FindGameObjectWithTag("stupid");
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		boulderC.Move(transform.forward * speed * Time.deltaTime);
		//GetComponentsInChildren<Sphere>();
		//Transform child=transform.Find("Sphere");
		//child.Rotate(Time.deltaTime*20, 0, 0);
		//GameObject sphere=GameObject.GetComponent("marker");
		sphere.transform.Rotate(Time.deltaTime*20,0,0);
		life+=Time.deltaTime;
		if(life >=10) Destroy(gameObject);
		Vector3 direction=(enemy.transform.position-boulderC.transform.localPosition);
		direction.Normalize();
		Quaternion test=new Quaternion(0,0,0,0);
		test.SetLookRotation(direction,boulderC.transform.up);
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		boulderC.transform.rotation = Quaternion.Lerp(boulderC.transform.rotation,test, Time.deltaTime*1.5);
		shittyCollisionDetection(4);
		
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		
		collisions=collisions+1;
		//Debug.Log(hit.controller.name);
		//Debug.Log(hit);
		if(collisions>10){
			GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
			Destroy(gameObject);
		}
		if(hit.collider.name.Equals("Hitbox")){
				//insert hit something report here.
				//Destroy(gameObject);
			}
		
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	public void setControl(GameObject e){
		control=e;	
	}
	public void shittyCollisionDetection(int lose){
		if(Vector3.Distance(boulderC.transform.position,control.transform.position)<3){
			Debug.Log("hit something");
			control.audio.PlayOneShot(body_hit_sound);
			control.GetComponent<CarpetControl>().detract(lose);
			Destroy(gameObject);
			
		}
		
	}
}
