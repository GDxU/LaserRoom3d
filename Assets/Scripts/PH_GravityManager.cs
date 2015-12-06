using UnityEngine;
using System.Collections;

public class PH_GravityManager : MonoBehaviour {
    public Transform initGravityDir;// self if default gravity.
    [HideInInspector]
    public Vector3 PHGravityDir;
    ConstantForce PHGravity;
    Rigidbody PHRigidBody;
	// Use this for initialization
	void Awake () {
        PHRigidBody = GetComponent<Rigidbody>();
        if (initGravityDir == transform)
            PHGravityDir = Vector3.Normalize(Physics.gravity);
        else
        {
            PHGravityDir = -initGravityDir.up;
            DestroyObject(initGravityDir.gameObject);
        }
        PHGravity = gameObject.AddComponent<ConstantForce>();
        PHGravity.force = PHGravityDir * PHRigidBody.mass * -Physics.gravity.y;
	}
	public void ChangeGravity(Vector3 newDir)
    {
        //直接转换重力
        PHGravityDir = newDir;
        PHGravity.force = PHGravityDir * PHRigidBody.mass * -Physics.gravity.y;
    }
}
