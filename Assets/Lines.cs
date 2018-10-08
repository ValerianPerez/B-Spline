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

    public List<Vector3> spline;

    /// <summary>
    /// The renderer of B-Spline
    /// </summary>
    public LineRenderer BSplineRenderer;

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

        controlPoints = new List<Vector3>();
        spline = new List<Vector3>();

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
            /*
            if (i > k && i <= controlPoints.Capacity)
            {
                nodeVector[i] = ++nodeNumber;
            }
            else
            {
                nodeVector[i] = nodeNumber;
            }
            */
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
        for (float i = 0.0f; i < controlPoints.Capacity; i += segmentStep)
        {
            Vector3 p = NewBSpline(i);
            spline.Add(p);
            Debug.Log(p);
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

        return factor * DeBoor(k - 1, ti, t) + (1 - factor) * DeBoor(k - 1, ti + 1, t);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="u"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    private Vector3 NewBSpline(float u)
    {

        Vector3[] F = new Vector3[k];
        int dec = 0;
        for (int i = k; u > nodeVector[i]; i++, dec++) ;
        //int index = k;
        //while (u > nodeVector[index])
        //{
        //    index++;
        //    dec++;
        //}


        for (int i = 0; i < k; i++)
        {
            F[i] = controlPoints[dec + i];
                Debug.Log(F[0]);
        }

        //while (index > 0)
        for (int j = k; j > 1; j--)
        {
            for (int i = 0; i < k-1; i++)
            {
                Vector3 left = F[i];
                Vector3 right = F[i + 1];

                int min = nodeVector[dec + j + 1];
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
