using UnityEngine;

namespace Backrooms
{
    public class Line
    {
        private OrientationType _orientation;
        private Vector2Int _coordinates;

        // Getters
        public Vector2Int Coordinates { get { return _coordinates; } }
        public OrientationType Orientation { get { return _orientation; } }

        public Line(OrientationType orientation, Vector2Int coordinates)
        {
            this._orientation = orientation;
            this._coordinates = coordinates;
        }
    }
}
