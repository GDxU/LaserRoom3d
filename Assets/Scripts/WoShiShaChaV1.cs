using UnityEngine;
using System.Collections;


//脚本实现了刚体角色的行走跳跃控制
//参考某js脚本
public class WoShiShaChaV1 : MonoBehaviour {
	public float inputSpeed;//最大的一个方向的速度
	public float inputJumpHeight;//跳跃高度
	public float maxAcceleration;//最大的一个方向的加速度
	CapsuleCollider theCapsule;
	bool grounded;//角色是否在地上

	Vector3 targetVelocity,nowVelocity,changeVelocity;
	// Use this for initialization
	void Start () {
		theCapsule = gameObject.GetComponent<CapsuleCollider> ();
	}
	void TrackGrounded(Collision col)
	{
		float minimumHeight=theCapsule.bounds.min.y + theCapsule.radius*0.293F;
		//获取角色脚的高度为胶囊体的底面加上半径的1-2分之根号2倍
		//这样角色可以在不陡于45度的坡上跳
		foreach (ContactPoint c in col.contacts)
		{
			//若有比脚低的接触点
			if (c.point.y < minimumHeight) 
			{
				//we track velocity for rigidbodies we're standing on
				//if (col.rigidbody) groundVelocity = col.rigidbody.velocity;
				//and become children of non-rigidbody colliders
				//else transform.parent = col.transform;
				//上面的代码有bug
				//则角色着地
				grounded = true;
			}
		}
	}
	// FixedUpdate is not called once per frame
	void FixedUpdate () {
		//目标速度为方向键输入
		targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		//让目标速度方向和角色坐标系相同
		targetVelocity = inputSpeed*transform.TransformDirection(targetVelocity);
		//获取当前速度，计算应有的速度改变
		nowVelocity = GetComponent<Rigidbody>().velocity;
		changeVelocity = targetVelocity - nowVelocity;
		//改变过大时受最大加速度控制
		changeVelocity.x = Mathf.Clamp (changeVelocity.x, -maxAcceleration, maxAcceleration);
		changeVelocity.z = Mathf.Clamp (changeVelocity.z, -maxAcceleration, maxAcceleration);
		changeVelocity.y = 0;
		//速度改变乘上定值10施加为物体的加速度，10由手感决定
		GetComponent<Rigidbody>().AddForce(changeVelocity*10,ForceMode.Acceleration);
		//如果着地，允许跳
		if(grounded)
		{
			if (Input.GetButton("Jump"))
			{
				//由运动学公式v^2=2gh推出需要的速度
				GetComponent<Rigidbody>().velocity=new Vector3(nowVelocity.x,Mathf.Sqrt(2 * inputJumpHeight * -Physics.gravity.y),nowVelocity.z);
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

}
