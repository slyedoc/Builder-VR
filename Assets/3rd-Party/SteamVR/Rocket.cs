using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour, IPlay {

    public float Thrust;
    public float BurnTime;
    private float EndTime;

    public bool IsPlaying { get; set; }
    public Transform nausel;

    Rigidbody rigidBody;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}

    public void Play()
    {
        IsPlaying = true;
        EndTime = Time.time + BurnTime;
    }

    void FixedUpdate()
    {
        if (IsPlaying && EndTime > Time.time)
        {
            rigidBody.AddForceAtPosition(transform.up * (Thrust/ BurnTime), nausel.localPosition);
        }
    }
}
