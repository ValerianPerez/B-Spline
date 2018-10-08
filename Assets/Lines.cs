using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
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
    /// The Vector of B-Spline
    /// </summary>
    public int[] nodeVector;

    /// <summary>
    /// The tmp spline
    /// </summary>
    public List<Vector3> Spline;

    /// <summary>
    /// The B-Spline object
    /// </summary>
    public GameObject BSplineObject;

    /// <summary>
    /// The renderer of B-Spline
    /// </summary>
    private LineRenderer BSplineRenderer;

    /// <summary>
    /// The control points of b-spline
    /// </summary>
    [SerializeField]
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
        BSplineRenderer = Instantiate(BSplineObject).GetComponent<LineRenderer>();

        controlPoints = new List<Vector3>();
        Spline = new List<Vector3>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            controlPoints.Add(transform.GetChild(i).transform.position);
        }

        ControlPointsRenderer = GetComponent<LineRenderer>();
        ControlPointsRenderer.positionCount = controlPoints.Capacity;
        ControlPointsRenderer.SetPositions(controlPoints.ToArray());

        segmentStep = (float)controlPoints.Capacity / Resolution;

        nodeVector = new int[k + controlPoints.Capacity];

        for (int i = 0; i < nodeVector.Length; i++)
        {
            nodeVector[i] = i;
        }
    }

    /// <summary>
    /// Refresh the B-Spline with the current control points
    /// </summary>
    public void RefreshBSpline()
    {
        Vector3[] bSpline = BSpline();
        BSplineRenderer.positionCount = bSpline.Length;
        BSplineRenderer.SetPositions(bSpline);
    }

    /// <summary>
    /// Create the B-Spline with registered control points 
    /// </summary>
    /// <returns>Return the node vector of B-Spline</returns>
    private Vector3[] BSpline()
    {
        for (float i = k-1; i < controlPoints.Capacity; i += segmentStep)
        {
            Spline.Add(CreateBSpline(i));
        }
        return Spline.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="u"></param>
    /// <returns></returns>
    private Vector3 CreateBSpline(float u)
    {
        Vector3[] F = new Vector3[k];
        int dec = 0;
        
        for (int i = k; u > nodeVector[i]; i++, dec++);

        for (int i = 0; i < k; i++)
        {
            F[i] = controlPoints[dec + i];
        }

        for (int j = k; j > 1; j--)
        {
            for (int i = 0; i < k-1; i++)
            {
                Vector3 left = F[i];
                Vector3 right = F[i + 1];

                int min = nodeVector[dec + i + 1];
                int max = nodeVector[dec + j + i];

                float l_factor = (max - u) / (max - min);
                float r_factor = (u - min) / (max - min);

                F[i] = l_factor * left + r_factor * right;
            }
            dec++;
        }

        return F[0];
    }
}
