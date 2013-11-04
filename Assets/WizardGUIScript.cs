﻿using UnityEngine;
using System.Collections;

public class WizardGUIScript : MonoBehaviour {
	//int length;
	static float HPcurrent=80;
	static float MPcurrent=100;
	static float maxHealth=100;
	static float maxMana=100;
	void start(){
		//length=Screen.width/2;
		//length=100;
	}
	void OnGUI () {
		// Make a background box
		//length=Screen.width/2-20;
		
		GUI.Box (new Rect(0,Screen.height-40, Screen.width/2-80, 40), (int)HPcurrent + "/" + maxHealth);   
		GUI.Box (new Rect(Screen.width/2+80,Screen.height-40, Screen.width/2-80, 40), (int)MPcurrent + "/" + maxMana); 
		GUI.Box(new Rect(Screen.width/2-80,Screen.height-80,160,80), "");
		GUI.Box(new Rect(Screen.width/2-5,Screen.height/2-5, 10, 10), "");
		GUI.backgroundColor = Color.red;
		GUI.color=Color.red;
		GUI.Box (new Rect(0,Screen.height-40,HPcurrent/maxHealth*(Screen.width/2-80), 40), "","button");   
		GUI.backgroundColor = Color.blue;
		GUI.color=Color.blue;
		GUI.Box (new Rect(Screen.width/2+80,Screen.height-40,MPcurrent/maxMana*(Screen.width/2-80), Screen.width), "","button");
		GUI.color=Color.yellow;
		if(HPcurrent<0)GUI.Box (new Rect( Screen.width/2-40,Screen.height/2-20,80,40), "YOU LOSE","button");
		
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
	static public void addHealth(float h){
		HPcurrent+=h;
		if(HPcurrent>maxHealth)HPcurrent=maxHealth;
		
	}
	static public void addMana(float m){
		MPcurrent+=m;
		if(MPcurrent>maxMana)MPcurrent=maxMana;
	}
	static public void setMana(float m){
		MPcurrent=m;	
	}
	static public void setHealth(float h){
		HPcurrent=h;	
	}
	static public float getHealth(){return HPcurrent;}
	static public float getMana(){return MPcurrent;}
	
}