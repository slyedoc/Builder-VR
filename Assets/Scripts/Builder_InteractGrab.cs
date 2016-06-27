using UnityEngine;
using System.Collections;
using VRTK;



public class Builder_InteractGrab : VRTK_InteractGrab
{
    public GameObject rodPrefab;
    
    new void Start()
    {
            base.Start();
            //if (GetComponent<VRTK_InteractTouch>() == null || GetComponent<VRTK_InteractGrab>() == null)
            //{
            //    Debug.LogError("BuilderController is required to be attached to a SteamVR Controller that has the VRTK_InteractTouch and VRTK_InteractGrab script attached to it");
            //    return;
            //}

            //GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);
            //GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

            //GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
            //GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);

        //GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
    }

    

    void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {


        //SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
        
        if (this.GetGrabbedObject() == null)
        {
            
            Debug.LogError("touch pressed");

            var rodClone = Instantiate(rodPrefab);
            rodClone.SetActive(true);
            rodClone.name = "rod clone";
            rodClone.transform.position = this.transform.position; // new Vector3(-2.36f, 1.2f, 0.635f);


            this.GrabTrackedObject();

            //var grable = this.IsObjectGrabbable(rodClone);


            var collider = rodClone.GetComponent<Collider>();
            if (collider != null)
            {
                this.GrabTrackedObjectCollider(collider);
            }


            //this.OnTriggerStay(collider);
            //this.AttemptGrab();
        }

        //rodClone.Grabbed(controller);
        //Rigidbody rb = rodClone.GetComponent<Rigidbody>();
        //rb.AddForce(-Vector3.forward * 2f);

        //SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
        //rodClone.transform.parent = rodClone.transform;

    }


    private bool CanGrab(VRTK_InteractGrab grabbingController)
    {
        return (grabbingController && grabbingController.GetGrabbedObject() == null /*&& grabbingController.gameObject.GetComponent<VRTK_ControllerEvents>().grabPressed*/);
    }

    //void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    //{
    //    rcCarScript.SetTouchAxis(e.touchpadAxis);
    //}

    //void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    //{
    //    rcCarScript.SetTriggerAxis(e.buttonPressure);
    //}

    //void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    //{
    //    rcCarScript.SetTouchAxis(Vector2.zero);
    //}

    //void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    //{
    //    rcCarScript.SetTriggerAxis(0f);
    //}

    //void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
    //{
    //    rcCarScript.Reset();
    //}
}
