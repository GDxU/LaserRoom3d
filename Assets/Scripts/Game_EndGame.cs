using UnityEngine;
using System.Collections;

public class Game_EndGame : MonoBehaviour {
	
	void OnTriggerEnter(Collider col){
		if(col.tag=="Player")
		{
            //Game.debugText.text="Congratulation!You've Finished.\nTime:"+(Time.time-Gameplay.startTime).ToString()+ " Sec";
            int finishTime = (int)(Time.time - Gameplay.startTime);
            GameUI.youfinishText.text = "YOU FINISHED THE LEVEL IN " + (finishTime / 60).ToString() + " MIN " + (finishTime % 60).ToString() + " SEC .";
            Game.CursorLocker = false;
            GameUI.canvas.SendMessage("OnLevelEnd", -1);
            GameUI.crosshair.enabled = false;
            
        }

	}
}
