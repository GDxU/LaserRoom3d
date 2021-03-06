﻿using UnityEngine;
using System.Collections;


//脚本实现了刚体角色的行走跳跃控制
//参考某js脚本
public class Character_WalkingScript : MonoBehaviour
{
	public Camera cam;
	public bool canRun = true;
	public bool canJump = true;
	public float inputSpeed = 16;
	public float runSpeed = 32;
	public float crouchSpeed = 8;//最大的一个方向的速度
	public float inputJumpHeight = 12;//跳跃高度
	public float maxAcceleration = 40;//最大的一个方向的加速度
	public float crouchScale = 0.5f;
	public bool freezeMoving;
    public float minimumHeight;
	CapsuleCollider theCapsule;
	bool grounded;//角色是否在地上
	bool isCrouching;
	bool crouched;
    float iniScaleY;

	Vector3 targetVelocity,nowVelocity,changeVelocity;
	// Use this for initialization
	void Start () {
		theCapsule = gameObject.GetComponent<CapsuleCollider> ();
        iniScaleY = theCapsule.transform.localScale.y;
        //获取角色脚的高度为胶囊体的底面加上半径的1-2分之根号2倍
        //多重力下为相对高度
        //这样角色可以在不陡于45度的坡上跳
        minimumHeight = theCapsule.radius * 0.293F + transform.InverseTransformPoint(theCapsule.bounds.min).y;
    }
	void TrackGrounded(Collision col)
    {
		foreach (ContactPoint c in col.contacts)
        {
			//若有比脚低的接触点
			if (transform.InverseTransformPoint(c.point).y < minimumHeight)
            {
				//we track velocity for rigidbodies we're standing on
				//if (col.rigidbody) groundVelocity = col.rigidbody.velocity;
				//and become children of non-rigidbody colliders
				//else transform.parent = col.transform;
				//上面的代码有bug
				//则角色着地
				grounded = true;
            }
            //Game.debugText.text = Gameplay.isTemporaryGravity.ToString() + '\n' + Gameplay.TemporaryGravityKeepTime.ToString();
            //若在临时重力场中在地面落脚
            if(Gameplay.isTemporaryGravity)
            {
                //若时间太短容易误判 0.5s依据手感确定
                if (Time.time - Gameplay.TemporaryGravityEntryTime < 0.5f) continue;
                //Debug.Log(Vector3.Angle(-c.normal, Gameplay.playerGravityDir).ToString());
                if(Vector3.Angle(-c.normal,Gameplay.playerGravityDir)<=50f)
                {
                    if (Mathf.Abs(c.normal.x) + Mathf.Abs(c.normal.y) + Mathf.Abs(c.normal.z) <= 1.001f)//保证表面水平或垂直
                    {
                        if (c.otherCollider.gameObject.layer < 29)//Not GravityShifterPlane And NonLandSurface
                        {
                            Gameplay.isTemporaryGravity = false;
                            //Change gravity to -collision's normal
                            gameObject.GetComponent<Character_GravityManager>().ChangeGravity(-c.normal, true);
                            //gameObject.SendMessage("ChangeGravity", -c.normal);
                            //Enable Jump
                            canJump = true;
                        }
                    }
                }
            }
		}
	}
	// FixedUpdate is not called once per frame
	void FixedUpdate () {
		//下蹲或起立的时候毛都别想做
		if (isCrouching)
			return;
		//if (crouched && !Input.GetKey(KeyCode.LeftControl) && CanStandUp())
		//这一句再说吧………………
		if (!Input.GetKey(KeyCode.LeftControl) && crouched)
		{
			isCrouching = true;
			StartCoroutine(Crouch(false));
			return;
		}

		if (Input.GetKey(KeyCode.LeftControl) && !crouched)
		{
			isCrouching = true;
			StartCoroutine(Crouch(true));
			return;
		}

		//目标速度为方向键输入
		targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		//让目标速度方向和角色坐标系相同，速度根据跑步和下蹲
        targetVelocity *= (crouched ? crouchSpeed : (canRun ? (Input.GetKey(KeyCode.LeftShift) ? runSpeed : inputSpeed) : inputSpeed));
		//获取当前速度，计算应有的速度改变
        nowVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        changeVelocity = targetVelocity - nowVelocity;
		//改变过大时受最大加速度控制
		changeVelocity.x = Mathf.Clamp (changeVelocity.x, -maxAcceleration, maxAcceleration);
		changeVelocity.z = Mathf.Clamp (changeVelocity.z, -maxAcceleration, maxAcceleration);
		changeVelocity.y = 0;
		//速度改变乘上定值10施加为物体的加速度，10由手感决定
		if (!freezeMoving)
			GetComponent<Rigidbody>().AddForce(transform.TransformDirection(changeVelocity*10),ForceMode.Acceleration);
		//如果着地，允许跳
		if(grounded && canJump && !crouched)
		{
			if (Input.GetButton("Jump"))
			{
				//由运动学公式v^2=2gh推出需要的速度 bug

                //GetComponent<Rigidbody>().velocity = nowVelocity - Level.playerGravityDir * Mathf.Sqrt(2 * inputJumpHeight * -Physics.gravity.y);
                //更改公式，适应多重力场
                GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(nowVelocity.x, Mathf.Sqrt(2 * inputJumpHeight * -Physics.gravity.y), nowVelocity.z));
            }
			grounded=false;
		}
	}
	void OnCollisionEnter(Collision col){
		TrackGrounded (col);
	}
	void OnCollisionStay(Collision col){
		TrackGrounded (col);
	}

	//通过逐帧改变尺寸来下蹲
	IEnumerator Crouch(bool down)
	{
		int frames = 20;
		float scaleChangePerFrame = iniScaleY * crouchScale/frames;
		if (!down)
			scaleChangePerFrame = -scaleChangePerFrame;
		for (int i = 0; i < frames; i++)
		{
			theCapsule.transform.localScale += new Vector3(0, -scaleChangePerFrame,0);
			yield return 1;
			//目前只通过改尺寸进行下蹲，有时候摄像机会透视
			//于是我稍微改低了视角
		}
		isCrouching = false;
		crouched = down;
	}

}
