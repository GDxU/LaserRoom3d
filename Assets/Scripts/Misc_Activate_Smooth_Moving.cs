using UnityEngine;
using System.Collections;

public class Misc_Activate_Smooth_Moving : MonoBehaviour
{
    public float time = 2f;
    public int thershold = 1;
    int currenthold = 0;
    Vector3 startPosition, endPosition;
    float currentSpeed, currentValue = 0;
    float activeTime;
    bool on;
    public void On()
    {
        ++currenthold;
        if (currenthold == thershold)
        {
            on = true;
            activeTime = Time.time;
        }
    }
    public void Off()
    {
        --currenthold;
        if (currenthold != thershold)
        {
            on = false;
            activeTime = Time.time;
        }
    }
	// Use this for initialization
	void Start () {
        Transform start = transform.FindChild("MoveStart"), end = transform.FindChild("MoveEnd");
        startPosition = transform.position;
        endPosition = transform.position + (end.position - start.position);
	}
	
	// Update is called once per frame
	void Update () {
        //节约资源，超过5倍时间则不计算
        if (Time.time-activeTime<5*time)
        {
            currentValue = Mathf.SmoothDamp(currentValue, on ? 1 : 0, ref currentSpeed, time);
            transform.position = Vector3.Lerp(startPosition,endPosition,currentValue) ;
        }
	}
}
