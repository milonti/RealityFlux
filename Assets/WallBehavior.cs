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
	
	bool color1 = false;
	
	// Use this for initialization
	void Start () {
		color1 = false;
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		wallC.Move(transform.forward * speed * Time.deltaTime);
		life+=Time.deltaTime;
		
		if(life >=5) Destroy(gameObject);
		//Vector3 direction=(enemy.transform.position-wallC.transform.localPosition);
		if(collisions>10){
				//insert hit something report here.
				Destroy(gameObject);
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		//GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		if(hit.collider.tag.Equals("Spell"))collisions++;
		//Debug.Log(hit.controller.name);
		Debug.Log(hit.collider.tag);
		Debug.Log (hit.collider.name);
		
		
		//Destroy(gameObject);
	}
	
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	
	public void setControl(GameObject e){
		control=e;	
	}
	
	public void OnCollisionEnter(Collision c){
		Debug.Log("Wall got hit");
		if(c.collider.tag.Equals("Spell")) collisions++;
		if(color1) {
			gameObject.renderer.material.color = Color.cyan;
		}
		else{
			gameObject.renderer.material.color = Color.blue;
		}
		color1 = (!color1);
	}
	
}
