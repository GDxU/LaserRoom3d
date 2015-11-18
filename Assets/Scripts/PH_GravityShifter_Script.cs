using UnityEngine;
using System.Collections;

public class PH_GravityShifter_Script : MonoBehaviour {

    Vector3 normal;
	// Use this for initialization
	void Start () {
        //根据模型坐标位置计算表面法线
        normal = Vector3.Normalize(transform.up - transform.right);
	}
	void OnCollisionEnter(Collision collision)
    {
        //首先必须是Player
        if (collision.gameObject.layer != 28) return;
        if (Gameplay.isTemporaryGravity) return;
        //Debug.Log(Vector3.Angle(Gameplay.playerGravityDir, -normal));
        //50度是临界阈值，超过则不能重力转换
        if(Vector3.Angle(Gameplay.playerGravityDir,-normal)<=50f)
        {
            Gameplay.isTemporaryGravity = true;
            //临时重力下不能跳跃
            Game.player.GetComponent<Character_WalkingScript>().canJump = false;
            //先把目前的重力方向记录下来以便恢复
            Gameplay.saveGravityDir = Gameplay.playerGravityDir;
            //Game.player.SendMessage("ChangeGravity", -normal);
            Game.player.GetComponent<Character_GravityManager>().ChangeGravity(-normal, false);
            Gameplay.TemporaryGravityEntryTime = Gameplay.TemporaryGravityKeepTime = Time.time;
        }
        //Game.debugText.text = collision.collider.name + Time.time.ToString();
    }
    void OnCollisionStay(Collision collision)
    {
        if (Gameplay.isTemporaryGravity&& Vector3.Angle(Gameplay.playerGravityDir, -normal) < 0.01f)
        {
            //延长临时重力时间
            Gameplay.TemporaryGravityKeepTime = Time.time;
        }
        //Game.debugText.text = collision.collider.name + Time.time.ToString();
    }
}
