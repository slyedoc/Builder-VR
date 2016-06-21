using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class Rod : VRTK_InteractableObject, IHasConnection, IPlay
{
    [Header("Rod Settings", order = 0)]

    
    public float length = 1;
    public float lengthMax = 10;
    public float lengthMin = 0.2f;

    public float MassPerUnit = 10;
    private float radius = .05f;
    private int nbSides = 32;
    private int nbHeightSeg = 1; // Not implemented yet
    private Mesh mesh = null;

    private float oldLength;


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
       
        BuildMesh();

        rigidBody = GetComponent<Rigidbody>();
        
        connectionManager = GetComponent<ConnectionManager>();
    }

    void IHasConnection.ReplaceConnection(Connector child, ConnectionJoint target)
    {
        if (Point1 == child.gameObject)
        {
            Point1Old = Point1;
            Point1 = target.gameObject;
        }

        if (Point2 == child.gameObject)
        {
            Point2Old = Point2;
            Point2 = target.gameObject;
        }

        UpdateRod();
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

    private void SetConnectors()
    {
        Point1.transform.localPosition = new Vector3(0, length / 2, 0);
        Point2.transform.localPosition = new Vector3(0, -length / 2, 0);
    }

    private void BuildMesh()
    {
        //build mesh
        mesh = new Mesh();
        int nbVerticesCap = nbSides + 1;

        #region Vertices

        // bottom + top + sides
        Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * nbHeightSeg * 2 + 2];
        int vert = 0;
        float _2pi = Mathf.PI * 2f;

        // Bottom cap
        vertices[vert++] = new Vector3(0f, -length / 2, 0f);
        while (vert <= nbSides)
        {
            float rad = (float)vert / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * radius, -length / 2, Mathf.Sin(rad) * radius);
            vert++;
        }

        // Top cap
        vertices[vert++] = new Vector3(0f, length / 2, 0f);
        while (vert <= nbSides * 2 + 1)
        {
            float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * radius, length / 2, Mathf.Sin(rad) * radius);
            vert++;
        }

        // Sides
        int v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * radius, length / 2, Mathf.Sin(rad) * radius);
            vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * radius, -length / 2, Mathf.Sin(rad) * radius);
            vert += 2;
            v++;
        }
        vertices[vert] = vertices[nbSides * 2 + 2];
        vertices[vert + 1] = vertices[nbSides * 2 + 3];
        #endregion

        #region Normales

        // bottom + top + sides
        Vector3[] normales = new Vector3[vertices.Length];
        vert = 0;

        // Bottom cap
        while (vert <= nbSides)
        {
            normales[vert++] = Vector3.down;
        }

        // Top cap
        while (vert <= nbSides * 2 + 1)
        {
            normales[vert++] = Vector3.up;
        }

        // Sides
        v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            normales[vert] = new Vector3(cos, 0f, sin);
            normales[vert + 1] = normales[vert];

            vert += 2;
            v++;
        }
        normales[vert] = normales[nbSides * 2 + 2];
        normales[vert + 1] = normales[nbSides * 2 + 3];
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];

        // Bottom cap
        int u = 0;
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= nbSides)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Top cap
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= nbSides * 2 + 1)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Sides
        int u_sides = 0;
        while (u <= uvs.Length - 4)
        {
            float t = (float)u_sides / nbSides;
            uvs[u] = new Vector3(t, 1f);
            uvs[u + 1] = new Vector3(t, 0f);
            u += 2;
            u_sides++;
        }
        uvs[u] = new Vector2(1f, 1f);
        uvs[u + 1] = new Vector2(1f, 0f);
        #endregion

        #region Triangles
        int nbTriangles = nbSides + nbSides + nbSides * 2;
        int[] triangles = new int[nbTriangles * 3 + 3];

        // Bottom cap
        int tri = 0;
        int i = 0;
        while (tri < nbSides - 1)
        {
            triangles[i] = 0;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 2;
            tri++;
            i += 3;
        }
        triangles[i] = 0;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = 1;
        tri++;
        i += 3;

        // Top cap
        //tri++;
        while (tri < nbSides * 2)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = nbVerticesCap;
            tri++;
            i += 3;
        }

        triangles[i] = nbVerticesCap + 1;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = nbVerticesCap;
        tri++;
        i += 3;
        tri++;

        // Sides
        while (tri <= nbTriangles)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;

            triangles[i] = tri + 1;
            triangles[i + 1] = tri + 2;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }


    protected override void Update()
    {
        base.Update();

        if (IsGrabbed())
        {
            var distance = (Point1.transform.position - Point2.transform.position).magnitude;
            length = Mathf.Clamp(distance, lengthMin, lengthMax);
            if (  Mathf.Pow(length - oldLength, 2f) > 0.1f)
            {
                UpdateRod();
                oldLength = length;
            }
        }

        if (!IsPlaying)
        {


            //set position
            
            //transform.up = (Point1.transform.position - transform.position);
        }
        else
        {

        }

    }

    public void UpdateRod()
    {

        transform.position = (Point1.transform.position + Point2.transform.position) / 2f;

        BuildMesh();
        GetComponent<CapsuleCollider>().height = length;
        GetComponent<Rigidbody>().mass = MassPerUnit * length;

    }

}
