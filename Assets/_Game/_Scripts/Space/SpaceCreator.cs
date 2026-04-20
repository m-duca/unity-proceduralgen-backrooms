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

        [Header("Rooms")]
        [SerializeField] private int _roomMinWidth;
        [SerializeField] private int _roomMinLength;
        [SerializeField, Range(0.0f, 0.3f)] private float _roomBottomLeftModifier;
        [SerializeField, Range(0.7f, 1f)] private float _roomTopRightModifier;
        [SerializeField, Range(0, 2)] private int _roomOffset;

        [Header("Corridors")]
        [SerializeField] private int _corridorWidth;

        [Header("Ceilings")]
        [SerializeField] private float _ceilingHeight;

        [Header("Pillars")]
        [SerializeField] private GameObject _pillarPrefab;
        [SerializeField, Range(0, 100)] private int _pillarSpawnChance;
        [SerializeField] private int _pillarOffset;

        [Header("References")]
        [SerializeField] private Material _floorMaterial;
        [SerializeField] private Material _ceilingMaterial;
        [SerializeField] private GameObject _wallHorizontalPrefab;
        [SerializeField] private GameObject _wallVerticalPrefab;

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

            GameObject ceilingParent = new GameObject("CeilingParent");
            wallParent.transform.SetParent(gameObject.transform);

            GameObject pillarParent = new GameObject("PillarParent");
            pillarParent.transform.SetParent(gameObject.transform);

            FloorCreator floorCreator = new FloorCreator();
            WallCreator wallCreator = new WallCreator();
            CeilingCreator ceilingCreator = new CeilingCreator();
            PillarCreator pillarCreator = new PillarCreator();

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

                ceilingCreator.CreateCeiling(
                    room.BottomLeftAreaCorner,
                room.TopRightAreaCorner,
                _ceilingHeight,
                _ceilingMaterial,
                ceilingParent.transform
                );

                pillarCreator.CreatePillars(
    room.BottomLeftAreaCorner,
    room.TopRightAreaCorner,
        _pillarPrefab,
            pillarParent.transform,
        _pillarSpawnChance,
            _pillarOffset
                );
            }

            wallCreator.InstantiateWalls(
                _wallHorizontalPrefab,
                _wallVerticalPrefab,
                wallParent.transform
            );
        }
    }
}
