using UnityEngine;
using System.Collections;

public class Game_EndGame : MonoBehaviour {
	
	void OnTriggerEnter(Collider col){
		if(col.tag=="Player")
		{
			Game.debugText.text="Congratulation!You've Finished.\nTime:"+(Time.time-Gameplay.startTime).ToString()+ " Sec";
            Game.CursorLocker = false;
            Game.canvas.SendMessage("OnLevelEnd",-1);
            
        }

	}
}
