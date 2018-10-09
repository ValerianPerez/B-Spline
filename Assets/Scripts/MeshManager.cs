using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour {

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
    private List<List<Vector3>> mesh;

	// Use this for initialization
	void Start () {
        lines = new List<Lines>();
        mesh = new List<List<Vector3>>();
        UseOpenNodalVector = true;

        foreach (Lines item in GetComponentsInChildren<Lines>())
        {
            item.Setup(k, Resolution, UseOpenNodalVector);
            lines.Add(item);
            mesh.Add(item.ControlPoints);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Generate all B-splines
    /// </summary>
    public void GenerateBSplines()
    {
        foreach (Lines item in lines)
        {
            item.RefreshBSpline();
        }

        //for (int i = 0; i < mesh[0].Capacity; i++)
        //{
        //    List<Vector3> cp = new List<Vector3>();

        //    for (int j = 0; j < mesh.Capacity; j++)
        //    {
        //        cp.Add(mesh[j][i]);
        //    }

        //    BSpline bSpline = new BSpline(k, Resolution, cp);
        //    //Create an empty line renderer
        //    BSplineRenderer = Instantiate(BSplineObject).GetComponent<LineRenderer>();
        //    BSplineRenderer.positionCount = cp.Capacity;
        //    BSplineRenderer.SetPositions(bSpline.Generate());
        //}
    }
}
