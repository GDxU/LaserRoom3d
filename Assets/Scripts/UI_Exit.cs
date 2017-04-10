using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ExitButton : MonoBehaviour, IPointerClickHandler
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        GameUI.canvas.SendMessage("ExitGame");
        //Debug.Log("Click");
    }
}
