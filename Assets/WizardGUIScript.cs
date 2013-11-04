﻿using UnityEngine;
using System.Collections;

public class WizardGUIScript : MonoBehaviour {
	//int length;
	float HPcurrent=80;
	float MPcurrent=100;
	float maxHealth=100;
	float maxMana=100;
	void start(){
		//length=Screen.width/2;
		//length=100;
	}
	void OnGUI () {
		// Make a background box
		//length=Screen.width/2-20;
		
		GUI.Box (new Rect(0,Screen.height-40, Screen.width/2-80, 40), HPcurrent + "/" + maxHealth);   
		GUI.Box (new Rect(Screen.width/2+80,Screen.height-40, Screen.width/2-80, 40), MPcurrent + "/" + maxMana); 
		GUI.Box(new Rect(Screen.width/2-80,Screen.height-80,160,80), "");
		GUI.Box(new Rect(Screen.width/2,Screen.height/2, 10, 10), "");
		GUI.backgroundColor = Color.red;
		GUI.color=Color.red;
		GUI.Box (new Rect(0,Screen.height-40,HPcurrent/maxHealth*(Screen.width/2-80), 40), "","button");   
		GUI.backgroundColor = Color.blue;
		GUI.color=Color.blue;
		GUI.Box (new Rect(Screen.width/2+80,Screen.height-40,MPcurrent/maxMana*(Screen.width/2-80), Screen.width), "","button");
		
		
		//GUI.Box(rec
		/*
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,80,20), "Level 1")) {
			Application.LoadLevel(1);
		}

		// Make the second button.
		if(GUI.Button(new Rect(20,70,80,20), "Level 2")) {
			Application.LoadLevel(2);
		}
		*/
	}
	public void addHealth(int h){
		HPcurrent+=h;
		if(HPcurrent>maxHealth)HPcurrent=maxHealth;
		
	}
	public void addMana(int m){
		MPcurrent+=m;
		if(MPcurrent>maxMana)MPcurrent=maxMana;
	}
	public void setMana(int m){
		MPcurrent=m;	
	}
	public void setHealth(int h){
		HPcurrent=h;	
	}
	public float getHealth(){return HPcurrent;}
	public float getMana(){return MPcurrent;}
}