using UnityEngine;
using System.Collections;

public class ActivateSimpleMove : MonoBehaviour {
	
	//Main main;
	public GameObject objStart,objEnd;
	public float moveTime=1F;
	Vector3 vStart,vEnd;
	bool isOn;
	float temp;

	AudioSource doorAudio;
	public void On()
	{
		//Debug.Log ("On");
		isOn = true;
	}
	public void Off()
	{
		//Debug.Log ("Off");
		isOn = false;
	}
	// Use this for initialization
	void Start () {
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		vStart = objStart.transform.position;
		vEnd = objEnd.transform.position;
		Destroy (objStart);
		Destroy (objEnd);

		doorAudio=gameObject.AddComponent<AudioSource> ();
		doorAudio.enabled = false;
		doorAudio.loop = true;
		doorAudio.minDistance = 1;
		//doorAudio.clip= main.soundDoorMoving;

	}
	
	// Update is called once per frame
	void Update () {
		if(isOn)
		{
			if(temp<1)
			{
				temp=temp+Time.deltaTime/moveTime;
				if(temp>=1)
				{
					doorAudio.enabled=false;
					//AudioSource.PlayClipAtPoint(main.soundDoorOpen,transform.position);
					temp=1;
				}
				else doorAudio.enabled=true;
				this.transform.position=Vector3.Lerp(vStart,vEnd,temp);
			}
		}
		else
		{
			if(temp>0)
			{
				temp=temp-Time.deltaTime/moveTime;
				if(temp<=0)
				{
					doorAudio.enabled=false;
					//AudioSource.PlayClipAtPoint(main.soundDoorClose,transform.position);
					temp=0;
				}
				else doorAudio.enabled=true;
				this.transform.position=Vector3.Lerp(vStart,vEnd,temp);
			}
		}

	}
}
