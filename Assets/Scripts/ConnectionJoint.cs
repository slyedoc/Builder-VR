using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class ConnectionJoint : MonoBehaviour, IPlay
{
    private List<Joint> joints = new List<Joint>();
    private List<Connector> connectors = new List<Connector>();

    private Rigidbody rb;

    public bool IsPlaying { get; set; }
    public void Start()
    {
        rb = GetComponent<Rigidbody>();      
          
    }


    public void Snap(Connector targetConnection)
    {
        //stop physics from going nuts
        Physics.IgnoreCollision(GetComponent<Collider>(), targetConnection.transform.parent.GetComponent<Collider>());

        var target = targetConnection.transform.parent.gameObject;
        var targetRigidBoby = target.GetComponent<Rigidbody>();


        //setup joint
        var joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = transform.localPosition;
        joint.connectedBody = targetRigidBoby;
        joint.connectedAnchor = targetConnection.transform.localPosition;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.breakTorque = float.PositiveInfinity;
        joint.breakForce = float.PositiveInfinity;

        joints.Add(joint);
        
        //add connection
        connectors.Add(targetConnection);

    }

    public void Play()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().useGravity = true;
    }

}
