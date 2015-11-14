using UnityEngine;
using System.Collections;

public static class Game
{
	//public GameObject endObject;
    public static GameObject linePrefab;
	public static GameObject gratingPlanePrefab;
	public static Transform player;
    public static GUIText debugText;
	public static GUIText crosshair;
	//public AudioClip soundDoorMoving,soundDoorOpen,soundDoorClose;
    
	//public GameObject menu_main,menu_instructions,menu_ingame;
	//public float startTime,endTime;
	//public GameObject player;
	//public Camera playerCamera;
	// Use this for initialization
	// 全局定义在这里实现
	//void Start () {
		//startTime = Time.time;
	//}
    public static bool CursorLocker
    {
        set
        {
            if (value)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }

        }
    }
    public static void InitTestGame(Game_Init init)
    {
        linePrefab = init.linePrefab;
        gratingPlanePrefab = init.gratingPlanePrefab;
        crosshair = init.crosshair;
        debugText = init.debugText;
        Game.CursorLocker = true;
    }
}
