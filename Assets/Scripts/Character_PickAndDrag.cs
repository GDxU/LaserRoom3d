﻿using UnityEngine;
using System.Collections;

public class Character_PickAndDrag : MonoBehaviour
{
	
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
    public GameObject gravityDirSignPrefab;
	
	//CapsuleCollider theCapsule;
    [HideInInspector]
	public Transform transformGrabbed;
	[HideInInspector]
	public bool disableThrowing;
	GameObject rotationDetector;
	bool isGrabbing;
	float iniDrag;
	bool startGrabbing;
    bool canPlace;
	

    Transform gravityDirSign;
    bool gravityDirSignShowing;
    private LineRenderer DebugLine;

	// Use this for initialization
	void Start () {
		//加载Main
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		
		//初始化一些重要的变量
		//theCapsule = gameObject.GetComponent<CapsuleCollider> ();
		rotationDetector = (GameObject)Instantiate (new GameObject ());
		rotationDetector.transform.parent = cam.transform;
        gravityDirSign = GameObject.Instantiate(gravityDirSignPrefab).transform;
        gravityDirSign.SendMessage("Disable");
        DebugLine = GetComponent<LineRenderer>();
	}
    void ShowGravityDirOnObject(Transform obj,Vector3 gravityDir)
    {
        gravityDirSign.position = obj.position;
        gravityDirSign.LookAt(obj.position + gravityDir);
    }
	// Update is called once per frame
	void Update () {
        Game.debugText.text = "";
        bool gravityDirSignShow=false;
		//Ray tray=cam.ScreenPointToRay(Input.mousePosition);
		Ray tray = cam.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2, 0));
		RaycastHit thit;
		canDrag = Physics.Raycast (tray, out thit, armLength);
		if(canDrag)
		{
            Rigidbody theBody = thit.collider.GetComponent<Rigidbody>();
            if (theBody == null || theBody.isKinematic) canDrag = false;
            else if(Gameplay.isTemporaryGravity)canDrag=false;//临时重力下别捡东西
            else
            {
                Vector3 PHGravity;
                if (theBody.useGravity == true) PHGravity = Vector3.Normalize(Physics.gravity);
                else
                {
                    PH_GravityManager gravityManager = theBody.GetComponent<PH_GravityManager>();
                    if (gravityManager != null)
                    {
                        PHGravity = gravityManager.PHGravityDir;
                    }
                    else PHGravity = Vector3.zero;
                }
                if(Vector3.Magnitude(Gameplay.playerGravityDir-PHGravity)>1e-3f)//角色与物体的重力方向不同
                {
                    canDrag = false;//不能捡东西
                }
                if (!isGrabbing && !isDraging)
                {
                    gravityDirSignShow = true;
                    ShowGravityDirOnObject(theBody.transform, PHGravity);
                }
            }
		}
        if (gravityDirSignShow != gravityDirSignShowing)
        {
            if (gravityDirSignShow)
                gravityDirSign.SendMessage("Enable");
            else
                gravityDirSign.SendMessage("Disable");
            gravityDirSignShowing = gravityDirSignShow;
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
				//transformGrabbed.GetComponent<Rigidbody>().useGravity = false;
                transformGrabbed.GetComponent<ConstantForce>().enabled = false;
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
        canPlace = false;
		if (isGrabbing)
		{
            canPlace=AgainstWallHint();
            if (canPlace)
            {
                Game.debugText.text = "Right Click To Place Against The Wall.";
                //Game.debugText.text = transformGrabbed.up.ToString();
            }
			if (Input.GetMouseButton(0))
			{
                this.gameObject.GetComponent<Character_WalkingScript>().freezeMoving = true;
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, Vector3.up, -rotationSensitivity * Input.GetAxis("Horizontal"));
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.right, rotationSensitivity * Input.GetAxis("Vertical"));
				rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.forward, rotationSensitivity * ((Input.GetKey (KeyCode.Q)?1:0)-(Input.GetKey (KeyCode.E)?1:0)));
			}
			else
                this.gameObject.GetComponent<Character_WalkingScript>().freezeMoving = false;
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
            ReleaseGrabbed(Input.GetMouseButton(0));
		}
		
		//main.debugText.text = isGrabbing.ToString ();
		
	}
    void ReleaseGrabbed(bool throwing)
    {
        if (canPlace)
        {
            PlaceGrabbed(transformGrabbed);
        }
        transformGrabbed.GetComponent<Rigidbody>().drag = iniDrag;
        //transformGrabbed.GetComponent<Rigidbody>().useGravity = true;
        transformGrabbed.GetComponent<ConstantForce>().enabled = true;
        isGrabbing = false;
        if (throwing)
        {
            if (disableThrowing)
            {
                Game.debugText.text = "The function is disabled in this level.";
            }
            else
            {
                transformGrabbed.GetComponent<Rigidbody>().AddForce(30 * cam.transform.forward, ForceMode.Impulse);
            }
        }
        this.gameObject.GetComponent<Character_WalkingScript>().freezeMoving = false;
        transformGrabbed = null;
    }
    Vector3 hitPoint, hitNormal, centerPoint;
    Vector3 floorPoint, floorNormal;
    float PhysicsEPS = 0.01f;
    bool AgainstWallHint()
    {
        //Todo: Disable when collider between me and grabbed object.
        if(Gameplay.isTemporaryGravity)return false;
        Ray tray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0) + transform.forward * 10);
        RaycastHit thit;
        bool isCollide = Physics.Raycast(tray, out thit, armLength,(7<<25));//25~27:Wall Layer
        if (!isCollide) return false;
        float angle = Vector3.Angle(thit.normal, Gameplay.playerGravityDir);
        if (angle < 88 || angle > 92) return false;
        hitPoint = thit.point;
        hitNormal = thit.normal;
        centerPoint = hitPoint + hitNormal * (2.5f+PhysicsEPS);//放置中心点
        isCollide = Physics.Raycast(centerPoint, Gameplay.playerGravityDir,out thit,6f, (7 << 25));
        floorPoint = thit.point;
        floorNormal = thit.normal;
        if (!isCollide) return false;
        if (Vector3.Angle(thit.normal, -Gameplay.playerGravityDir) > 5f) return false;
        DebugLine.SetPosition(0, thit.point);
        DebugLine.SetPosition(1, centerPoint);
        return isCollide;
    }
    float TestPlaceRotation(Vector3 up, Vector3 forward, Vector3 exp_up, Vector3 exp_forward)
    {
        return Vector3.Angle(up, exp_up) + Vector3.Angle(forward, exp_forward);
    }
    int[] test_x={0,0,1,1,2,2};
    int[] test_y={1,2,0,2,0,1};
    void PlaceGrabbed(Transform grabbed)
    {
        Vector3 center = floorPoint + floorNormal * (2.5f+PhysicsEPS); 
        grabbed.position = center;
        Vector3[] testDir=new Vector3[3];
        //目标up,forward,right
        testDir[0] = floorNormal;
        testDir[1] = hitNormal;
        testDir[2] = Vector3.Cross(testDir[0], testDir[1]);
        Debug.Log(testDir[0].ToString());
        float minTest=9e9f;
        int minAt=-1;
        //找出24种旋转方式中最接近某种目标的旋转方式后设置up和forward
        for (int i = 0; i < 24; ++i)
        {
            float tmp = TestPlaceRotation(grabbed.up, grabbed.forward, ((i & 2) > 0 ? 1 : -1) * testDir[test_x[i >> 2]], ((i & 1) > 0 ? 1 : -1) * testDir[test_y[i >> 2]]);

            if(tmp < minTest)
            {
                minTest = tmp;
                minAt = i;
            }
        }
        //grabbed.forward = ((minAt & 1) > 0 ? 1 : -1) * testDir[test_y[minAt >> 2]];
        //grabbed.up = ((minAt & 2) > 0 ? 1 : -1) * testDir[test_x[minAt >> 2]];
        //grabbed.right = Vector3.Cross(grabbed.up, grabbed.forward);
        //使用LookAt来设置forward和up，不然会爆炸
        grabbed.LookAt(grabbed.position + ((minAt & 1) > 0 ? 1 : -1) * testDir[test_y[minAt >> 2]], ((minAt & 2) > 0 ? 1 : -1) * testDir[test_x[minAt >> 2]]);
    }
}
