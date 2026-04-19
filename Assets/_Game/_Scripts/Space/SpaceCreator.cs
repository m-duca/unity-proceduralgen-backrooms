using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class SpaceCreator : MonoBehaviour
    {
        // Inspector
        [Header("Total Space")]
        [SerializeField] private int _totalWidth;
        [SerializeField] private int _totalLength;
        [SerializeField] private int _maxIterations;

        [Header("Single Room")]
        [SerializeField] private int _roomMinWidth;
        [SerializeField] private int _roomMinLength;
        [SerializeField, Range(0.0f, 0.3f)] private float _roomBottomLeftModifier;
        [SerializeField, Range(0.7f, 1f)] private float _roomTopRightModifier;
        [SerializeField, Range(0, 2)] private int _roomOffset;

        [Header("Single Corridor")]
        [SerializeField] private int _corridorWidth;

        [Header("References")]
        [SerializeField] private Material _floorMaterial;
        [SerializeField] private GameObject _wallHorizontal;
        [SerializeField] private GameObject _wallVertical;

        // Not serialized
        private SpaceGenerator _generator;

        private void Awake() => _generator = new SpaceGenerator(_totalWidth, _totalLength);

        private void Start() => CreateSpace();
        public void CreateSpace()
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

            GameObject floorParent = new GameObject("FloorParent");
            floorParent.transform.SetParent(gameObject.transform);

            GameObject wallParent = new GameObject("WallParent");
            wallParent.transform.SetParent(gameObject.transform);

            FloorCreator floorCreator = new FloorCreator();
            WallCreator wallCreator = new WallCreator();

            foreach (Node room in roomsList)
            {
                floorCreator.CreateFloor(
                    room.BottomLeftAreaCorner,
                    room.TopRightAreaCorner,
                    _floorMaterial,
                    floorParent.transform
                );

                wallCreator.CalculateWallPositions(
                    room.BottomLeftAreaCorner,
                    room.TopRightAreaCorner
                );
            }

            wallCreator.InstantiateWalls(
                _wallHorizontal,
                _wallVertical,
                wallParent.transform
            );
        }
    }
}
