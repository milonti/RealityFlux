using UnityEngine;
using System.Collections;

public class BounceBehavior : MonoBehaviour {
	
	public float speed = 20;
	
	public CharacterController bounceC;
	double counter=0;
	public float life=0;
	public bool hitGround=false;
	public GameObject explosionParticles;
	public GameObject enemy;
	public Vector3 oldEnemyPosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		if(hitGround){
			counter+=Time.deltaTime;
			Vector3 direction=(oldEnemyPosition-bounceC.transform.localPosition);
			direction.Normalize();
			Quaternion test=new Quaternion(0,0,0,0);
			test.SetLookRotation(direction,bounceC.transform.up);
			bounceC.transform.rotation=test;
			bounceC.Move(transform.forward * speed * Time.deltaTime);
			if(counter>=6)hitGround=false;
			
		}
		else bounceC.Move(new Vector3(0,-1,0)*Time.deltaTime*speed);
		/*
		bounceC.Move(transform.forward * speed * Time.deltaTime);
		Vector3 direction=(enemy.transform.position-bounceC.transform.localPosition);
		direction.Normalize();
		Quaternion test=new Quaternion(0,0,0,0);
		test.SetLookRotation(direction,bounceC.transform.up);
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		bounceC.transform.rotation = Quaternion.Lerp(bounceC.transform.rotation,test, Time.deltaTime);
		*/
		life+=Time.deltaTime;
		if(life >=30) Destroy(gameObject);
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		//GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		//if(hit.collider.name.Equals("Terrain")){
			Debug.Log(hit.collider.name);
			//bounce!
			//do the animation
			if(hit.collider.name.Equals("Hitbox")){
				//insert hit something report here.
				Destroy(gameObject);
			}
			else{
			hitGround=true;
			counter=0;
			oldEnemyPosition=enemy.transform.position;
			}
		//}
		
		//Destroy(gameObject);
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
}
