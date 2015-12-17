using UnityEngine;
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
    public float mouseRotateSensitivity = 5f;
	SpringJoint theSpring;
	int canDrag;//1:canDrag 0:canNotDrag 2:canOnlyPull
    bool isDraging;
	Transform transformRotation;
    public GameObject gravityDirSignPrefab;
    bool UpdateMouseButton0, UpdateMouseButton1;//Use Update() To Store MouseButtonDown To Prevent Bugs In FixedUpdate().
	
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
    bool pointToFixHolder;

    Transform gravityDirSign;
    bool gravityDirSignShowing;
    private LineRenderer DebugLine;

    MouseLook mouselook_script1, mouselook_script2;

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
        mouselook_script1 = GetComponent<MouseLook>();
        mouselook_script2 = GetComponentInChildren<Camera>().GetComponent<MouseLook>();
	}
    void ShowGravityDirOnObject(Transform obj,Vector3 gravityDir)
    {
        gravityDirSign.position = obj.position;
        gravityDirSign.LookAt(obj.position + gravityDir);
    }
    Vector3 GetPHGravity(Rigidbody theBody)
    {
        if (theBody.useGravity == true) return Vector3.Normalize(Physics.gravity);
        else
        {
            PH_GravityManager gravityManager = theBody.GetComponent<PH_GravityManager>();
            if (gravityManager != null)
            {
                return gravityManager.PHGravityDir;
            }
            else return Vector3.zero;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        GameUI.FixedUpdate();
        //Game.debugText.text = "";
        bool gravityDirSignShow=false;
		//Ray tray=cam.ScreenPointToRay(Input.mousePosition);
		Ray tray = cam.ScreenPointToRay(new Vector3 (Screen.width / 2, Screen.height / 2, 0));
		RaycastHit thit;
        canDrag = Physics.Raycast(tray, out thit, armLength) ? 1 : 0;
		if(canDrag==1&&!isGrabbing)
		{
            Rigidbody theBody = thit.collider.GetComponent<Rigidbody>();
            if (theBody == null || (theBody.isKinematic && theBody.tag != "FixedPHBox"))
            {
                canDrag = 0;
                GameUI.crosshair.color = Color.white;
            }
            else if (Gameplay.isTemporaryGravity)
            {
                canDrag = 0;//临时重力下别捡东西
                GameUI.crosshair.color = Color.red;
            }
            else
            {
                Vector3 PHGravity = GetPHGravity(theBody);
                if (Vector3.Magnitude(Gameplay.playerGravityDir - PHGravity) > 1e-3f)//角色与物体的重力方向不同
                {
                    canDrag = 2;//不能捡东西，只能拖东西
                    GameUI.crosshair.color = Color.red;
                }
                if (!isGrabbing && !isDraging)
                {
                    gravityDirSignShow = true;
                    ShowGravityDirOnObject(theBody.transform, PHGravity);
                }
            }
            if(canDrag==1)
            {
                GameUI.crosshair.color = Color.green;
            }
		}
        else
        {
            GameUI.crosshair.color = isGrabbing ? Color.yellow : Color.white;
        }
        if (gravityDirSignShow != gravityDirSignShowing)
        {
            if (gravityDirSignShow)
                gravityDirSign.SendMessage("Enable");
            else
                gravityDirSign.SendMessage("Disable");
            gravityDirSignShowing = gravityDirSignShow;
        }
        if (UpdateMouseButton0 && !isGrabbing)
		{
            Game.CursorLocker = true;
			//Screen.lockCursor=true;
			if(canDrag!=0)
			{
				theSpring=gameObject.AddComponent<SpringJoint>();
                theSpring.anchor = anchorPos;
                thit.collider.GetComponent<Rigidbody>().isKinematic = false;
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

        if (!isDraging && !isGrabbing && UpdateMouseButton1)
        {
            Game.CursorLocker = true;
			if (canDrag==1)
			{
				isGrabbing = true;
				transformGrabbed = thit.transform;
                transformGrabbed.GetComponent<Rigidbody>().isKinematic = false;
				//transformGrabbed.GetComponent<Rigidbody>().useGravity = false;
                transformGrabbed.GetComponent<ConstantForce>().enabled = false;
				//建立一个物体，让这个物体随着cam改变自己角度，以便把角度传给拿起的物体
				rotationDetector.transform.position = transformGrabbed.position;
				//rotationDetector.transform.rotation = transformGrabbed.rotation;
				rotationDetector.transform.eulerAngles=transformGrabbed.eulerAngles+20*new Vector3(Random.value-.5f,Random.value-.5f,Random.value-.5f);
				iniDrag = transformGrabbed.GetComponent<Rigidbody>().drag;//把自己的drag传出来以便恢复
				transformGrabbed.GetComponent<Rigidbody>().drag = grabDrag;//加一个阻力让它能够快速停下来,数值？做实验吧……
				startGrabbing = true;
				GameUI.crosshair.color = Color.yellow;
                if (transformGrabbed.gameObject.layer == 8)//TP_Box Layer
                {
                    transformGrabbed.gameObject.layer = 2;//改变成透明，防止干扰
                }
			}
            else if(canDrag==2)
            {
                GameUI.SetHint(100, "You can only manipulate objects that share same gravitational field with you.");
            }
		}
        canPlace = false;
		if (isGrabbing)
		{
            canPlace=AgainstWallHint();
            if (canPlace)
            {
                //Game.debugText.text = "Right Click To Place Against The Wall.";
                GameUI.SetHint(2,"Right click to place the box aligned.");
                //Game.debugText.text = transformGrabbed.up.ToString();
            }
            if (Input.GetMouseButton(0))
            {
                this.gameObject.GetComponent<Character_WalkingScript>().freezeMoving = true;
                mouselook_script1.enabled = false;
                mouselook_script2.enabled = false;
                //三种旋转方式
                //由于重力可以改变，因此绕transform.up转
                rotationDetector.transform.RotateAround(rotationDetector.transform.position, transform.up, -(rotationSensitivity * Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X")*mouseRotateSensitivity));
                rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.right, rotationSensitivity * Input.GetAxis("Vertical") + Input.GetAxis("Mouse Y") * mouseRotateSensitivity);
                rotationDetector.transform.RotateAround(rotationDetector.transform.position, cam.transform.forward, rotationSensitivity * ((Input.GetKey(KeyCode.Q) ? 1 : 0) - (Input.GetKey(KeyCode.E) ? 1 : 0)));
            }
            else
            {
                mouselook_script1.enabled = true;
                mouselook_script2.enabled = true;
                this.gameObject.GetComponent<Character_WalkingScript>().freezeMoving = false;
            }
			transformGrabbed.rotation = rotationDetector.transform.rotation;
			Vector3 targetPoint = 10 * cam.transform.forward.normalized + cam.transform.position;
			Vector3 iniPoint= transformGrabbed.position;
			Vector3 force = (targetPoint-iniPoint).normalized * Vector3.Distance (iniPoint, targetPoint) * Vector3.Distance (iniPoint, targetPoint);
			transformGrabbed.GetComponent<Rigidbody>().AddForce(force);
		}
		if (startGrabbing)
			startGrabbing = false;
        else if (isGrabbing && (UpdateMouseButton1 || Vector3.Distance(transformGrabbed.position, cam.transform.position) > 20))
        {
            Game.CursorLocker = true;
            ReleaseGrabbed(Input.GetMouseButton(0));
		}
		
		//main.debugText.text = isGrabbing.ToString ();
        UpdateMouseButton0 = false;
        UpdateMouseButton1 = false;
		
	}
    public void ForceReleaseGrabbed(bool throwing)
    {
        if(isGrabbing)
        {
            canPlace = false;
            ReleaseGrabbed(throwing);
        }
        if (isDraging)
        {
            Destroy(theSpring);
            isDraging = false;
        }
    }
    void ReleaseGrabbed(bool throwing)
    {
        if (canPlace&&!throwing)
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
                //Game.debugText.text = "The function is disabled in this level.";
                GameUI.SetHint(250,"You cannot throw object in this level.");
            }
            else
            {
                transformGrabbed.GetComponent<Rigidbody>().AddForce(30 * cam.transform.forward, ForceMode.Impulse);
            }
        }
        mouselook_script1.enabled = true;
        mouselook_script2.enabled = true;
        this.gameObject.GetComponent<Character_WalkingScript>().freezeMoving = false;
        if (transformGrabbed.gameObject.layer==2)//如果改成了IgnoreRayCast
        {
            transformGrabbed.gameObject.layer = 8;//更改回TP_Box
        }
        transformGrabbed = null;
    }
    Vector3 hitPoint, hitNormal, centerPoint;
    Vector3 floorPoint, floorNormal;
    float PhysicsEPS = 0f;
    bool AgainstWallHint()
    {
        //Todo: Disable when collider between me and grabbed object.
        if(Gameplay.isTemporaryGravity)return false;
        Ray tray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0) + transform.forward * 10);
        RaycastHit thit;
        bool isCollide = Physics.Raycast(tray, out thit, armLength,(15<<24));//25~27:Wall Layer 24:FixHolder
        if (!isCollide) return false;
        //DebugLine.SetPosition(0, thit.point);
        //DebugLine.SetPosition(1, thit.normal+thit.point);
        if(thit.transform.gameObject.layer==24)//FixHolder
        {
            pointToFixHolder = true;
            floorPoint = thit.transform.position;
            floorNormal = thit.transform.forward;
            hitNormal = thit.transform.up;
            return true;
        }
        float angle = Vector3.Angle(thit.normal, Gameplay.playerGravityDir);
        if (angle < 88 || angle > 92) return false;
        hitPoint = thit.point;
        hitNormal = thit.normal;
        //卡门不允许
        if(Physics.Raycast(hitPoint,hitNormal,5f,(7<<25)))return false;

        centerPoint = hitPoint + hitNormal * (2.5f+PhysicsEPS);//放置中心点
        isCollide = Physics.Raycast(centerPoint, Gameplay.playerGravityDir, out thit, 6f, (7 << 25) | 1 << 8);//地面和TP_Box顶端
        floorPoint = thit.point;
        floorNormal = thit.normal;
        if (!isCollide) return false;
        if (Vector3.Angle(thit.normal, -Gameplay.playerGravityDir) > 5f) return false;
        //卡地不允许
        if (Physics.Raycast(floorPoint, floorNormal, 5f, (7 << 25))) return false;
        pointToFixHolder = false;
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
        Vector3[] testDir=new Vector3[3];
        //目标up,forward,right
        testDir[0] = floorNormal;
        testDir[1] = hitNormal;
        testDir[2] = Vector3.Cross(testDir[0], testDir[1]);
        DebugLine.SetPosition(0, center);
        //微调靠墙角问题，防止箱子撞墙飞出去
        RaycastHit thit;
        if (Physics.Raycast(center, testDir[2], out thit, 2.5f, (7 << 25)))
            center += testDir[2] * (thit.distance - 2.5f);
        if (Physics.Raycast(center, -testDir[2], out thit, 2.5f, (7 << 25)))
            center += -testDir[2] * (thit.distance - 2.5f);
        DebugLine.SetPosition(1, center);
     
        //Debug.Log(testDir[0].ToString());
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
        Rigidbody theBody=grabbed.GetComponent<Rigidbody>();
        theBody.position = center;
        //静止该物体
        theBody.velocity = Vector3.zero;
        //grabbed.forward = ((minAt & 1) > 0 ? 1 : -1) * testDir[test_y[minAt >> 2]];
        //grabbed.up = ((minAt & 2) > 0 ? 1 : -1) * testDir[test_x[minAt >> 2]];
        //grabbed.right = Vector3.Cross(grabbed.up, grabbed.forward);
        //使用LookAt来设置forward和up，不然会爆炸
        grabbed.LookAt(grabbed.position + ((minAt & 1) > 0 ? 1 : -1) * testDir[test_y[minAt >> 2]], ((minAt & 2) > 0 ? 1 : -1) * testDir[test_x[minAt >> 2]]);

        grabbed.transform.position = center;
        grabbed.rotation = theBody.rotation;
        //theBody.isKinematic = true;
        //grabbed.tag = "FixedPHBox";
        if(pointToFixHolder)
        {
            grabbed.transform.position = center;
            Vector3 gravityDir = GetPHGravity(theBody);
            if (Vector3.Angle(gravityDir,-floorNormal)<=31)
                theBody.isKinematic = true;
            grabbed.tag = "FixedPHBox";
        }
    }
    void Update()
    {
        UpdateMouseButton0 = Input.GetMouseButtonDown(0);
        UpdateMouseButton1 = Input.GetMouseButtonDown(1);
    }
}
