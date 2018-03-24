using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ProjectileLine : MonoBehaviour
{
    /*************************************************************************************************
    *** Variables
    *************************************************************************************************/
    static public ProjectileLine S; //Singleton

    public float minDistance = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
	
	
    /*************************************************************************************************
    *** Start
    *************************************************************************************************/
    void Awake ()
    {
	   S = this;

	   //Get a reference to the LineRenderer
	   line = gameObject.GetComponent<LineRenderer>();

	   //Disable the LineRenderer until its needed
	   line.enabled = false;

	   //Initialize the points List
	   points = new List<Vector3>();
    
    }//void Start
    
    
    /*************************************************************************************************
    *** FixedUpdate
    *************************************************************************************************/
    void FixedUpdate ()
    {
	   if (poi == null)
	   {
		  //If there is no poi, search for one
		  if (FollowCam.S.poi != null)
		  {
			 if (FollowCam.S.poi.tag == "Projectile")
				poi = FollowCam.S.poi;
			 else
				return; //Return if we didnt find a poi
		  }
		  else
			 return; //Return if we didnt find a poi

	   }//if

	   //If there is a poi, its location is added every FixedUpdate
	   AddPoint();

	   if (poi.GetComponent<Rigidbody>().IsSleeping())
		  //Once the poi is sleeping, it is cleared
		  poi = null;

    }//void FixedUpdate


    /*************************************************************************************************
    *** Properties
    *************************************************************************************************/
    public GameObject poi
    {
	   get { return (_poi); }
	   set
	   {
		  _poi = value;
		  if (_poi != null)
		  {
			 //When _poi is set to something new, it resets everything
			 line.enabled = false;
			 points = new List<Vector3>();
			 AddPoint();

		  }//if
		  
	   }//set

    }//public GameObject poi 


    //Returns the location of the most recently added point
    public Vector3 lastPoint
    {
	   get
	   {
		  if (points == null)
			 //If there are no points, returns Vector3.zero
			 return (Vector3.zero);

		  return (points[points.Count - 1]);

	   }//get

    }//public Vector3 lastPoint


    /*************************************************************************************************
    *** Methods
    *************************************************************************************************/
    //This can be used to clear the line directly
    public void Clear ()
    {
	   _poi = null;
	   line.enabled = false;
	   points = new List<Vector3>();

    }//public void Clear


    public void AddPoint ()
    {
	   //This is called to add a point to the line
	   Vector3 pt = _poi.transform.position;

	   if (points.Count > 0 && (pt - lastPoint).magnitude < minDistance)
		  //If the point isnt far enough from the last point, it returns
		  return;

	   if (points.Count == 0)
	   {
		  //If this is the launch point...
		  Vector3 launchPoint = Slingshot.S.launchPoint.transform.position;
		  Vector3 launchPosDiff = pt - launchPoint;

		  //...it adds an extra bit of line to aid aiming later
		  points.Add(pt + launchPosDiff);
		  points.Add(pt);
		  line.SetVertexCount(2);

		  //Sets the first two points
		  line.SetPosition(0, points[0]);
		  line.SetPosition(1, points[1]);

		  //Enables the LineRenderer
		  line.enabled = true;
	   }
	   else
	   {
		  //Normal behavior of adding a point
		  points.Add(pt);
		  line.SetVertexCount(points.Count);
		  line.SetPosition(points.Count - 1, lastPoint);
		  line.enabled = true;

	   }//if else

    }//public void AddPoint


}//public class ProjectileLine