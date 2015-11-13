using UnityEngine;
using System.Collections;

public class PickAndDragV2 : MonoBehaviour {
	
	//Main main;
	public GUIText crosshair;
	//public GUIText texter;
	public Camera cam;
	public Vector3 anchorPos=new Vector3(0,2,0);
	public float springSpring=10F;
	public float springDamper=0.2F;
	public float springMaxDistance=0.5F;
	public float armLength=15;
	public float maxAngularSpeed = 1;
	public float grabDrag = 2;
	public float rotationSensitivity = 2f;
	SpringJoint theSpring;
	bool canDrag,isDraging;
	Transform transformRotation;
	
	Transform transformGrabbed;
	GameObject rotationDetector;
	bool isGrabbing;
	float iniDrag;
	
	
	
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		
		rotationDetector = (GameObject)Instantiate (new GameObject ());
		rotationDetector.transform.parent = cam.transform;
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
			//main.debugText.text=thit.collider.gameObject.name + ":" + thit.point.ToString("f6");
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
            Game.CursorLocker = true;
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
		
		//指针指向一个可以被拖动的刚体的时候，QE可进行旋转
		/*if (canDrag)
		{
			transformRotation = thit.transform;
			if (Input.GetKey (KeyCode.Q))
				transformRotation.rigidbody.AddTorque(-transform.rigidbody.angularVelocity.x, maxAngularSpeed -transform.rigidbody.angularVelocity.y,-transform.rigidbody.angularVelocity.z,ForceMode.VelocityChange);
			if (Input.GetKey(KeyCode.E))
				transformRotation.rigidbody.AddTorque(-transform.rigidbody.angularVelocity.x, -maxAngularSpeed - transform.rigidbody.angularVelocity.y,-transform.rigidbody.angularVelocity.z,ForceMode.VelocityChange);

		}*/
		if(Input.GetMouseButtonUp(0))
		{
			Destroy(theSpring);
			Game.debugText.text="";
			isDraging=false;
		}
		
		if (!isDraging && !isGrabbing && Input.GetMouseButtonDown(1))
        {
            Game.CursorLocker = true;
			if (canDrag)
			{
				isGrabbing = true;
				transformGrabbed = thit.transform;
				transformGrabbed.GetComponent<Rigidbody>().useGravity = false;
				//建立一个物体，让这个物体随着cam改变自己角度，以便把角度传给拿起的物体
				rotationDetector.transform.position = transformGrabbed.position;
				rotationDetector.transform.rotation = transformGrabbed.rotation;
				iniDrag = transformGrabbed.GetComponent<Rigidbody>().drag;//把自己的drag传出来以便恢复
				transformGrabbed.GetComponent<Rigidbody>().drag = grabDrag;//加一个阻力让它能够快速停下来,数值？做实验吧……
			}
		}
		if (isGrabbing)
		{
			if (Input.GetKey(KeyCode.J))
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, Vector3.up, rotationSensitivity);
			if (Input.GetKey(KeyCode.L))
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, Vector3.up, -rotationSensitivity);
			if (Input.GetKey(KeyCode.I))
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.rotation * Vector3.right, rotationSensitivity);
			if (Input.GetKey(KeyCode.K))
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.rotation * Vector3.right, -rotationSensitivity);
			
			transformGrabbed.rotation = rotationDetector.transform.rotation;
			Vector3 targetPoint = 10 * cam.transform.forward.normalized + cam.transform.position;
			Vector3 iniPoint= transformGrabbed.position;
			Vector3 force = (targetPoint-iniPoint).normalized * Vector3.Distance (iniPoint, targetPoint) * Vector3.Distance (iniPoint, targetPoint);
			transformGrabbed.GetComponent<Rigidbody>().AddForce(force);
			
		}
		
		if (isGrabbing && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonDown (0) || Vector3.Distance (transformGrabbed.position, cam.transform.position)>20))
		{
			transformGrabbed.GetComponent<Rigidbody>().drag = iniDrag;
			transformGrabbed.GetComponent<Rigidbody>().useGravity = true;
			isGrabbing = false;
		}
		
	}
}