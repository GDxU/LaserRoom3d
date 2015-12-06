using UnityEngine;
using System.Collections;

public class UI_Gameplay : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	public void OnLevelEnd(int id)
    {
        DontDestroyOnLoad(gameObject);
        Application.LoadLevel("Empty");
        anim.SetTrigger("LevelEnd");
    }
	// Update is called once per frame
	void Update () {
	
	}
}
