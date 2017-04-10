using UnityEngine;
using System.Collections;

public class Misc_HintArea : MonoBehaviour {

    public string text;
    public int time = 500;
    public bool KeepWhenEnter;
    public bool reshow;
    bool shown;
    void OnTriggerEnter(Collider col)
    {
        if (!shown || reshow)
        {
            if (col.tag == "Player")
            {
                GameUI.SetHint(KeepWhenEnter?-2:time, text.Replace("\\n", "\n"));
                shown = true;
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(KeepWhenEnter)
        {
            if (col.tag == "Player")
            {
                GameUI.SetHint(time, text.Replace("\\n", "\n"));
                shown = true;
            }
        }
    }
}
