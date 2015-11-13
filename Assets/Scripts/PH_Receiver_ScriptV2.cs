using UnityEngine;
using System.Collections;

public class PH_Receiver_ScriptV2: MonoBehaviour {
	
	//Main main;
	public AudioClip soundActivation;

	public GameObject attachedObject;
	public bool cantTurnOff;
	public bool isTurnOn;
	bool isOn;
	public void Activate()
	{
		isTurnOn = true;
	}
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(isTurnOn==false && isOn==true && !cantTurnOff)
		{
			attachedObject.SendMessage("Off");
			isOn=false;
		}
		if(isTurnOn==true && isOn==false)
		{
			attachedObject.SendMessage("On");
			isOn=true;
			//AudioSource.PlayClipAtPoint(soundActivation, this.transform.position);
		}
		isTurnOn = false;
	}
}
