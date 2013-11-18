using UnityEngine;
using System.Collections;

public class WallBehavior : MonoBehaviour {
	
	public float speed = 15;
	
	public CharacterController wallC;
	
	public float life=2;
	public int collisions=0;
	public GameObject explosionParticles;
	public GameObject enemy;
	public GameObject control;
	
	public AudioClip body_hit_sound;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		wallC.Move(transform.forward * speed * Time.deltaTime);
		life+=Time.deltaTime;
		//wallC.transform.localScale=wallC.transform.localScale*1.5f*Time.deltaTime;
		if(life >=10) Destroy(gameObject);
		//Vector3 direction=(enemy.transform.position-wallC.transform.localPosition);
		//direction.Normalize();
		//Quaternion test=new Quaternion(0,0,0,0);
		//test.SetLookRotation(direction,wallC.transform.up);
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		//wallC.transform.rotation = Quaternion.Lerp(wallC.transform.rotation,test, Time.deltaTime*2);
		//shittyCollisionDetection(4);
		
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		//GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		if(hit.collider.tag.Equals("Spell"))collisions++;
		//Debug.Log(hit.controller.name);
		Debug.Log(hit.collider.tag);
		Debug.Log (hit.collider.name);
		
		if(collisions>10){
				//insert hit something report here.
				Destroy(gameObject);
			}
		//Destroy(gameObject);
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	public void setControl(GameObject e){
		control=e;	
	}
	
}
