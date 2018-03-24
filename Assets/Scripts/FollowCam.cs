using UnityEngine;
using System.Collections;


public class FollowCam : MonoBehaviour
{
    /*************************************************************************************************
    *** Variables
    *************************************************************************************************/
    static public FollowCam S; //a FollowCam Singleton
    public float easing = 0.05f;
    public bool debugProjVelocity = false;

    [HideInInspector]
    public GameObject poi; //The point of interest
    private float camZ; //The desired Z pos of the camera
    private Vector2 minXY;


    /*************************************************************************************************
    *** Start
    *************************************************************************************************/
    void Awake()
    {
	   S = this;
	   camZ = gameObject.transform.position.z;

    }//void Start


    /*************************************************************************************************
    *** Start
    *************************************************************************************************/
    void Start ()
    {
        
    
    }//void Start
    
    
	/*************************************************************************************************
    *** FixedUpdate
    *************************************************************************************************/
    void FixedUpdate ()
    {
	   Vector3 destination;

	   //If there is no poi, return to origin
	   if (poi == null)
		  destination = Vector3.zero;
	   else
	   {
		  if (debugProjVelocity)
			 Debug.Log(poi.GetComponent<Rigidbody>().velocity.ToString());
	   
		  //Get the position of the poi
		  destination = poi.transform.position;

		  //If poi is a projectile, check to see if it's at rest
		  if (poi.tag == "Projectile")
		  {
			 //If it is sleeping (that is, not moving)
			 if (poi.GetComponent<Rigidbody>().IsSleeping())
			 {
				//Return to default view
				poi = null;

				//In the next update
				return;
			 }//if

		  }//if

	   }//if else

	   //Limit the X and Y to minimun values
	   destination.x = Mathf.Max(minXY.x, destination.x);
	   destination.y = Mathf.Max(minXY.y, destination.y);

	   //Interpolate from the current Camera position toward destination
	   destination = Vector3.Lerp(gameObject.transform.transform.position, destination, easing);

	   //Retain a destination.z of camZ
	   destination.z = camZ;

	   //Set the camera to the destination
	   gameObject.transform.position = destination;

	   //Set the orthographicSize of the camera to keep ground in view
	   gameObject.GetComponent<Camera>().orthographicSize = destination.y + 10;

    }//void FixedUpdate
	
    
}//public class FollowCam