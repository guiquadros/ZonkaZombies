using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class ProceduralMeshTest : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public Mesh Mesh;

	// Use this for initialization
	void Start ()
	{
	    MeshFilter = GetComponent<MeshFilter>();
        Mesh = new Mesh();
	}
	
	// Update is called once per frame
	void Update () {
		Mesh.Clear();

	    Vector2 v0 = Vector2.down + Vector2.left;
	    Vector2 v1 = Vector2.up + Vector2.left;
	    Vector2 v2 = Vector2.up + Vector2.right;
	    Vector2 v3 = Vector2.down + Vector2.right;

        Mesh.vertices = new Vector3[] {v0, v1, v2, v3};
        Mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3};

	    MeshFilter.mesh = Mesh;
	}
}
