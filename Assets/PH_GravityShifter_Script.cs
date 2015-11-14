using UnityEngine;
using System.Collections;

public class PH_GravityShifter_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnCollisionEnter(Collision collision)
    {
        //Game.debugText.text = collision.collider.name + Time.time.ToString();
    }
    void OnCollisionStay(Collision collision)
    {
        //Game.debugText.text = collision.collider.name + Time.time.ToString();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
