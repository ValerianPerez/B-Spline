using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour {

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

        foreach (Lines item in GetComponentsInChildren<Lines>())
        {
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
    }
}
