using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(ConnectionManager), typeof(MeshRenderer), typeof(Rigidbody))]
public class Wheel : VRTK_InteractableObject, IHasConnection, IPlay
{
    [Header("Wheel Settings", order = 0)]

    public float Speed = 100f;


    public GameObject Point1;
    public GameObject Point1Old;
    public GameObject Point2;
    public GameObject Point2Old;

    private Rigidbody rigidBody;
    private ConnectionManager connectionManager;

    public bool IsPlaying { get; set;  }

    public void Play()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.useGravity = true;
        
        IsPlaying = true;
    }
   
    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody>();
        connectionManager = GetComponent<ConnectionManager>();
    }

    void IHasConnection.ReplaceConnection(Connector child, ConnectionJoint target)
    {
        ReplaceConnector( Point1, ref Point1Old, child, target);
        ReplaceConnector(Point2, ref Point2Old, child, target);
    }

    private void ReplaceConnector(GameObject point, ref GameObject oldPoint, Connector child, ConnectionJoint target)
    {
        if (point == child.gameObject)
        {
            oldPoint = point;
            Point1 = target.gameObject;
        }

        //HingeJoint hinge = GetComponent<HingeJoint>();
        //JointMotor motor = hinge.motor;
        //motor.force = 100;
        //motor.targetVelocity = 90;
        //motor.freeSpin = false;
        //hinge.motor = motor;



    }

    public override void Grabbed(GameObject grabbingObject)
    {

        base.Grabbed(grabbingObject);
        rigidBody.constraints = RigidbodyConstraints.None;
        GetComponent<Collider>().enabled = false;
        connectionManager.EnableSnap = true;

    }

    public override void Ungrabbed(GameObject grabbingObject)
    {
        base.Ungrabbed(grabbingObject);
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().enabled = true;
        connectionManager.EnableSnap = false;
    }




    protected override void Update()
    {
        base.Update();
        if (IsPlaying)
        {
            rigidBody.AddTorque(Vector3.forward * Speed, ForceMode.Force);
        } 
    }


}
