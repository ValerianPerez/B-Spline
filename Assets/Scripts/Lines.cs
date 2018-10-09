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
    /// Define if the open nodal vector is used or not
    /// </summary>
    public bool UseOpenNodalVector { get; private set; }

    /// <summary>
    /// The curve's order
    /// </summary>
    private int k;

    /// <summary>
    /// The number of point on the B-Spline
    /// </summary>
    private int resolution;

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

    /// <summary>
    /// Set up the lines of control points
    /// </summary>
    /// <param name="k">the order of the curve</param>
    /// <param name="resolution">The number of points in curve</param>
    public void Setup(int k, int resolution, bool useOpenNodalVector)
    {
        this.k = k;
        this.resolution = resolution;
        this.UseOpenNodalVector = useOpenNodalVector;

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
        CustomBSpline = new BSpline(k, resolution, ControlPoints, UseOpenNodalVector);
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

    /// <summary>
    /// Return the points of spline
    /// </summary>
    /// <returns>List of points of spline</returns>
    public List<Vector3> GetSplinePoints()
    {
        return CustomBSpline.Spline;
    }
}
