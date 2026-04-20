using UnityEngine;

namespace Backrooms
{
    public class CeilingCreator
    {
        public void CreateCeiling(Vector2 bottomLeftAreaCorner, Vector2 topRightAreaCorner, float height, 
                        Material material, Transform parent)
        {
            Vector3 bottomLeftPoint = new Vector3(bottomLeftAreaCorner.x, height, bottomLeftAreaCorner.y);
            Vector3 topRightPoint = new Vector3(topRightAreaCorner.x, height, topRightAreaCorner.y);
            Vector3 bottomRightPoint = new Vector3(topRightAreaCorner.x, height, bottomLeftAreaCorner.y);
            Vector3 topLeftPoint = new Vector3(bottomLeftAreaCorner.x, height, topRightAreaCorner.y);

            Vector3[] vertices = new Vector3[]
            {
                topLeftPoint,
                topRightPoint,
                bottomLeftPoint,
                bottomRightPoint
            };

            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < uvs.Length; i++)
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);

            int[] triangles = new int[]
            {
                0, 
                2, 
                1,
                2, 
                3, 
                1
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            GameObject meshGo = new GameObject($"Mesh_Ceiling_{bottomLeftAreaCorner}", typeof(MeshFilter), typeof(MeshRenderer));
            
            if (parent != null)
                meshGo.transform.parent = parent;
            
            meshGo.transform.position = Vector3.zero;
            meshGo.GetComponent<MeshFilter>().mesh = mesh;
            meshGo.GetComponent<MeshRenderer>().material = material;
        }
    }
}
