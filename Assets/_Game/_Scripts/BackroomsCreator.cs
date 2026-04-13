using UnityEngine;

namespace Backrooms
{
    public class BackroomsCreator : MonoBehaviour
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
        private BackroomsGenerator _generator;

        private void Awake()
        {
            _generator = new BackroomsGenerator(_totalWidth, _totalLength);
        }

        private void Start()
        {
            CreateBackrooms();
        }

        private void CreateBackrooms()
        {
            var roomsList = _generator.CalculateRooms(_maxIterations, _roomMinWidth, _roomMinLength);
        }
    }
}
