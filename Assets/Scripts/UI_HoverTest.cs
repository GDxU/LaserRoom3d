using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HoverTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler{

    public UI_AllLevel alllevels;
	// Use this for initialization
	void Start () {
	
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        alllevels.SendMessage("OnHovering", GetComponent<Button>());
        //Debug.Log("Hovering");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        alllevels.SendMessage("OnExit", GetComponent<Button>());
        //Debug.Log("Exit");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        alllevels.SendMessage("OnClick", GetComponent<Button>());
        //Debug.Log("Click");
    }
	// Update is called once per frame
	void Update () {
	
	}
}
