using UnityEngine;
using UnityEngine.UI;

public class UI_Gameplay : MonoBehaviour {
    public Text youfinishText;
    Animator anim;
    public Animator shaderAnim;
    bool levelEnding = false;
    bool levelRestarting = false;
    bool gameExiting = false;
	void Start () {
        anim = GetComponent<Animator>();
        GameUI.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameUI.hintText = GameObject.Find("HintText").GetComponent<Text>();
        GameUI.sayingText = GameObject.Find("SayingText").GetComponent<Text>();
        GameUI.crosshair = GameObject.Find("+").GetComponent<Text>();
        GameUI.youfinishText = youfinishText;
        if(GameUI.ingameUI)
        {
            anim.SetTrigger("IngameStart");
            GameUI.ingameUI = false;
            GameUI.crosshair.enabled = true;
        }
	}
	public void OnLevelEnd(int id)
    {
        if (levelRestarting || levelEnding) return;
        levelEnding = true;
        shaderAnim.SetTrigger("ShadeEnable");
    }
    public void OnLevelRestart(int id)
    {
        if (levelRestarting || levelEnding) return;
        levelRestarting = true;
        shaderAnim.SetTrigger("ShadeEnable");

    }
    public void ExitGame()
    {
        gameExiting = true;
        shaderAnim.SetTrigger("ShadeEnable");
    }
    public void FadeFinish()
    {
        if(gameExiting)
        {
            Application.Quit();
            return;
        }
        if (levelEnding)
        {
            DontDestroyOnLoad(gameObject);
            Application.LoadLevel("Empty");
          
            StartCoroutine(Common.WaitAndAction(LevelEndToLevelSelect, 3f));
            if(GameUI.youfinishText.text!="")
            StartCoroutine(Common.WaitAndAction(ClearText, 4f)); 
        }
        else if(levelRestarting)
        {
            DontDestroyOnLoad(gameObject);
            Application.LoadLevel(Application.loadedLevel);
            shaderAnim.SetTrigger("ShadeDisable");
            levelRestarting = false;
        }
    }
    public void LevelEndToLevelSelect()
    {
        shaderAnim.SetTrigger("ShadeDisable");
        anim.SetTrigger("LevelEnd");
        levelEnding = false;
        Game.CursorLocker = false;
    }
    public void ClearText()
    {
        GameUI.youfinishText.text = "";
    }
}
