using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRTK;
using Vectrosity;

public class GooNeighboor
{
    public GooJoint GooJoint { get; set; }
    public VectorLine line { get; set; }
}

public static class GooNumber
{
    public static int  i = 0;
    public static string GetId()
    {
        i++;
        return i.ToString();
    }
}

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class GooJoint : VRTK_InteractableObject, IPlay
{
    [Header("Goo Joint Settings", order = 5)]
    public bool IsPlaying = false;
    public bool Frozen = false;

    private List<GooNeighboor> neighboors = new List<GooNeighboor>();
    
    [Range(1f, 1000f)]
    public float lineWidth = 100f;
    public Color32 lineColor;
    public Color32 lineColorError;

    [Range(0f, 1f)]
    public float DistanceMin = .5f;
    [Range(0f, 5f)]
    public float DistanceMax = 1f;

    private bool UsingPositionValid = false;
    private GameObject UsingPositionSnap;
    public Vector3 UsingPosition;
    private VectorLine UsingLine;

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

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if (Frozen)
            rb.constraints = RigidbodyConstraints.FreezePosition;
        else
            rb.constraints = RigidbodyConstraints.None;
    }


    public override void Grabbed(GameObject grabbingObject)
    {

        base.Grabbed(grabbingObject);
        GetComponent<Collider>().enabled = false;
    }

    public override void Ungrabbed(GameObject grabbingObject)
    {
        base.Ungrabbed(grabbingObject);
        GetComponent<Collider>().enabled = true;
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);

        //setup events for goo touched so we can snap to existing goo joints
        var gooTouched = currentUsingObject.GetComponent<GooTouched>();
        gooTouched.Touched += SetSnap;
        gooTouched.Untouched += ClearSnap;

        //setup line
        var list = new List<Vector3>();
        list.Add(transform.position);
        list.Add(currentUsingObject.transform.position);

        UsingLine = new VectorLine("using line", list, 1, LineType.Continuous);
        UsingLine.SetWidth(lineWidth);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);

        //clear events
        var gooTouched = previousUsingObject.GetComponent<GooTouched>();
        gooTouched.Touched -= SetSnap;
        gooTouched.Untouched -= ClearSnap;

        if (UsingPositionValid)
        {
            GooJoint gooJoint;
            //if not snapped ot anything, create new joint and add as neighboor
            if (UsingPositionSnap == null)
            {
                gooJoint = CreateGooJoint(UsingPosition);
                AddNeighboor(gooJoint);
            }
            else
            {
                //if snapped, add if not already an neighboor                
                gooJoint = UsingPositionSnap.GetComponent<GooJoint>();
                if (!neighboors.Any(a => a.GooJoint == gooJoint))
                {
                    AddNeighboor(gooJoint);
                }
            }
        }

        VectorLine.Destroy(ref UsingLine);
    }

    public void SetSnap(GameObject obj)
    {

        if (obj != gameObject)
        {
            UsingPositionSnap = obj;
        }
    }

    public void ClearSnap(GameObject obj)
    {
        UsingPositionSnap = null;
    }


    private GooJoint CreateGooJoint(Vector3 pos)
    {
        //create connection joint
        var path = "GooJoint";
        var prefab = Resources.Load(path);
        if (prefab == null)
        {
            Debug.LogError("Couldn't load prefab: Assets/Resources/" + path);
            return null;
        }
        var obj = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        obj.name += GooNumber.GetId();
        obj.transform.parent = transform.parent;
        return obj.GetComponent<GooJoint>();        
    }


    public void AddNeighboor(GooJoint goo)
    {
        //setup joint
        var joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.anchor = Vector3.zero;
        joint.connectedBody = goo.GetComponent<Rigidbody>();
        joint.connectedAnchor = Vector3.zero;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;


        joint.breakTorque = float.PositiveInfinity;
        joint.breakForce = float.PositiveInfinity;
        joint.linearLimitSpring = new SoftJointLimitSpring()
        {
            spring = 50,
            damper = 10
        };

        goo.neighboors.Add(new GooNeighboor() { GooJoint = this });
        this.neighboors.Add(new GooNeighboor() { GooJoint = goo });
    }


    protected override void Update()
    {
        base.Update();

        if (IsUsing())
        {
            //see if the target is in range
            var distance = Vector3.Distance(transform.position, usingObject.transform.position);          
            if (distance < DistanceMin)
            {
                UsingPositionValid = false;
                UsingPosition = usingObject.transform.position;
            }
            else if (distance < DistanceMax)
            {                
                UsingPositionValid = true;
                //see if we need to snap to an existing goo joint
                if (UsingPositionSnap != null) {
                    UsingPosition = UsingPositionSnap.transform.position;
                }
                else { 
                    UsingPosition = usingObject.transform.position;
                }
            }
            else
            {
                //see position to the nearest possable if out of range
                UsingPositionValid = true;
                var direction = (usingObject.transform.position - transform.position).normalized;
                UsingPosition = transform.position + (direction * DistanceMax);
            }
        }
        
        DrawLines();

    }

    private void DrawLines()
    {
        foreach (var n in neighboors)
        {
            var distance = Vector3.Distance(transform.position, n.GooJoint.transform.position);
            var direction = (n.GooJoint.transform.position - transform.position).normalized;

            if (n.line == null)
            {
                //create the line if doesnt exist
                var list = new List<Vector3>();
                list.Add(transform.position);
                list.Add(transform.position + (direction * (distance / 2)));

                n.line = new VectorLine("line", list, 1, LineType.Continuous);
                n.line.SetWidth(lineWidth);
                n.line.SetColor(lineColor);
            }
            else
            {
                //update the line, remember to only draw half since each goo joint will draw half of each connection
                n.line.points3[0] = transform.position;
                n.line.points3[1] = transform.position + (direction * (distance / 2));
            }
            n.line.Draw3D();
        }

        //Draw line to the using position
        if (IsUsing())
        {

            if (UsingPositionValid) {
                UsingLine.SetColor(lineColor);
            }
            else { 
                UsingLine.SetColor(lineColorError);
            }

            //update line
            UsingLine.points3[0] = transform.position;
            UsingLine.points3[1] = UsingPosition;
            UsingLine.Draw3D();
        }        
    }
}
