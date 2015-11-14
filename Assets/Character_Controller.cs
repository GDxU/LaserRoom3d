using UnityEngine;
using System.Collections;

public class Character_Controller : MonoBehaviour {
    Rigidbody playerRigidBody;
    //使用恒力来模拟重力
    //因为游戏中会改变重力
    ConstantForce playerGravity;
	// Use this for initialization
	void Start () {
        Game.player = transform;
        Level.playerGravityDir = -transform.up;
        playerRigidBody = GetComponent<Rigidbody>();
        playerGravity = GetComponent<ConstantForce>();
        //重力公式G=mg
        //重力加速度g由Physics.gravity.y决定
        playerGravity.force = Level.playerGravityDir * playerRigidBody.mass * -Physics.gravity.y;

	}
	public void ChangeGravity(Vector3 newDir)
    {

    }
	// Update is called once per frame
	void Update () {
	
	}
}
