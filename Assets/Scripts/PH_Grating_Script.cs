using UnityEngine;
using System.Collections;

public class PH_Grating_Script : MonoBehaviour {
	//Main main;
	GameObject gratingPlane;
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		gratingPlane=(GameObject)Instantiate (Game.gratingPlanePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
		gratingPlane.transform.parent = this.transform;
		//根据需要修改这个预设角度
		gratingPlane.transform.localEulerAngles=new Vector3 (90, 0, 0);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
