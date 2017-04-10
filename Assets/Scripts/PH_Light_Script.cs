using UnityEngine;
using System.Collections;

public class PH_Light_Script : MonoBehaviour {

	//Main main;
	LineRenderer theLine;
	int vertexCount;
	Vector3 lineDir,linePos;
	Ray theRay;
	RaycastHit theOutRay;
	bool isActivated=false;
	float tAngle;
	//public Transform emitter;

	// Use this for initialization
	void Start () {
		theLine = this.gameObject.GetComponent<LineRenderer>();
	}
	void CreatePos(Vector3 pos)
	{
		vertexCount += 1;
		theLine.SetVertexCount (vertexCount);
		theLine.SetPosition (vertexCount-1, pos);
	}
	public void Deactivate()//关闭射线
	{
		isActivated = false;
	}
	public void Activate()//开启射线
	{
		isActivated = true;
	}
	// Update is called once per frame
	void Update () {
		Transform colliderTrans;
		vertexCount=0;
		if (!isActivated)
						return;
		linePos = theLine.transform.position;
		lineDir = theLine.transform.forward;
		//******直接获取方向和位置

		CreatePos (linePos);
		//while(true)
		//防死循环
		for(int i=1;i<=1000;++i)
		{
			theRay.origin = linePos;
			theRay.direction = lineDir;
			Vector3 normalDir;
            //检测到无穷远处
            bool lightReflected = true;
            if (Physics.Raycast(theRay, out theOutRay, Mathf.Infinity, ~((3 << 8) | (1 << 2) | (1 << 26) | (1 << 22))))//Ignore Light Stuff
			{
				linePos=theOutRay.point;
				normalDir=theOutRay.normal;
				switch(theOutRay.collider.gameObject.layer)
                {
                case 10://"Mirror":
                case 27://"Wall_Reflective":
					CreatePos (linePos-lineDir*0.001F);
					CreatePos (linePos);
					lineDir=Vector3.Reflect(lineDir,normalDir);
					CreatePos (linePos+lineDir*0.001F);
					linePos+=lineDir*0.001F;
					//为了光线的渲染效果更好，每个转折点处新建3个关键点
					break;
                case 11://"ParaboloidMirror":
					CreatePos (linePos-lineDir*0.001F);
					CreatePos (linePos);
					colliderTrans=theOutRay.collider.transform;
					//凹面镜的焦点必须是它的transform.position
					normalDir=(colliderTrans.right.normalized+(colliderTrans.position-theOutRay.point).normalized).normalized;
					//通过简单的解析几何计算出凹面镜真正法向量的方向，再进行反射
					lineDir=Vector3.Reflect(lineDir,normalDir);
					CreatePos (linePos+lineDir*0.001F);
					linePos+=lineDir*0.001F;
                    break;
                case 12://"BoxedMirror":
                    CreatePos(linePos - lineDir * 0.001F);
                    CreatePos(linePos);
                    //镜箱的表面并非都是反射性表面，只有镜面才能反射。
                    //注：如果直接将镜箱拆成两个非Convex的Mesh Collider为镜面和非镜面部分时，Unity5.0会不允许，因此必须写这个算法判定：
                    colliderTrans=theOutRay.collider.transform;
                    //InverseTransformVector：全局坐标转局部坐标
                    Vector3 delta = colliderTrans.InverseTransformVector(colliderTrans.position - linePos);
                    //Game.debugText.text = (delta.ToString());
                    //箱子非反射性部分到中心构成一个边长为4.8的正方体表面
                    //以此来检测此表面是否是反射性的
                    if (Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z)) < 2.38)
                    {
                        lineDir = Vector3.Reflect(lineDir, normalDir);
                        CreatePos(linePos + lineDir * 0.001F);
                        linePos += lineDir * 0.001F;
                    }
                    else lightReflected = false;
                    break;
                case 13://"GratingPlane":
					//main.debugText.text=Vector3.Angle(theOutRay.collider.transform.right,lineDir).ToString();
					tAngle=Vector3.Angle(theOutRay.collider.transform.right,lineDir);
					//角度为[89°,91°]的予以放行
                    if(tAngle>=89F && tAngle<=91F)
					{
						linePos+=lineDir*0.001F;
					}
					else
					{
						CreatePos (linePos-lineDir*0.001F);
						CreatePos (theOutRay.point);
                        lightReflected = false;
					}
					break;
				case 14://"Receiver(Trans)":
					theOutRay.collider.SendMessage("Activate");
					linePos+=lineDir*0.001F;
					break;
				case 15://"Receiver(Nontrans)":
					theOutRay.collider.SendMessage("Activate");
					CreatePos (linePos-lineDir*0.001F);
					CreatePos (theOutRay.point);
                    lightReflected = false;
                    break;
				default:
					CreatePos (linePos-lineDir*0.001F);
					CreatePos (theOutRay.point);
					//Game.debugText.text=theOutRay.collider.contactOffset.ToString();
                    lightReflected = false;
                    break;
				}
                if (lightReflected == false) break;

			}
			else
			{
				CreatePos(linePos+lineDir*1000000);
				break;
			}
		}
	}
}
