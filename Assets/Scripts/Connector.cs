using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRTK;

public class Connector : MonoBehaviour
{
    private ConnectionManager connectionManager;

    public void Start()
    {
        connectionManager = GetComponentInParent<ConnectionManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {

        //ignore if not enabled
        if (!connectionManager.EnableSnap)
            return;


        var connector = collider.GetComponent<Connector>();
        if (connector)
        {
            connectionManager.HitConnector(this, connector);
        }

        var cj = collider.GetComponent<ConnectionJoint>();
        if (cj)
        {
            connectionManager.HitJoint(this, cj);
        } 
    }

    //public void OnTriggerExit(Collider other)
    //{
    //    if (!connectionManager.EnableSnap)
    //        return;

    //    if (other == lastSnapCollider)
    //    {
    //        lastSnapCollider = null;
    //    }
    //}

    //public void SnapToLastCollider()
    //{
    //    if (lastSnapCollider != null &&  IsConnected == false)
    //    {
    //        Collider collider = lastSnapCollider;
    //        var collidedItem = collider.GetComponent<Connector>();
    //        if (collidedItem)
    //        {
    //            connectionManager.HitConnector(this, collidedItem);
    //            IsConnected = true;
    //        }
    //        var cj = collider.GetComponent<ConnectionJoint>();
    //        if (cj)
    //        {
    //            connectionManager.HitJoint(this, cj);
    //            IsConnected = true;
    //        }
    //    }
    //}

    //private void OnTriggerStay(Collider collider)
    //{

    //    //ignore if not enabled
    //    if (!connectionManager.EnableSnap)
    //        return;

    //    //ignore if already connected
    //    if (IsConnected == true)
    //        return;

    //    var isConnected = false;
    //    var rod = this.GetComponentInParent<VRTK_InteractableObject>();
    //    if (rod != null && rod.IsGrabbed() == false) //don't snap if still grabbed.
    //    {
    //        var collidedItem = collider.GetComponent<Connector>();
    //        if (collidedItem)
    //        {
    //            connectionManager.HitConnector(this, collidedItem);
             
    //        }
    //        var cj = collider.GetComponent<ConnectionJoint>();
    //        if (cj)
    //        {
    //            connectionManager.HitJoint(this, cj);
    //            isConnected = true;
    //        }
    //        IsConnected = isConnected;
    //    }

    //}



}
