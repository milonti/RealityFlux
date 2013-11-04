using UnityEngine;
using System.Collections;

public class FireballBehavior : MonoBehaviour {
	
	public float speed = 10;
	
	public CharacterController fireC;
	
	public float life=5;
	
	public GameObject explosionParticles;
	public GameObject enemy;
	public GameObject control;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		fireC.Move(transform.forward * speed * Time.deltaTime);
		life+=Time.deltaTime;
		if(life >=10) Destroy(gameObject);
		Vector3 direction=(enemy.transform.position-fireC.transform.localPosition);
		direction.Normalize();
		Quaternion test=new Quaternion(0,0,0,0);
		test.SetLookRotation(direction,fireC.transform.up);
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		fireC.transform.rotation = Quaternion.Lerp(fireC.transform.rotation,test, Time.deltaTime);
		//shittyCollisionDetection(5);
		
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		Debug.Log(hit.controller.name);
		Debug.Log(hit);
		
		if(hit.collider.name.Equals("Hitbox")){
			WizardGUIScript.addHealth(-5);
				//insert hit something report here.
				//Destroy(gameObject);
			}
		Destroy(gameObject);
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	public void setControl(GameObject e){
		enemy=e;	
	}
	public void shittyCollisionDetection(int lose){
		if(Vector3.Distance(gameObject.transform.position,enemy.transform.position)<5){
			
			Destroy(gameObject);
			
		}
		
	}
}
