using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public enum GameMode
{
    idle,
    playing,
    levelEnd

}//public enum GameMode


public class MissionDemolition : MonoBehaviour
{
    /*************************************************************************************************
    *** Variables
    *************************************************************************************************/
    static public MissionDemolition S; //A singleton
    public GameObject[] castles;
    public Text textLevel;
    public Text textScore;
    public Vector3 castlePosition;

    private int curLevel; //The current level
    private int maxLevel; //The number of levels
    private int shotsTaken;
    private GameObject curCastle; //The current castle
    private GameMode mode = GameMode.idle;
    private string showing = "Slingshot"; //FollowCam mode

	
    /*************************************************************************************************
    *** Start
    *************************************************************************************************/
    void Start ()
    {
	   S = this;
	   curLevel = 0;
	   maxLevel = castles.Length;
	   StartLevel();
    
    }//void Start
    
    
    /*************************************************************************************************
    *** Update
    *************************************************************************************************/
    void Update ()
    {
	   ShowGT();

	   //Check for level end
	   if (mode == GameMode.playing && Goal.goalMet)
	   {
		  //Change mode to stop checking for level end
		  mode = GameMode.levelEnd;

		  //Zoom out
		  SwitchView("Both");

		  //Start the next level in 2 seconds
		  Invoke("NextLevel", 2f);

	   }//if
    
    }//void Update


    /*************************************************************************************************
    *** Methods
    *************************************************************************************************/
    void StartLevel ()
    {
	   //Get rid of the old castle if one exists
	   if (curCastle != null)
		  Destroy(curCastle);

	   //Destroy old projectiles if they exists
	   GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");

	   foreach (GameObject pTemp in gos)
		  Destroy(pTemp);

	   //Instantiate the new castle
	   curCastle = Instantiate(castles[curLevel]) as GameObject;
	   curCastle.transform.position = castlePosition;
	   shotsTaken = 0;

	   //Reset the camera
	   SwitchView("Both");
	   ProjectileLine.S.Clear();

	   //Reset the goal
	   Goal.goalMet = false;

	   ShowGT();

	   mode = GameMode.playing;

    }//void StartLevel


    void ShowGT ()
    {
	   //Show the data in the Texts
	   textLevel.text = "Level: " + (curLevel + 1) + " of " + maxLevel;
	   textScore.text = "Shots Taken: " + shotsTaken;

    }//void ShowGT


    void NextLevel ()
    {
	   curLevel++;

	   if (curLevel == maxLevel)
		  curLevel = 0;

	   StartLevel();

    }//void NextLevel


    void OnGUI ()
    {
	   //Draw the GUI button for view switching at the top of the screen
	   Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);

	   switch (showing)
	   {
		  case "Slingshot":
			 if (GUI.Button(buttonRect, "Show Castle"))
				SwitchView("Castle");
			 break;

		  case "Castle":
			 if (GUI.Button(buttonRect, "Show Both"))
				SwitchView("Both");
			 break;

		  case "Both":
			 if (GUI.Button(buttonRect, "Show Slingshot"))
				SwitchView("Slingshot");
			 break;

	   }//switch

    }//void OnGUI


    //Static method that allows code anywhere to request a view change
    static public void SwitchView (string eView)
    {
	   S.showing = eView;
	   switch (S.showing)
	   {
		  case "Slingshot":
			 FollowCam.S.poi = null;
			 break;

		  case "Castle":
			 FollowCam.S.poi = S.curCastle;
			 break;

		  case "Both":
			 FollowCam.S.poi = GameObject.Find("ViewBoth");
			 break;

	   }//switch

    }//static public void SwitchView


    //Static method that allows code anywhere to increment shotsTaken
    public static void ShotFired ()
    {
	   S.shotsTaken++;

    }//public static void ShotFired 


}//public class MissionDemolition