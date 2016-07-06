using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class ConnectionJoint : Builder_InteractableObject
{
    private List<Joint> joints = new List<Joint>();

    public bool DrawJoints = true;

    private LineRenderer line;

    protected override void Awake()
    {
        base.Awake();
        line = GetComponent<LineRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        line.SetWidth(.05f, .05f);
    }

    public void Snap(Connector targetConnection)
    {
        //stop physics from going nuts
        var collider = targetConnection.transform.parent.GetComponent<Collider>();
        Physics.IgnoreCollision(GetComponent<Collider>(), collider);
        foreach (var j in joints)
        {
            Physics.IgnoreCollision(j.connectedBody.GetComponent<Collider>(), collider);
        }


        var target = targetConnection.transform.parent.gameObject;
        var targetRigidBoby = target.GetComponent<Rigidbody>();


        //setup joint
        var joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedBody = targetRigidBoby;
        joint.connectedAnchor = targetConnection.transform.localPosition;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.breakTorque = float.PositiveInfinity;
        joint.breakForce = float.PositiveInfinity;

        joints.Add(joint);
    }

    public void UpdateJoint(Rigidbody target, Vector3 connectedAnchor)
    {
        var j = joints.Where(a => a.connectedBody == target).FirstOrDefault();
        if( j != null) { 
           j.connectedAnchor = connectedAnchor;      
        }        
    }

    protected override void Update()
    {
        base.Update();
        if (DrawJoints)
        {
            line.SetVertexCount(joints.Count * 2);

            int i = 0;
            foreach (var j in joints)
            {
                line.SetPosition(i, transform.position);
                line.SetPosition(i + 1, j.connectedBody.transform.TransformPoint(j.connectedAnchor));
                i += 2;
            }
        }

    }

}
