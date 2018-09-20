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
    public Vector3[] nodeVector;

    /// <summary>
    /// The renderer of B-Spline
    /// </summary>
    public LineRenderer BSplineRenderer;

    /// <summary>
    /// The control points of b-spline
    /// </summary>
    private List<Vector3> points;

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

        points = new List<Vector3>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            points.Add(transform.GetChild(i).transform.position);
        }

        ControlPointsRenderer = GetComponent<LineRenderer>();
        ControlPointsRenderer.positionCount = points.Capacity;
        ControlPointsRenderer.SetPositions(points.ToArray());

        segmentStep = 1 / NbSegments;

        nodeVector = new Vector3[NbSegments * points.Capacity];

        BSplineRenderer.SetPositions(BSpline());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Create the B-Spline with registered control points 
    /// </summary>
    /// <returns>Return the node vector of B-Spline</returns>
    private Vector3[] BSpline()
    {
        int index = 0;

        for (int i = 0; i < n + points.Capacity; i++)
        {
            float t = i;
            while (t < i + 1)
            {
                Debug.Log(t);
                //nodeVector[index++] = DeBoor(n, i, t) * points[i];
                t += segmentStep;
            }
        }

        return nodeVector;
    }

    /// <summary>
    /// The function which compute the deBoor factor
    /// </summary>
    /// <param name="k">The degree of calculation</param>
    /// <param name="ti">The current node</param>
    /// <param name="t">The current time</param>
    /// <returns></returns>
    private float DeBoor(int k, int ti, float t)
    {
        if (k == 0)
        {
            if (ti < t && t < ti + 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        float factor = (t - ti) / (k);

        return factor * DeBoor(k-1, ti, t) + (1-factor) * DeBoor(k-1, ti+1, t);
    }
}
