using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeGenerator : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    public Vector3 offset = Vector3.zero;
    public Color cubeColor = Color.white;

    void OnValidate()
    {
        EnsureMeshComponents();
        GenerateCube();
    }

    void EnsureMeshComponents()
    {
        if (GetComponent<MeshFilter>() == null)
        {
            gameObject.AddComponent<MeshFilter>();
        }

        if (GetComponent<MeshRenderer>() == null)
        {
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            // Create and assign a Lit URP material
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.material = material;
        }

        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    void GenerateCube()
    {
        Mesh mesh = new Mesh();
        mesh.name = "FacetedCube";

        // Define the vertices of the cube
        Vector3 halfSize = size * 0.5f;
        Vector3[] vertices = {
            // Front face
            offset + new Vector3(-halfSize.x, -halfSize.y,  halfSize.z),
            offset + new Vector3( halfSize.x, -halfSize.y,  halfSize.z),
            offset + new Vector3( halfSize.x,  halfSize.y,  halfSize.z),
            offset + new Vector3(-halfSize.x,  halfSize.y,  halfSize.z),

            // Back face
            offset + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x, -halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x,  halfSize.y, -halfSize.z),
            offset + new Vector3(-halfSize.x,  halfSize.y, -halfSize.z),

            // Left face
            offset + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            offset + new Vector3(-halfSize.x, -halfSize.y,  halfSize.z),
            offset + new Vector3(-halfSize.x,  halfSize.y,  halfSize.z),
            offset + new Vector3(-halfSize.x,  halfSize.y, -halfSize.z),

            // Right face
            offset + new Vector3( halfSize.x, -halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x, -halfSize.y,  halfSize.z),
            offset + new Vector3( halfSize.x,  halfSize.y,  halfSize.z),
            offset + new Vector3( halfSize.x,  halfSize.y, -halfSize.z),

            // Top face
            offset + new Vector3(-halfSize.x,  halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x,  halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x,  halfSize.y,  halfSize.z),
            offset + new Vector3(-halfSize.x,  halfSize.y,  halfSize.z),

            // Bottom face
            offset + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x, -halfSize.y, -halfSize.z),
            offset + new Vector3( halfSize.x, -halfSize.y,  halfSize.z),
            offset + new Vector3(-halfSize.x, -halfSize.y,  halfSize.z),
        };

        // Define the triangles of the cube (two per face)
        int[] triangles = {
            // Front face
            0, 1, 2,
            0, 2, 3,

            // Back face
            4, 6, 5,
            4, 7, 6,

            // Left face
            8, 9, 10,
            8, 10, 11,

            // Right face
            12, 14, 13,
            12, 15, 14,

            // Top face
            16, 18, 17,
            16, 19, 18,

            // Bottom face
            20, 21, 22,
            20, 22, 23,
        };

        // Define the inverted normals of the vertices
        Vector3[] normals = {
            // Front face
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,

            // Back face
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,

            // Left face
            Vector3.right, Vector3.right, Vector3.right, Vector3.right,

            // Right face
            Vector3.left, Vector3.left, Vector3.left, Vector3.left,

            // Top face
            Vector3.down, Vector3.down, Vector3.down, Vector3.down,

            // Bottom face
            Vector3.up, Vector3.up, Vector3.up, Vector3.up,
        };

        // Define the UVs for texturing
        Vector2[] uv = {
            // Front face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Back face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Left face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Right face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Top face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Bottom face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Set the material color
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = cubeColor;

        // Ensure the BoxCollider is correctly sized and positioned
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = size;
        boxCollider.center = offset;
    }
    
    private void OnDrawGizmos()
    {
        // Get the MeshFilter component attached to this GameObject
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        // Get the mesh and its vertices and normals
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        // Iterate over each vertex and draw the normal
        for (int i = 0; i < vertices.Length; i++)
        {
            // Transform the vertex and normal from local space to world space
            Vector3 vertex = transform.TransformPoint(vertices[i]);
            Vector3 normal = transform.TransformDirection(normals[i]);

            // Draw a line from the vertex along the normal
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(vertex, vertex + normal * 0.2f);
        }
    }
}
