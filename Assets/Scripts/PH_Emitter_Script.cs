using UnityEngine;
using System.Collections;

public class PH_Emitter_Script : MonoBehaviour {
	//Main main;
	GameObject emitterLight;
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		emitterLight=(GameObject)Instantiate (Game.linePrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
		//保证光线跟着发射器走，先设爸爸
		emitterLight.transform.parent = this.gameObject.transform;
		//实例化一条新光线，位置和方向都为发射器的属性
		emitterLight.SendMessage ("Activate");
		//然后激活
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
