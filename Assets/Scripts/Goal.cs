using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Goal : MonoBehaviour
{
    /*************************************************************************************************
    *** Variables
    *************************************************************************************************/
    //A static field accesible by code anywhere
    static public bool goalMet = false;
	
	
    /*************************************************************************************************
    *** OnTriggerEnter
    *************************************************************************************************/
    void OnTriggerEnter(Collider col)
    {
	   //When the trigger is hit by something
	   //Check to see if its a Projectile
	   if (col.gameObject.tag == "Projectile")
	   {
		  //If so, set goalMet to true
		  Goal.goalMet = true;

		  //Also set the alpha of the color to higher opacity
		  Color c = gameObject.GetComponent<Renderer>().material.color;
		  c.a = 1;
		  gameObject.GetComponent<Renderer>().material.color = c;

	   }//if

    }//void OnTriggerEnter
    

}//public class Goal