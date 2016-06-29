using UnityEngine;
using System.Collections;
using VRTK;



public class Builder_InteractGrab : VRTK_InteractGrab
{
    [Header("Builder Interactions", order = 5)]
    public GameObject rodPrefab;
    
    new void Start()
    {
            base.Start();
            //if (GetComponent<VRTK_InteractTouch>() == null )
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


        if (this.GetGrabbedObject() == null)
        {
            
            Debug.LogError("touch pressed");
           var placedRoot =  GameObject.FindGameObjectsWithTag("PlacedRoot");
            var rodClone = Instantiate(rodPrefab);
            rodClone.SetActive(true);
            rodClone.name = "rod clone";
            rodClone.transform.position = this.transform.position; 
            rodClone.transform.rotation = this.transform.rotation; //take rotation from controller
            rodClone.transform.Rotate(90, 0, 0);   //rotate so its inline with controller. 
            rodClone.transform.parent = placedRoot[0].transform;
            var interObj = rodClone.GetComponent<VRTK_InteractableObject>();


            var collider = rodClone.GetComponent<Collider>();
            //if (collider != null)
            //{
            //    this.GrabTrackedObjectCollider(collider);
            //}

            var touch = this.GetComponent<VRTK_InteractTouch>();
            if (touch != null && collider != null)
            {
                touch.OnTriggerStay(collider);
            }


            this.GrabTrackedObject();

            //if (interObj != null)
            //{
            //    interObj.Grabbed(this.gameObject);
            //}

            //var grable = this.IsObjectGrabbable(rodClone);


            


        }

    }

}
