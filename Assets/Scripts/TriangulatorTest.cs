using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangulatorTest : MonoBehaviour
{
    private Material _mat;
    void Start()
    {
        GameObject go = new GameObject();
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        _mat = Resources.Load("White", typeof(Material)) as Material;
        mr.material = _mat;
        go.AddComponent<MeshCollider>();


        Mesh mesh = mf.mesh;
        
        List<Vector3> vericies = new List<Vector3>();
        List<Vector2> points = new List<Vector2>();
        List<List<Vector2>> holes = new List<List<Vector2>>();
        List<int> indices = new List<int>();

        points.Add(new Vector2(10, 0));
        points.Add(new Vector2(20, 0));
        points.Add(new Vector2(20, 10));
        points.Add(new Vector2(30, 10));
        points.Add(new Vector2(30, 20));
        points.Add(new Vector2(20, 20));
        points.Add(new Vector2(20, 30));
        points.Add(new Vector2(10, 30));
        points.Add(new Vector2(10, 20));
        points.Add(new Vector2(0, 20));
        points.Add(new Vector2(0, 10));
        points.Add(new Vector2(10, 10));

        List<Vector2> hole = new List<Vector2>
        {
            new Vector2(12, 12), 
            new Vector2(18, 12), 
            new Vector2(18, 18), 
            new Vector2(12, 18)
        };

        holes.Add(hole);
        
        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);
        }
        
        Triangulation.Triangulate(points, holes, out indices, out vericies);
        
        mesh.Clear();
        mesh.vertices = vericies.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        go.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
