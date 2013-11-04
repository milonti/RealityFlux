using UnityEngine;
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
		//life+=Time.deltaTime;
		
		fireC.Move(transform.forward * speed * Time.deltaTime);
		//Vector3 direction=(enemy.transform.position-fireC.transform.localPosition);
		//direction.Normalize();
		//Quaternion test=new Quaternion(0,0,0,0);
		//test.SetLookRotation(direction,fireC.transform.up);
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		//fireC.transform.rotation = Quaternion.Lerp(fireC.transform.rotation,test, Time.deltaTime);
		shittyCollisionDetection(10);
		life+=Time.deltaTime;
		if(life >=3) Destroy(gameObject);
	}
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	public void setControl(GameObject e){
		enemy=e;	
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		//WizardGUIScript.addHealth(-5);
		GameObject explode = (GameObject)Instantiate(explosionParticles, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		if(hit.collider.name.Equals("Hitbox")){
				//insert hit something report here.
				//Destroy(gameObject);
			}
		Destroy(gameObject);
	}
	public void shittyCollisionDetection(int lose){
		if(Vector3.Distance(fireC.transform.position,control.transform.position)<3&&life>2){
			Debug.Log("hit something");
			control.GetComponent<CarpetControl>().detract(lose);
			Destroy(gameObject);
			
		}
		
	}
	
}
