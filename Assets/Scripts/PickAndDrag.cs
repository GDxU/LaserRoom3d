using UnityEngine;
using System.Collections;

public class PickAndDrag : MonoBehaviour {
	
	//Main main;
	public Camera cam;
	public Vector3 anchorPos=new Vector3(0,1,0);
	public float springSpring=2F;
	public float springDamper=0.2F;
	public float springMaxDistance=0.5F;
	public float armLength=3;
	public float maxAngularSpeed = 1;
	public float grabDrag = 2;
	public float rotationSensitivity = 2f;
	SpringJoint theSpring;
	bool canDrag,isDraging;
	Transform transformRotation;
	
	//CapsuleCollider theCapsule;
	Transform transformGrabbed;
	GameObject rotationDetector;
	bool isGrabbing;
	float iniDrag;
	
	bool startGrabbing;
	
	[HideInInspector]
	public bool disableThrowing;
	
	// Use this for initialization
	void Start () {
		//加载Main
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		
		//初始化一些重要的变量
		//theCapsule = gameObject.GetComponent<CapsuleCollider> ();
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
		if(canDrag && !isGrabbing)
		{
			//main.debugText.text=thit.collider.gameObject.name + ":" + thit.point.ToString("f6");
            Game.crosshair.color = Color.green;
		}
		else
		{
			if(!isDraging && !isGrabbing)
			{
				//main.debugText.text="";
				Game.crosshair.color=Color.white;
			}
		}
		if(Input.GetMouseButtonDown(0) && !isGrabbing)
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
		
		//指针指向一个可以被拖动的刚体的时候，QE可进行旋转
		/*if (canDrag)
		{
			transformRotation = thit.transform;
			if (Input.GetKey (KeyCode.Q))
				transformRotation.rigidbody.AddTorque(-transform.rigidbody.angularVelocity.x, maxAngularSpeed -transform.rigidbody.angularVelocity.y,-transform.rigidbody.angularVelocity.z,ForceMode.VelocityChange);
			if (Input.GetKey(KeyCode.E))
				transformRotation.rigidbody.AddTorque(-transform.rigidbody.angularVelocity.x, -maxAngularSpeed - transform.rigidbody.angularVelocity.y,-transform.rigidbody.angularVelocity.z,ForceMode.VelocityChange);

		}*/
		if(Input.GetMouseButtonUp(0) && isDraging)
		{
			Destroy(theSpring);
			//main.debugText.text="";
			isDraging=false;
		}
		
		if (!isDraging && !isGrabbing && Input.GetMouseButtonDown(1))
		{
			//Screen.lockCursor=true;
			if (canDrag)
			{
				isGrabbing = true;
				transformGrabbed = thit.transform;
				transformGrabbed.GetComponent<Rigidbody>().useGravity = false;
				//建立一个物体，让这个物体随着cam改变自己角度，以便把角度传给拿起的物体
				rotationDetector.transform.position = transformGrabbed.position;
				//rotationDetector.transform.rotation = transformGrabbed.rotation;
				rotationDetector.transform.eulerAngles=transformGrabbed.eulerAngles+20*new Vector3(Random.value-.5f,Random.value-.5f,Random.value-.5f);
				iniDrag = transformGrabbed.GetComponent<Rigidbody>().drag;//把自己的drag传出来以便恢复
				transformGrabbed.GetComponent<Rigidbody>().drag = grabDrag;//加一个阻力让它能够快速停下来,数值？做实验吧……
				startGrabbing = true;
				Game.crosshair.color = Color.yellow;
			}
		}
		if (isGrabbing)
		{
			if (Input.GetMouseButton(0))
			{
				this.gameObject.GetComponent<WalkingScript>().freezeMoving = true;
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, Vector3.up, -rotationSensitivity * Input.GetAxis("Horizontal"));
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.right, rotationSensitivity * Input.GetAxis("Vertical"));
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.forward, rotationSensitivity * ((Input.GetKey (KeyCode.Q)?1:0)-(Input.GetKey (KeyCode.E)?1:0)));
			}
			else
				this.gameObject.GetComponent<WalkingScript>().freezeMoving = false;
			transformGrabbed.rotation = rotationDetector.transform.rotation;
			Vector3 targetPoint = 10 * cam.transform.forward.normalized + cam.transform.position;
			Vector3 iniPoint= transformGrabbed.position;
			Vector3 force = (targetPoint-iniPoint).normalized * Vector3.Distance (iniPoint, targetPoint) * Vector3.Distance (iniPoint, targetPoint);
			transformGrabbed.GetComponent<Rigidbody>().AddForce(force);
		}
		if (startGrabbing)
			startGrabbing = false;
		else if (isGrabbing && (Input.GetMouseButtonDown(1) || Vector3.Distance (transformGrabbed.position, cam.transform.position)>20))
		{
			transformGrabbed.GetComponent<Rigidbody>().drag = iniDrag;
			transformGrabbed.GetComponent<Rigidbody>().useGravity = true;
			isGrabbing = false;
			if (Input.GetMouseButton(0))
			{
				if(disableThrowing)
				{
					Game.debugText.text="The function is disabled in this level.";
				}
				else
				{
					transformGrabbed.GetComponent<Rigidbody>().AddForce(30 * cam.transform.forward, ForceMode.Impulse);
				}
				this.gameObject.GetComponent<WalkingScript>().freezeMoving = false;
			}
		}
		
		//main.debugText.text = isGrabbing.ToString ();
		
	}
}
