using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class UI_AllLevel : MonoBehaviour
{
    public Button[] LevelButtons;
    public String[] LevelNames;
    public String[] LevelScenes;
    public Text changeText;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void OnHovering(Button button)
    {
        for(int i=0;i<LevelButtons.Length;++i)
        {
            if(LevelButtons[i]==button)
            {
                changeText.text = LevelNames[i];
                return;
            }
        }
    }
    public void OnExit(Button button)
    {
        changeText.text = "";
    }
    public void OnClick(Button button)
    {
        for (int i = 0; i < LevelButtons.Length; ++i)
        {
            if (LevelButtons[i] == button)
            {
                DontDestroyOnLoad(gameObject);
                Application.LoadLevel(LevelScenes[i]);
                anim.SetTrigger("Shade");
                return;
            }
        }
    }
    void OnLevelWasLoaded(int level)
    {
        anim.SetTrigger("DoneLoadingLevel");
    }
}
