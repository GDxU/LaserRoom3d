using UnityEngine;
using System.Collections;

public class Qipa : MonoBehaviour {
	
	public Rigidbody BallMF;
	public Transform Camera;
	public Transform BallPos;
	public GUIText TextMF;
	private float deltaRotationY;
	private float sensitivityX = 15f;
	private float minZoom = 8;
	private float maxZoom = 60;
	private float wheelsensitivity = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if(scroll != 0){
			scroll = -scroll;
			Vector3 displace = Camera.localPosition;
			Vector3 direction = displace.normalized;
			if (displace.magnitude < maxZoom && displace.magnitude > minZoom || displace.magnitude >= maxZoom && scroll < 0 || displace.magnitude <= minZoom && scroll > 0)
			{
				Camera.localPosition = displace + wheelsensitivity * scroll * direction;
			}
		}
		if (Input.GetMouseButton(1)){
			deltaRotationY = Input.GetAxis("Mouse X") * sensitivityX;
			Quaternion quaternionY = Quaternion.AngleAxis(deltaRotationY, Vector3.up);
			BallPos.rotation = BallPos.rotation * quaternionY;
			TextMF.text = "true" + deltaRotationY + BallPos.rotation.y;
		}
		Vector3 inputVector = new Vector3(Input.GetAxis ("Horizontal"), Input.GetKey(KeyCode.Space)?5f:0f, Input.GetAxis ("Vertical"));
		BallMF.AddForce(100*(BallPos.rotation * inputVector));
		BallPos.position = BallMF.position;
	}
}

/*using UnityEngine;
using System.Collections;

public class Qipa : MonoBehaviour {

	Main main;
	public Rigidbody BallMF;
	public Transform BallPos;
	public GUIText TextMF;
	// Use this for initialization
	void Start () {
		GameObject level = GameObject.Find ("Level");
		main = level.GetComponent<Main> ();
		this.gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		BallMF.AddForce(100*new Vector3(Input.GetAxis ("Horizontal"), Input.GetKey(KeyCode.Space)?5:0, Input.GetAxis ("Vertical")));
		BallPos.position = BallMF.position;
		TextMF.text = BallMF.position.ToString("f10");
	}
}*/
