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

        // Not serialized
        private SpaceGenerator _generator;

        private void Awake()
        {
            _generator = new SpaceGenerator(_totalWidth, _totalLength);
        }

        private void Start()
        {
            CreateBackrooms();
        }

        private void CreateBackrooms()
        {
            List<Node> roomsList = _generator.CalculateRooms(_maxIterations, _roomMinWidth, _roomMinLength);
        }
    }
}
