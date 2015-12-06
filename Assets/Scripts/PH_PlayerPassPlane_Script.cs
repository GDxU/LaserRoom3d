using UnityEngine;
using System.Collections;

public class PH_PlayerPassPlane_Script : MonoBehaviour {

    void OnTriggerStay(Collider col)
    {
        if (col.tag!="Player") return;
        Character_PickAndDrag pickAndDrag = col.GetComponent<Character_PickAndDrag>();
        pickAndDrag.ForceReleaseGrabbed(false);
    }
}
