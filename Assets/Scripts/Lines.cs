using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{
    /// <summary>
    /// The control points of b-spline
    /// </summary>
    public List<Vector3> ControlPoints { get; private set; }

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
    /// The renderer of B-Spline
    /// </summary>
    private LineRenderer BSplineRenderer;

    /// <summary>
    /// The line renderer for control points
    /// </summary>
    private LineRenderer ControlPointsRenderer;

    /// <summary>
    /// The <see cref="BSpline"/> class for computation
    /// </summary>
    private BSpline CustomBSpline;

    // Use this for initialization
    void Start()
    {
        //Create an empty line renderer
        BSplineRenderer = Instantiate(BSplineObject).GetComponent<LineRenderer>();
        BSplineRenderer.positionCount = 0;

        //Find & add control points
        ControlPoints = new List<Vector3>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            ControlPoints.Add(transform.GetChild(i).transform.position);
        }

        //Set the control polygon
        ControlPointsRenderer = GetComponent<LineRenderer>();
        ControlPointsRenderer.positionCount = ControlPoints.Capacity;
        ControlPointsRenderer.SetPositions(ControlPoints.ToArray());

        //Create a new BSpline class
        CustomBSpline = new BSpline(k, Resolution, ControlPoints);
    }

    /// <summary>
    /// Refresh the B-Spline with the current control points
    /// </summary>
    public void RefreshBSpline()
    {
        Vector3[] bSpline = CustomBSpline.Generate();
        BSplineRenderer.positionCount = bSpline.Length;
        BSplineRenderer.SetPositions(bSpline);
    }

}
