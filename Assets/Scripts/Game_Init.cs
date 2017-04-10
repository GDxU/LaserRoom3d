using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_Init : MonoBehaviour {
    public GameObject linePrefab;
    public GameObject gratingPlanePrefab;
    //public GUIText crosshair;
    public GUIText debugText;
    public LineRenderer debugLine;
    public GameObject ingameCanvas;

	// Use this for initialization
	void Awake () {
        Gameplay.startTime = Time.time;
        Time.timeScale = 2F;
        Game.InitTestGame(this);
        if (GameObject.Find("Canvas") == null)//Inlevel start
        {
            Application.LoadLevelAdditive("Main");
            GameUI.ingameUI = true;
        }
        else GameUI.crosshair.enabled = true;
	}
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            GameUI.canvas.SendMessage("OnLevelEnd",-1);
        }
        if(Input.GetKey(KeyCode.R))
        {
            GameUI.canvas.SendMessage("OnLevelRestart", -1);
        }
    }
}
