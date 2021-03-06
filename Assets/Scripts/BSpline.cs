﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline
{
    /// <summary>
    /// The tmp spline
    /// </summary>
    public List<Vector3> Spline { get; private set; }

    /// <summary>
    /// Define if the open nodal vector is used or not
    /// </summary>
    private bool isOpenNodalVector;

    /// <summary>
    /// The curve's order
    /// </summary>
    private readonly int k;

    /// <summary>
    /// The number of point on the B-Spline
    /// </summary>
    private readonly int Resolution;

    /// <summary>
    /// The Vector of B-Spline
    /// </summary>
    private int[] nodeVector;

    /// <summary>
    /// The control points of b-spline
    /// </summary>
    private List<Vector3> controlPoints;

    /// <summary>
    /// The step for segmentation
    /// </summary>
    private readonly float segmentStep;

    /// <summary>
    /// The constructir for the BSpline
    /// </summary>
    /// <param name="k">Degree of curve</param>
    /// <param name="Resolution">Number of points</param>
    /// <param name="controlPoints">List of control points positions</param>
    public BSpline(int k, int resolution, List<Vector3> controlPoints, bool isOpenNodalVector)
    {
        this.controlPoints = controlPoints;
        this.Resolution = resolution;
        this.k = k;
        this.isOpenNodalVector = isOpenNodalVector;

        Spline = new List<Vector3>();

        segmentStep = (float)controlPoints.Count / Resolution;
        nodeVector = new int[k + controlPoints.Count];

        int nodeNumber = 0;
        for (int i = 0; i < nodeVector.Length; i++)
        {
            if (this.isOpenNodalVector)
            {
                if (i > k - 1 && i < controlPoints.Count+1)
                {
                    nodeNumber++;
                }
                nodeVector[i] = nodeNumber;
            }
            else
            {
                nodeVector[i] = i;
            }
            
        }
    }    

    /// <summary>
    /// Create the B-Spline with registered control points 
    /// </summary>
    /// <returns>Return the discrete position of B-Spline</returns>
    public Vector3[] Generate()
    {
        for (float u = nodeVector[k - 1]; u < nodeVector[controlPoints.Count]; u += segmentStep)
        {
            Spline.Add(CreateBSpline(u));
        }
        return Spline.ToArray();
    }

    /// <summary>
    /// Create the curernt point of BSpline
    /// </summary>
    /// <param name="u">Value of current point</param>
    /// <returns>Returns the point associated to the value</returns>
    private Vector3 CreateBSpline(float u)
    {
        Vector3[] F = new Vector3[k];
        int dec = 0;

        for (int i = k; u > nodeVector[i]; i++, dec++) ;

        for (int i = 0; i < k; i++)
        {
            F[i] = controlPoints[dec + i];
        }

        for (int j = k; j > 1; j--)
        {
            for (int i = 0; i < k - 1; i++)
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
