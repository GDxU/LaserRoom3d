using UnityEngine;
using System.Collections;

public class PH_Receiver_Script : MonoBehaviour {
	
	public AudioClip soundActivation;
	public GameObject attachedObject;
	public bool isTurnOn;
	public GameObject activatedReceiverPrefab;
	GameObject activatedReceiver;
	AudioSource doorAudio;
	public bool cantTurnOff;
	bool isOn;
	public void Activate()
	{
		isTurnOn = true;
	}
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		activatedReceiver = (GameObject)Instantiate (activatedReceiverPrefab, transform.position, transform.rotation);
		activatedReceiver.transform.parent = transform;
		activatedReceiver.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {

		if(isTurnOn==false && isOn==true && !cantTurnOff)
		{
			attachedObject.SendMessage("Off");
			activatedReceiver.SetActive(false);
			isOn=false;
		}
		if(isTurnOn==true && isOn==false)
		{
			attachedObject.SendMessage("On");
			activatedReceiver.SetActive(true);
			isOn=true;
			//AudioSource.PlayClipAtPoint(soundActivation, this.transform.position);

		}
		isTurnOn = false;
	}
}
