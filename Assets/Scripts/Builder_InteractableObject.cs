using UnityEngine;
using System.Collections;
using UnityEditor;
using VRTK;

[RequireComponent(typeof(ConnectionManager), typeof(MeshRenderer), typeof(Rigidbody))]
public class Builder_InteractableObject : VRTK_InteractableObject, IPlay
{
    public bool IsPlaying { get; set; }

    public ConnectionManager cm;

    protected override void Awake()
    {
        base.Awake();
        cm = GetComponent<ConnectionManager>();
    }

    public void Play()
    {
        rb.isKinematic = false;
        IsPlaying = true;
    }

    public override void Grabbed(GameObject grabbingObject)
    {

        base.Grabbed(grabbingObject);
        GetComponent<Collider>().enabled = false;
        cm.EnableSnap = true;
    }

    public override void Ungrabbed(GameObject grabbingObject)
    {
        base.Ungrabbed(grabbingObject);
        GetComponent<Collider>().enabled = true;
        cm.EnableSnap = false;
    }
}
