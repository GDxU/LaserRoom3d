using UnityEngine;
using System.Collections;

public class Misc_GravityDirSign : MonoBehaviour {
    public MeshRenderer m1, m2;
	// Use this for initialization
    public void Enable()
    {
        m1.enabled = true;
        m2.enabled = true;
    }
    public void Disable()
    {
        m1.enabled = false;
        m2.enabled = false;
    }
	
}
