using UnityEngine;
using System.Collections;
using UnityEditor;
using VRTK;

public class Wheel : Builder_InteractableObject
{
    [Header("Wheel Settings", order = 5)]
    public float Speed = 100f;
    
    protected override void Update()
    {
        base.Update();

        if (IsPlaying)
        {
            rb.AddTorque(Vector3.forward * Speed, ForceMode.Force);
        } 
    }


}
