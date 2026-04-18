using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class SpaceCreator : MonoBehaviour
    {
        // Inspector
        [Header("Parameters")]

        [Header("Total Space")]
        [SerializeField] private int _totalWidth;
        [SerializeField] private int _totalLength;

        [Header("Single Room")]
        [SerializeField] private int _roomMinWidth;
        [SerializeField] private int _roomMinLength;
        [SerializeField, Range(0.0f, 0.3f)] private float _roomBottomLeftModifier;
        [SerializeField, Range(0.7f, 1f)] private float _roomTopRightModifier;
        [SerializeField, Range(0, 2)] private int _roomOffset;

        [Header("Single Corridor")]
        [SerializeField] private int _corridorWidth;

        [Header("Creation")]
        [SerializeField] private int _maxIterations;

        [Header("References")]
        [SerializeField] private Material _floorMaterial;
        [SerializeField] private GameObject _wallHorizontal;
        [SerializeField] private GameObject _wallVertical;

        // Not serialized
        private SpaceGenerator _generator;

        private List<Vector3Int> _possibleDoorHorizontalPosition;
        private List<Vector3Int> _possibleDoorVerticalPosition;

        private List<Vector3Int> _possibleWallHorizontalPosition;
        private List<Vector3Int> _possibleWallVerticalPosition;


        private void Awake() => _generator = new SpaceGenerator(_totalWidth, _totalLength);

        private void Start() => CreateSpace();

        private void CreateSpace()
        {
            List<Node> roomsList = _generator.CalculateSpace
            (
                _maxIterations,
                _roomMinWidth,
                _roomMinLength,
                _roomBottomLeftModifier,
                _roomTopRightModifier,
                _roomOffset,
                _corridorWidth
            );

            GameObject wallParent = new GameObject("WallParent");
            wallParent.transform.parent = gameObject.transform;

            _possibleDoorHorizontalPosition = new List<Vector3Int>();
            _possibleDoorVerticalPosition = new List<Vector3Int>();

            _possibleWallHorizontalPosition = new List<Vector3Int>();
            _possibleWallVerticalPosition = new List<Vector3Int>();

            foreach (Node room in roomsList)
                CreateMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner, "Mesh_Floor_", _floorMaterial);

            CreateWalls(wallParent);
        }

        private void CreateWalls(GameObject wallParent)
        {
            foreach (Vector3Int wallPosition in _possibleWallHorizontalPosition)
                CreateWall(wallParent, wallPosition, _wallHorizontal);

            foreach (Vector3Int wallPosition in _possibleWallVerticalPosition)
                CreateWall(wallParent, wallPosition, _wallVertical);
        }

        private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
        {
            Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        }

        private void CreateMesh(Vector2 bottomLeftAreaCorner, Vector2 topRightAreaCorner,
                    string prefixName, Material material)
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
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);

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

            GameObject meshGo = new GameObject($"{prefixName}{bottomLeftAreaCorner}", typeof(MeshFilter), typeof(MeshRenderer));
            meshGo.transform.position = Vector3.zero;
            meshGo.transform.localScale = Vector3.one;
            meshGo.transform.rotation = Quaternion.identity;
            meshGo.GetComponent<MeshFilter>().mesh = mesh;
            meshGo.GetComponent<MeshRenderer>().material = material;

            for (int row = (int)bottomLeftPoint.x; row < (int)bottomRightPoint.x; row++)
            {
                Vector3 wallPosition = new Vector3(row, 0, bottomLeftPoint.z);
                AddWallPositionToList(wallPosition, _possibleWallHorizontalPosition, _possibleDoorHorizontalPosition);
            }

            for (int row = (int)topLeftPoint.x; row < (int)topRightPoint.x; row++)
            {
                Vector3 wallPosition = new Vector3(row, 0, topRightPoint.z);
                AddWallPositionToList(wallPosition, _possibleWallHorizontalPosition, _possibleDoorHorizontalPosition);
            }

            for (int col = (int)bottomLeftPoint.z; col < (int)topLeftPoint.z; col++)
            {
                Vector3 wallPosition = new Vector3(bottomLeftPoint.x, 0, col);
                AddWallPositionToList(wallPosition, _possibleWallVerticalPosition, _possibleDoorVerticalPosition);
            }

            for (int col = (int)bottomRightPoint.z; col < (int)topRightPoint.z; col++)
            {
                Vector3 wallPosition = new Vector3(bottomRightPoint.x, 0, col);
                AddWallPositionToList(wallPosition, _possibleWallVerticalPosition, _possibleDoorVerticalPosition);
            }
        }

        private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
        {
            Vector3Int point = Vector3Int.CeilToInt(wallPosition);

            if (wallList.Contains(point))
            {
                doorList.Add(point);
                wallList.Remove(point);
            }
            else
            {
                wallList.Add(point);
            }
        }
    }
}
