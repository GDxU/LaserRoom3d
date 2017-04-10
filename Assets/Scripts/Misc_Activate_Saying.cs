using UnityEngine;
using System.Collections;

public class Misc_Activate_Saying : MonoBehaviour {

    public int thershold = 1;
    public string[] text;
    public int[] time;
    public int breakTime = 5;
    int currenthold = 0;
    bool on;
    public void On()
    {
        ++currenthold;
        if (currenthold >= thershold)
        {
            ActivateSaying();
            Destroy(this);
        }
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
            ActivateSaying();
            Destroy(this);
        }
    }
    void ActivateSaying()
    {
        for (int i = 0; i < text.Length; ++i)
        {
            GameUI.SetSaying(time[i], text[i]);
            if (breakTime > 0)
            {
                GameUI.SetSaying(breakTime, "");
            }
        }
    }
}
