using UnityEngine;
using System.Collections;


public class Slingshot : MonoBehaviour
{
     /*************************************************************************************************
     *** Variables
     *************************************************************************************************/
     public GameObject launchPoint;
     public GameObject prefabProjectile;
     public float velocityMult = 4f;

     static public Slingshot S;
     private Vector3 launchPos;
     private GameObject projectile;
     private bool aimingMode;
     private Pause_Menu pauseMenu;


     /*************************************************************************************************
     *** Start
     *************************************************************************************************/
     void Awake()
     {
          //Set the Slingshot singleton S
          S = this;

          pauseMenu = FindObjectOfType<Pause_Menu>();

          launchPoint.SetActive(false);
          launchPos = launchPoint.transform.position;

     }//void Start


     /*************************************************************************************************
     *** Update
     *************************************************************************************************/
     void Update()
     {
          //If  Slingshot is not aimingMode, dont run this code
          if (!aimingMode) return;

          //Get the current mouse position in 2D screen coordinates
          Vector3 mousePos2D = Input.mousePosition;

          //Convert the mouse position to 3D world coordinates
          mousePos2D.z = -Camera.main.transform.position.z;
          Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

          //Find the delta from the launchPos to the mousePos3D
          Vector3 mouseDelta = mousePos3D - launchPos;

          //Limit mouseDelta to the radius of the Slingshot SphereCollider
          float maxMagnitude = gameObject.GetComponent<SphereCollider>().radius;

          if (mouseDelta.magnitude > maxMagnitude)
          {
               mouseDelta.Normalize();
               mouseDelta *= maxMagnitude;

          }//if

          //Move the projectile to this new position
          Vector3 projPos = launchPos + mouseDelta;
          projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, projPos, Time.deltaTime * velocityMult * 2);

          if (Input.GetMouseButtonUp(0))
          {
               //The mouse has been released
               aimingMode = false;

               projectile.GetComponent<Rigidbody>().isKinematic = false;
               projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
               FollowCam.S.poi = projectile;
               projectile = null;
               MissionDemolition.ShotFired();

          }//if

     }//void Update


     /*************************************************************************************************
     *** OnMouseEnter
     *************************************************************************************************/
     void OnMouseEnter()
     {
          if (!pauseMenu.IsPaused)
               launchPoint.SetActive(true);

     }//void OnMouseEnter


     /*************************************************************************************************
     *** OnMouseExit
     *************************************************************************************************/
     void OnMouseExit()
     {
          launchPoint.SetActive(false);

     }//void OnMouseExit


     /*************************************************************************************************
     *** OnMouseDown
     *************************************************************************************************/
     void OnMouseDown()
     {
          if (!pauseMenu.IsPaused)
          {
               //The player has pressed the mouse button while over Slingshot
               aimingMode = true;

               //Instantiate projectile
               projectile = Instantiate(prefabProjectile) as GameObject;

               //Start it at the launchPoint
               projectile.transform.position = launchPos;

               //Set it to isKinematic for now
               projectile.GetComponent<Rigidbody>().isKinematic = true;

          }//if

     }//void OnMouseDown


}//public class Slingshot