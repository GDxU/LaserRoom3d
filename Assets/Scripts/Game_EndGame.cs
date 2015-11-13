using UnityEngine;
using System.Collections;

public class Game_EndGame : MonoBehaviour {
	
	//Main main;
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();

	}
	void OnTriggerEnter(Collider col){
		if(col.tag=="Player")
		{
			//main.endTime=Time.time;
			//main.debugText.text="Congratulation!You've Finished.\nTime:"+(main.endTime-main.startTime).ToString()+ " Sec";

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
