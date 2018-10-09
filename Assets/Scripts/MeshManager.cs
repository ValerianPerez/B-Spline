using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{

    /// <summary>
    /// The curve's order
    /// </summary>
    public int k = 2;

    /// <summary>
    /// The number of point on the B-Spline
    /// </summary>
    public int Resolution = 10;

    /// <summary>
    /// The B-Spline object
    /// </summary>
    public GameObject BSplineObject;

    /// <summary>
    /// Define if the open nodal vector is used or not
    /// </summary>
    public bool UseOpenNodalVector { get; private set; }

    /// <summary>
    /// The renderer of B-Spline
    /// </summary>
    private LineRenderer BSplineRenderer;

    /// <summary>
    /// The different lines of controle points
    /// </summary>
    private List<Lines> lines;

    /// <summary>
    /// The 2D List of mesh control points
    /// </summary>
    private List<List<Vector3>> meshPoints;

    // Use this for initialization
    void Start()
    {
        lines = new List<Lines>();
        meshPoints = new List<List<Vector3>>();
        UseOpenNodalVector = true;

        foreach (Lines item in GetComponentsInChildren<Lines>())
        {
            item.Setup(k, Resolution, UseOpenNodalVector);
            lines.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Generate all B-splines
    /// </summary>
    public void GenerateBSplines()
    {
        foreach (Lines item in lines)
        {
            item.RefreshBSpline();
            meshPoints.Add(item.GetSplinePoints());
        }


        for (int i = 0; i < meshPoints[0].Count; i++)
        {

            List<Vector3> cp = new List<Vector3>();

            for (int j = 0; j < meshPoints.Count; j++)
            {
                cp.Add(meshPoints[j][i]);
            }

            BSpline bSpline = new BSpline(k, Resolution, cp, UseOpenNodalVector);

            //Create a line renderer
            BSplineRenderer = Instantiate(BSplineObject).GetComponent<LineRenderer>();
            Vector3[] tmp = bSpline.Generate();
            BSplineRenderer.positionCount = tmp.Length;
            BSplineRenderer.SetPositions(tmp);
        }

        CreateMesh();
    }

    /// <summary>
    /// Create the mesh from points of mesh
    /// </summary>
    private void CreateMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        for (int i = 0; i < meshPoints.Count - 1; i++)
        {
            for (int j = 0; j < meshPoints[i].Count - 1; j++)
            {

            }
        }
    }
}
