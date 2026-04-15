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
        [SerializeField] private int _corridorWidth;

        [Header("Creation")]
        [SerializeField] private int _maxIterations;

        [Header("References")]
        [SerializeField] private Material _floorMaterial;

        // Not serialized
        private SpaceGenerator _generator;

        private void Awake() => _generator = new SpaceGenerator(_totalWidth, _totalLength);

        private void Start() => CreateSpace();

        private void CreateSpace()
        {
            List<Node> roomsList = _generator.CalculateRooms(_maxIterations, _roomMinWidth, _roomMinLength);

            foreach (Node room in roomsList)
               MeshSpawner.CreateMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner, "Mesh_Floor_", _floorMaterial);
        }
    }
}
