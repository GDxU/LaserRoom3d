using UnityEngine;
using System.Collections;

public class Misc_Activate_Smooth_Moving_Rotating : MonoBehaviour {
    PH_GravityShifter_Script shifter;
    bool isShifter;
    public float time = 2f;
    public enum RotateAxis
    {
        x,y,z
    };
    public RotateAxis Axis=RotateAxis.y;
    public bool relativeRotate = false;
    public float degree = 90;

    Vector3 startPosition, endPosition, calcAxis;
    float currentSpeed, currentValue, lastValue;
    float activeTime;
    Quaternion originRotation;
    float rotateSpeed;
    bool on;
    public void On()
    {
        on = true;
        activeTime = Time.time;
    }
    public void Off()
    {
        on = false;
        activeTime = Time.time;
    }
    void GetRelativeAxis()
    {
        if (Axis == RotateAxis.y)
        {
            calcAxis = transform.up;
        }
        else if (Axis == RotateAxis.x)
        {
            calcAxis = transform.right;
        }
        else if (Axis == RotateAxis.z)
        {
            calcAxis = transform.forward;
        }

    }
    // Use this for initialization
    void Start()
    {
        if(Axis==RotateAxis.y)
        {
            calcAxis = Vector3.up;
        }
        else if (Axis == RotateAxis.x)
        {
            calcAxis =Vector3.right;
        }
        else if (Axis == RotateAxis.z)
        {
            calcAxis = Vector3.forward;
        }
        Transform start = transform.FindChild("MoveStart"), end = transform.FindChild("MoveEnd");
        startPosition = transform.position;
        endPosition = transform.position + (end.position - start.position);
        originRotation = transform.rotation;
        shifter = GetComponent<PH_GravityShifter_Script>();
        if (shifter != null) isShifter = true;
    }

    // Update is called once per frame
    void Update()
    {
        //节约资源，超过5倍时间则不计算
        if (Time.time - activeTime < 5 * time)
        {
            currentValue = Mathf.SmoothDamp(currentValue, on ? 1 : 0, ref currentSpeed, time);
            transform.position = Vector3.Lerp(startPosition, endPosition, currentValue);
            if (relativeRotate)
            {
                //相对旋转
                GetRelativeAxis();
                transform.Rotate(calcAxis, (currentValue - lastValue) * degree, Space.World);
            }
            else
            {
                transform.rotation = originRotation;
                transform.Rotate(calcAxis, currentValue * degree, Space.World);
            }
            if(isShifter)
            {
                //旋转后重新计算法线
                shifter.RecalculateNormal();
            }
            lastValue = currentValue;
        }
    }
}
