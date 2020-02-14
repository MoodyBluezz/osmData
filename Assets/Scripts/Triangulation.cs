using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;
using Polygon = TriangleNet.Geometry;

public class Triangulation {

    public static bool Triangulate(List<Vector2> points, List<List<Vector2>> holes, out List<int> outIndices, out List<Vector3> outVertices)
    {
        outVertices = new List<Vector3>();
        outIndices = new List<int>();
        Polygon.Polygon poly = new Polygon.Polygon();
        
        //Points and Segments
        for (int i = 0; i < points.Count; i++)
        {
            poly.Add(new Polygon.Vertex(points[i].x, points[i].y));

            if (i == points.Count - 1)
            {
                poly.Add(new Polygon.Segment(new Polygon.Vertex(points[i].x, points[i].y), new Polygon.Vertex(points[0].x, points[1].y)));
            }
            else
            {
                poly.Add(new Polygon.Segment(new Polygon.Vertex(points[i].x, points[i].y), new Polygon.Vertex(points[i+1].x, points[i+1].y)));
            }
        }
        
        //Holes
        for (int i = 0; i < holes.Count; i++)
        {
            List<Polygon.Vertex> vertices = new List<Polygon.Vertex>();
            for (int j = 0; j < holes[i].Count; j++)
            {
                vertices.Add(new Polygon.Vertex(holes[i][j].x, holes[i][j].y));
            }
            poly.Add(new Polygon.Contour(vertices), true);
        }

        var mesh = poly.Triangulate();

        foreach (ITriangle t in mesh.Triangles)
        {
            for (int j = 2; j >= 0; j--)
            {
                bool found = false;
                for (int k = 0; k < outVertices.Count; k++)
                {
                    if (outVertices[k].x == t.GetVertex(j).X && (outVertices[k].z == t.GetVertex(j).Y))
                    {
                        outIndices.Add(k);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    outVertices.Add(new Vector3((float)t.GetVertex(j).X, 0, (float)t.GetVertex(j).Y));
                    outIndices.Add(outVertices.Count - 1);
                }
            }
        }
        return true;
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;
using Polygon = TriangleNet.Geometry;
*/

/*public class Triangulation : MonoBehaviour {
    public Material building;
    protected static MapReader map;
    
    void Awake()
    {
        map = GetComponent<MapReader>();
    }
    public static bool Triangulate( OsmWay way, Vector3 origin, List<Vector3> normals, List<Vector2> uvs, out List<int> outIndices, out List<Vector3> outVertices)
    {
        outVertices = new List<Vector3>();
        outIndices = new List<int>();
        Polygon.Polygon poly = new Polygon.Polygon();

        var mesh = poly.Triangulate();

        // Get the centre of the roof
        Vector3 oTop = new Vector3(0, way.Height, 0);
		
        // First vector is the middle point in the roof
        outVertices.Add(oTop);
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0.5f, 0.5f));
		
        for (int i = 1; i < way.NodeIDs.Count; i++)
        {
            OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
            OsmNode p2 = map.nodes[way.NodeIDs[i]];
            //Debug.Log(way.NodeIDs[i]);
            Vector3 v1 = p1 - origin;
            Vector3 v2 = p2 - origin;
            Vector3 v3 = v1 + new Vector3(0, way.Height, 0);
            Vector3 v4 = v2 + new Vector3(0, way.Height, 0);

            outVertices.Add(v1);
            outVertices.Add(v2);
            outVertices.Add(v3);
            outVertices.Add(v4);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);

            int idx1, idx2, idx3, idx4;
            idx4 = outVertices.Count - 1;
            idx3 = outVertices.Count - 2;
            idx2 = outVertices.Count - 3;
            idx1 = outVertices.Count - 4;

            // first triangle v1, v3, v2
            outIndices.Add(idx1);
            outIndices.Add(idx3);
            outIndices.Add(idx2);
     
            // second         v3, v4, v2
            outIndices.Add(idx3);
            outIndices.Add(idx4);
            outIndices.Add(idx2);

            // third          v2, v3, v1
            outIndices.Add(idx2);
            outIndices.Add(idx3);
            outIndices.Add(idx1);
            
            // fourth         v2, v4, v3
           outIndices.Add(idx2);
           outIndices.Add(idx4);
           outIndices.Add(idx3);
    
            // And now the roof triangles
            outIndices.Add(0);
            outIndices.Add(idx3);
            outIndices.Add(idx4);
            
            // Don't forget the upside down one!
            outIndices.Add(idx4);
            outIndices.Add(idx3);
            outIndices.Add(0);
        }
        
        foreach (ITriangle t in mesh.Triangles)
        {
            for (int j = 2; j >= 0; j--)
            {
                bool found = false;
                for (int k = 0; k < outVertices.Count; k++)
                {
                    if (outVertices[k].x == t.GetVertex(j).X && (outVertices[k].z == t.GetVertex(j).Y))
                    {
                        outIndices.Add(k);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    outVertices.Add(new Vector3((float)t.GetVertex(j).X, 0, (float)t.GetVertex(j).Y));
                    outIndices.Add(outVertices.Count - 1);
                }
            }
        }
        return true;
    }
}*/
