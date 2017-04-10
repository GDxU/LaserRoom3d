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
    public Animator shaderAnim;
    Animator anim;
    bool loadingLevel;
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
                loadingLevel = true;
                Application.LoadLevel(LevelScenes[i]);
                anim.SetTrigger("LoadingLevel");
                shaderAnim.SetTrigger("ShadeEnable");
                return;
            }
        }
    }
    void OnLevelWasLoaded(int level)
    {
        if (loadingLevel==true)
        {
            shaderAnim.SetTrigger("ShadeDisable");
            loadingLevel = false;
        }
    }
}
