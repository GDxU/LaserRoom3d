using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class GameUI 
{
    public static Canvas canvas;
    public static Text hintText;
    public static Text sayingText;
    public static Text crosshair;
    public static Text youfinishText;
    public static bool ingameUI;
    public static Queue<string> sayingList = new Queue<string>();
    public static Queue<int> sayingTimeList = new Queue<int>();
    public static int hintTextDisplayed,sayingTextDisplayed; //包括延时，每帧值-1，减到0就消失，为图方便实现在Character_PickAndDrag.cs中
    public static void SetHint(int time,string str)
    {
        hintText.text = str;
        hintTextDisplayed = time;
    }
    public static void SetSaying(int time,string saying)
    {
        if(sayingTextDisplayed==0)
        {
            sayingText.text = saying;
            sayingTextDisplayed = time;
        }
        else
        {
            sayingList.Enqueue(saying);
            sayingTimeList.Enqueue(time);
        }
    }
    public static void FixedUpdate()
    {
        if(hintTextDisplayed>0)
        {
            hintTextDisplayed--;
            if (hintTextDisplayed == 0)
            hintText.text = "";
        }
        if(sayingTextDisplayed>0)
        {
            sayingTextDisplayed--;
            if(sayingTextDisplayed==0)
            {
                if (sayingList.Count > 0)
                {
                    sayingText.text = sayingList.Dequeue();
                    sayingTextDisplayed = sayingTimeList.Dequeue();
                }
                else sayingText.text = "";
            }
        }
    }

}
