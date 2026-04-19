using UnityEngine;

namespace Backrooms
{
    public class FloorCreator
    {
        public void CreateFloor(Vector2 bottomLeftAreaCorner, Vector2 topRightAreaCorner, Material material, Transform parent)
        {
            Vector3 bottomLeftPoint = new Vector3(bottomLeftAreaCorner.x, 0, bottomLeftAreaCorner.y);
            Vector3 topRightPoint = new Vector3(topRightAreaCorner.x, 0, topRightAreaCorner.y);
            Vector3 bottomRightPoint = new Vector3(topRightAreaCorner.x, 0, bottomLeftAreaCorner.y);
            Vector3 topLeftPoint = new Vector3(bottomLeftAreaCorner.x, 0, topRightAreaCorner.y);

            Vector3[] vertices = new Vector3[]
            {
                topLeftPoint,
                topRightPoint,
                bottomLeftPoint,
                bottomRightPoint
            };

            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }

            int[] triangles = new int[]
            {
                0, 
                1, 
                2, 
                2, 
                1, 
                3
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            GameObject meshGo = new GameObject($"Mesh_Floor_{bottomLeftAreaCorner}", typeof(MeshFilter), typeof(MeshRenderer));
            
            if (parent != null)
                meshGo.transform.parent = parent;

            meshGo.transform.localPosition = Vector3.zero;
            meshGo.transform.localScale = Vector3.one;
            meshGo.transform.rotation = Quaternion.identity;
            
            meshGo.GetComponent<MeshFilter>().mesh = mesh;
            meshGo.GetComponent<MeshRenderer>().material = material;
        }
    }
}