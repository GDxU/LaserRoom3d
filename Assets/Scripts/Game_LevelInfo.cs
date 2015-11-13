using UnityEngine;
using System.Collections;
/*
public class Game_LevelInfo : MonoBehaviour {
	Main main;
	public int levelNum;
	public Transform levelStart;
	public Transform levelEnd;
	public bool disableThrowing;
	GameObject debugModeObject;
	// Use this for initialization
	void Start () {
		
		GameObject level = GameObject.Find ("Level");
		if(level==null)
		{
			//假如在关卡场景中播放，则level物体为空
			//则启动Debug模式
			debugModeObject=(GameObject)Instantiate(Resources.LoadAssetAtPath("Assets/Prefabs/DebugModeObject.prefab",typeof(GameObject)));
			debugModeObject.GetComponent<Game_DebugModeStart> ().debugLevelID = Application.loadedLevel;
			debugModeObject.SendMessage ("StartDebugMode");
		}
		else 
		{
			main = level.GetComponent<Main> ();
			levelStart.gameObject.SetActive (false);
			levelEnd.gameObject.SetActive (false);
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
*/