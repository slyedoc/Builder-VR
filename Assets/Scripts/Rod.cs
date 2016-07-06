using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using VRTK;


[ExecuteInEditMode]
public class Rod : Builder_InteractableObject
{
    [Header("Rod Settings", order = 5)]
    
    public float length = 1;
    public float lengthMax = 10;
    public float lengthMin = 0.2f;
    
    public float MassPerUnit = 10;
    private float radius = .05f;
    private int nbSides = 64;
    private int nbHeightSeg = 1; // Not implemented yet
    private Mesh mesh = null;

    private float oldLength;    

    protected override void Awake()
    {
        base.Awake();
        BuildMesh();
        cm.ConnectionCreated += ConnectionCreated;
    }

    private void ConnectionCreated(object sender)
    {
        Debug.Log("Changing grab type");
        var go = grabbingObject;
        if( go )
            Ungrabbed(grabbingObject);
        grabAttachMechanic = GrabAttachType.Track_Object;
        if( go )
            Grabbed(go);
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

        if (IsGrabbed() && cm.connections.Count == 2)
        {            
            var distance = (cm.connections[0].first.transform.position - cm.connections[1].first.transform.position).magnitude;
            length = Mathf.Clamp(distance, lengthMin, lengthMax);
            if (length != oldLength)
            {
                //update position
                transform.position = (cm.connections[0].first.transform.position + cm.connections[1].first.transform.position) / 2f;

                //update mesh
                BuildMesh();

                GetComponent<CapsuleCollider>().height = length;
                GetComponent<Rigidbody>().mass = MassPerUnit * length;

                //update connection joints
                UpdateConnectionJoint(cm.connections[0].second, new Vector3(0, length / 2, 0));
                UpdateConnectionJoint(cm.connections[1].second, new Vector3(0, -length / 2, 0));

                oldLength = length;
            }
        }
    }

    private void UpdateConnectionJoint(ConnectionJoint joint, Vector3 connectedAnchor)
    {
        if (joint != null)
        {
            joint.UpdateJoint(rb, connectedAnchor);
        }
    }
}
