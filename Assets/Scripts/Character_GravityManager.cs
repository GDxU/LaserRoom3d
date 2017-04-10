using UnityEngine;
using System.Collections;

public class Character_GravityManager : MonoBehaviour
{
    //默认浮空临时重力时间
    public float maxTemporaryGravityTime = 0.75f;//别在这里设没用的
    Rigidbody playerRigidBody;
    //使用恒力来模拟重力
    //因为游戏中会改变重力
    ConstantForce playerGravity;
    float rotSpeed;
	// Use this for initialization
	void Start () {
        Game.player = transform;
        Gameplay.playerGravityDir = -transform.up;
        playerRigidBody = GetComponent<Rigidbody>();
        playerGravity = GetComponent<ConstantForce>();
        //重力公式G=mg
        //重力加速度g由Physics.gravity.y决定
        playerGravity.force = Gameplay.playerGravityDir * playerRigidBody.mass * -Physics.gravity.y;
	}
	public void ChangeGravity(Vector3 newDir,bool isPermanentGravity)
    {
        Gameplay.playerGravityDir = newDir;
        playerGravity.force = Gameplay.playerGravityDir * playerRigidBody.mass * -Physics.gravity.y;
        if (isPermanentGravity)
        {
            Transform grabbedObject = GetComponent<Character_PickAndDrag>().transformGrabbed;
            if (grabbedObject != null)
            {
                //手持物体一起改变重力方向
                grabbedObject.GetComponent<PH_GravityManager>().ChangeGravity(newDir);
            }
        }
        rotSpeed = 0;
        //Game.debugLine.enabled = true;
        //Game.debugLine.SetPosition(0, transform.position);
        //Game.debugLine.SetPosition(1, transform.position + Vector3.Cross(oldDir, newDir)*10);
        //Game.debugText.text = Vector3.Cross(oldDir, newDir).ToString();
        //transform.Rotate(transform.InverseTransformDirection(Vector3.Cross(oldDir, newDir)), Vector3.Angle(oldDir, newDir));
    }
	// Update is called once per frame
	void Update () {

        float rotAngle = -Vector3.Angle(-transform.up, Gameplay.playerGravityDir);
        //float maxAngle = Time.deltaTime * 90;
        if(Mathf.Abs(rotAngle)>=1e-3f)
        {
            float newrot = Mathf.SmoothDamp(rotAngle, 0, ref rotSpeed, 0.25f);
            //最后那个弛豫时间，由手感决定
            //Debug.Log(rotAngle);
            transform.Rotate(transform.InverseTransformDirection(Vector3.Cross(-transform.up, Gameplay.playerGravityDir)),newrot-rotAngle /*Mathf.Clamp(rotAngle, -maxAngle, maxAngle)*/);
        }
        if (Gameplay.isTemporaryGravity)
        {
            //临时重力下浮空超时
            if (Time.time - Gameplay.TemporaryGravityKeepTime > maxTemporaryGravityTime)
            {
                //退出临时重力模式，允许跳跃
                ChangeGravity(Gameplay.saveGravityDir,true);
                Gameplay.isTemporaryGravity = false;
                Game.player.GetComponent<Character_WalkingScript>().canJump = true;
            }
        }
	}
}
