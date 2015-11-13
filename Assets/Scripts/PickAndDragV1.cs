using UnityEngine;
using System.Collections;

public class PickAndDragV1 : MonoBehaviour {
	
	//Main main;
	public GUIText crosshair;
	//public GUIText texter;
	public Camera cam;
	public Vector3 anchorPos=new Vector3(0,1,0);
	public float springSpring=2F;
	public float springDamper=0.2F;
	public float springMaxDistance=0.5F;
	public float armLength=3;
	SpringJoint theSpring;
	bool canDrag,isDraging;
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		
	}
	// Update is called once per frame
	void Update () {
		//Ray tray=cam.ScreenPointToRay(Input.mousePosition);
		Ray tray = cam.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2, 0));
		RaycastHit thit;
		canDrag = Physics.Raycast (tray, out thit, armLength);
		if(canDrag)
		{
			Rigidbody theBody=thit.collider.GetComponent<Rigidbody>();
			if(theBody==null || theBody.isKinematic) canDrag=false;
		}
		if(canDrag)
		{
			Game.debugText.text=thit.collider.gameObject.name + ":" + thit.point.ToString("f6");
			crosshair.color=Color.green;
		}
		else
		{
			if(isDraging==false)
			{
				Game.debugText.text="";
				crosshair.color=Color.white;
			}

		}
		if(Input.GetMouseButtonDown(0))
		{
			//Screen.lockCursor=true;
			if(canDrag)
			{
				theSpring=gameObject.AddComponent<SpringJoint>();
				theSpring.anchor=anchorPos;
				theSpring.connectedBody=thit.collider.GetComponent<Rigidbody>();
				theSpring.connectedAnchor=thit.point;
				theSpring.spring=springSpring;
				theSpring.damper=springDamper;
				theSpring.maxDistance=springMaxDistance;
				isDraging=true;
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			Destroy(theSpring);
			Game.debugText.text="";
			isDraging=false;
		}

	}
}
