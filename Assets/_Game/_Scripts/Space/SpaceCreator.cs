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

        // Not serialized
        private SpaceGenerator _generator;

        private void Awake() => _generator = new SpaceGenerator(_totalWidth, _totalLength);

        private void Start() => CreateSpace();

        private void CreateSpace()
        {
            List<Node> roomsList = _generator.CalculateRooms(_maxIterations, _roomMinWidth, _roomMinLength, _roomBottomLeftModifier, _roomTopRightModifier, _roomOffset);

            foreach (Node room in roomsList)
               MeshSpawner.CreateMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner, "Mesh_Floor_", _floorMaterial);
        }
    }
}
