using UnityEngine;
using System.Collections;

public class Game_Menu_Main : MonoBehaviour {
    /*
	//Main main;
	GameObject menuInstructions;
	int buttonWidth=120,buttonHeight=30;
	void Start(){
		//GameObject level = GameObject.Find ("Level");
		//main = level.GetComponent<Main> ();
		GameObject debugModeObject = GameObject.Find ("DebugModeObject(Clone)");
		if(debugModeObject!=null)
		{
			//关卡测试启动模式
			//你既然都已经从关卡场景启动了，那我还有什么存在的意义
			Destroy(gameObject);
		}
	}
	void OnGUI(){

		GUI.Label(new Rect ((Screen.width-buttonWidth)/2, 10, buttonWidth, buttonHeight),"Select Level");
		int tCount = 0;

		string[] res={"Level0","Level1","Level2","Level3(Updated)"};
		//在编辑器的设置里寻找场景名字
		//这种破办法只能临时用用
		//foreach (UnityEditor.EditorBuildSettingsScene i in UnityEditor.EditorBuildSettings.scenes)
		for(int i=0;i<=3;++i)
		{
			//if (i.enabled)
			{
				string name = res[i];//i.path.Substring(i.path.LastIndexOf('/')+1);
				//name = name.Substring(0,name.Length-6);
				//如果名字是Base，那它不是一个关卡，而是主控场景
				if(name=="Base")continue;
				if(GUI.Button (new Rect ((Screen.width-buttonWidth)/2, 40+(buttonHeight+10)*tCount, buttonWidth, buttonHeight), name))
				{
					//加载所选关卡
					main.SendMessage("LoadLevelByName",name);
					//销毁主菜单对象
					Destroy(gameObject);
				}
			}
			tCount+=1;
		}
		GUI.Label(new Rect((Screen.width - buttonWidth) / 2-60,45 + (buttonHeight + 10) * tCount,60,buttonHeight),"Click ->");
		if(GUI.Button (new Rect ((Screen.width - buttonWidth) / 2, 40 + (buttonHeight + 10) * tCount, buttonWidth, buttonHeight), "Instructions"))
		{
			menuInstructions=(GameObject)Instantiate(main.menu_instructions);
			menuInstructions.transform.parent=transform.parent;
			GameObject.Destroy(gameObject);
		}

	}
    */
}
