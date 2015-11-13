using UnityEngine;
using System.Collections;

public class Game_Init : MonoBehaviour {
	//Main main;
    //GameObject level;
	//Game_LevelInfo levelinfo;
	//public GameObject debugModeObject;
    public GameObject linePrefab;
    public GameObject gratingPlanePrefab;
    public GUIText crosshair;
    public GUIText debugText;

	// Use this for initialization
	void Awake () {
        Time.timeScale = 2F;
        Game.InitTestGame(this);
	}
    /*
	//用名字加载关卡，这种破办法只能临时用用
	void LoadLevelByName(string levelName)
	{
		main.player = (GameObject)Instantiate (main.playerPrefab);
		main.playerCamera = main.player.GetComponentInChildren<Camera> ();
		main.player.transform.parent = level.transform;
		Application.LoadLevel (levelName);
	}
	void OnLevelWasLoaded(int sceneID){
		levelinfo = GameObject.Find ("LevelInfo").GetComponent<Game_LevelInfo> ();
		main.player.transform.position = levelinfo.levelStart.transform.position;
		main.player.transform.rotation = levelinfo.levelStart.transform.rotation;
		Instantiate (main.endObject, levelinfo.levelEnd.transform.position, levelinfo.levelEnd.transform.rotation);
		main.player.GetComponent<PickAndDrag> ().disableThrowing = levelinfo.disableThrowing;
	}
	// Update is called once per frame
	void Update () {
	
	}*/
}
