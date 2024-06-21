using System.Collections.Generic;
using UnityEngine;

public class MeshSeparator : MonoBehaviour
{
    [ContextMenu("Separate Meshes")]
   
    void Run()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            SplitMesh(meshFilter.mesh);
        }
    }

    void SplitMesh(Mesh mesh)
    {
        var vertices = mesh.vertices;
        var triangles = mesh.triangles;
        var uvs = mesh.uv;
        var visited = new HashSet<int>();
        var meshes = new List<Mesh>();

        for (int i = 0; i < vertices.Length; i++)
        {
            if (!visited.Contains(i))
            {
                var newMeshVertices = new List<Vector3>();
                var newMeshTriangles = new List<int>();
                var newMeshUVs = new List<Vector2>();

                DFS(i, vertices, triangles, uvs, visited, newMeshVertices, newMeshTriangles, newMeshUVs);

                Mesh newMesh = new Mesh();
                newMesh.SetVertices(newMeshVertices);
                newMesh.SetTriangles(newMeshTriangles, 0);
                newMesh.SetUVs(0, newMeshUVs);
                meshes.Add(newMesh);
            }
        }

        // Instantiate new GameObjects for each separated mesh
        foreach (var newMesh in meshes)
        {
            GameObject newMeshObject = new GameObject("SubMesh");
            var meshFilter = newMeshObject.AddComponent<MeshFilter>();
            meshFilter.mesh = newMesh;
            newMeshObject.AddComponent<MeshRenderer>();
        }
    }

    void DFS(int index, Vector3[] vertices, int[] triangles, Vector2[] uvs, HashSet<int> visited, List<Vector3> newMeshVertices, List<int> newMeshTriangles, List<Vector2> newMeshUVs)
    {
        visited.Add(index);
        int newVertexIndex = newMeshVertices.Count;
        newMeshVertices.Add(vertices[index]);
        newMeshUVs.Add(uvs[index]);

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (triangles[i] == index || triangles[i + 1] == index || triangles[i + 2] == index)
            {
                for (int j = 0; j < 3; j++)
                {
                    int vertexIndex = triangles[i + j];
                    if (!visited.Contains(vertexIndex))
                    {
                        DFS(vertexIndex, vertices, triangles, uvs, visited, newMeshVertices, newMeshTriangles, newMeshUVs);
                    }
                }

                newMeshTriangles.Add(newVertexIndex);
                newMeshTriangles.Add(newMeshVertices.IndexOf(vertices[triangles[i + 1]]));
                newMeshTriangles.Add(newMeshVertices.IndexOf(vertices[triangles[i + 2]]));
            }
        }
    }
}