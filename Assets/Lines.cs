using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{
    /// <summary>
    /// The curve's degree
    /// </summary>
    public int n = 2;

    /// <summary>
    /// The number of segments for each nodes
    /// </summary>
    public int NbSegments = 10;

    /// <summary>
    /// The Vector of B-Spline
    /// </summary>
    public int[] nodeVector;

    public List<Vector3> spline;

    /// <summary>
    /// The renderer of B-Spline
    /// </summary>
    public LineRenderer BSplineRenderer;

    /// <summary>
    /// The control points of b-spline
    /// </summary>
    private List<Vector3> controlPoints;

    /// <summary>
    /// The line renderer for control points
    /// </summary>
    private LineRenderer ControlPointsRenderer;

    /// <summary>
    /// The step for segmentation
    /// </summary>
    private float segmentStep;

    // Use this for initialization
    void Start()
    {

        controlPoints = new List<Vector3>();
        spline = new List<Vector3>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            controlPoints.Add(transform.GetChild(i).transform.position);
        }

        ControlPointsRenderer = GetComponent<LineRenderer>();
        ControlPointsRenderer.positionCount = controlPoints.Capacity;
        ControlPointsRenderer.SetPositions(controlPoints.ToArray());

        segmentStep = (float) 1 / NbSegments;

        nodeVector = new int[n + controlPoints.Capacity + 1];

        int nodeNumber = 0;

        for (int i = 0; i < nodeVector.Length; i++)
        {
            if (i > n && i <= controlPoints.Capacity)
            {
                nodeVector[i] = ++nodeNumber;
            }
            else
            {
                nodeVector[i] = nodeNumber;
            }
        }
    }

    /// <summary>
    /// Refresh the B-Spline with the current control points
    /// </summary>
    public void RefreshBSpline()
    {
        BSplineRenderer.SetPositions(BSpline());
    }

    /// <summary>
    /// Create the B-Spline with registered control points 
    /// </summary>
    /// <returns>Return the node vector of B-Spline</returns>
    
    private Vector3[] BSpline()
    {
        
        int index = 0;
        
        for (int i = 0; i < controlPoints.Capacity ; i++)
        {
            for (float t = i; t < i + 1; t += segmentStep)
            {
                spline.Add(DeBoor(n, i, t));
            }
        }
        return spline.ToArray();
        
    }
    

    /// <summary>
    /// The function which compute the deBoor factor
    /// </summary>
    /// <param name="k">The degree of calculation</param>
    /// <param name="ti">The current node</param>
    /// <param name="t">The current time</param>
    /// <returns>Return the new point</returns>
    private Vector3 DeBoor(int k, int ti, float t)
    {
        if (k == 0)
        {
            //if (ti <= t && t < ti + 1)
            //{
                return controlPoints[ti];
            //}
            //else
            //{
                //return Vector3.zero;
            //}
        }

        float factor = (t - nodeVector[ti]) / (nodeVector[ti + k] - nodeVector[ti]);

        return factor * DeBoor(k-1, ti, t) + (1-factor) * DeBoor(k-1, ti+1, t);
    }
}
