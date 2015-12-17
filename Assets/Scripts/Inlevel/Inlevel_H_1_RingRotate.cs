using UnityEngine;
using System.Collections;

public class Inlevel_H_1_RingRotate : MonoBehaviour {
    public GameObject ring1, ring2, ring3;
    //public float time = 2f;
    public int thershold = 1;
    int currenthold = 0;
    //Vector3 startPosition, endPosition;
    //float currentSpeed, currentValue = 0;
    //float activeTime;
    bool on;
    public void Enable()
    {
        if (on) return;
        on = true;
        //activeTime = Time.time;
        ring1.SendMessage("On");
        ring2.SendMessage("On");
        ring3.SendMessage("On");
        StartCoroutine(Common.WaitAndAction(OpenDoor, 10));

    }
    public void On()
    {
        ++currenthold;
        if (currenthold >= thershold)
        {
            Enable();
        }
    }
    void OpenDoor()
    {
        this.SendMessage("On");
    }
    public void Off()
    {
        --currenthold;
    }
    // Use this for initialization
    void Start()
    {
        if (currenthold >= thershold)
        {
            Enable();
        }
    }
}
