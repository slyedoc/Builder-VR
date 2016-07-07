using UnityEngine;
using System.Collections;
using VRTK;

public delegate void GooTouchedEventHandler(GameObject sender);

public class GooTouched : MonoBehaviour
{

    private GameObject lastGooTouchedObject;
    public event GooTouchedEventHandler Touched;
    public event GooTouchedEventHandler Untouched;

    private void OnTriggerEnter(Collider other)
    {
        if (IsObjectInteractable(other.gameObject))
        {
            lastGooTouchedObject = other.gameObject;
            if (Touched != null)
                Touched(other.gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (lastGooTouchedObject != null && (lastGooTouchedObject == other.gameObject))
        {
            lastGooTouchedObject = null;
            if (Untouched != null)
                Untouched(null);
        }
    }

    public bool IsObjectInteractable(GameObject obj)
    {
        return (obj && (obj.GetComponent<GooJoint>()));
    }

}
