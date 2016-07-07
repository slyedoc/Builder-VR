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

    #region IPlay
    Vector3 play_position;
    Quaternion play_rotation;
    Vector3 play_scale;

    public void Play()
    {
        play_position = transform.position;
        play_rotation = transform.rotation;
        play_scale = transform.localScale;
        rb.isKinematic = false;
    }

    public void Stop()
    {
        rb.isKinematic = true;
        transform.position = play_position;
        transform.rotation = play_rotation;
        transform.localScale = play_scale;
    }
    #endregion


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
