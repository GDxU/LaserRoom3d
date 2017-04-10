using UnityEngine;
using System.Collections;

public class Misc_Saying_Area : MonoBehaviour {

    public string[] text;
    public int[] time;
    public int breakTime=5;
    public bool reshow;
    bool shown;
    void OnTriggerEnter(Collider col)
    {
        if (!shown || reshow)
        {
            if (col.tag == "Player")
            {
                for (int i = 0; i < text.Length; ++i)
                {
                    GameUI.SetSaying(time[i], text[i]);
                    if (breakTime>0)
                    {
                        GameUI.SetSaying(breakTime, "");
                    }
                }
                shown = true;
            }
        }
    }
}
