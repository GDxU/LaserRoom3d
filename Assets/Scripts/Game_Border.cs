using UnityEngine;
using System.Collections;

public class Game_Border : MonoBehaviour {

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            GameUI.canvas.SendMessage("OnLevelRestart", -1);
        }
    }
}
