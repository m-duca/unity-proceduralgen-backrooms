using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class BackroomsGenerator
    {
        private int _totalWidth;
        private int _totalLength;

        private RoomNode _rootNode;
        private List<RoomNode> _allNodes = new();

        public BackroomsGenerator(int totalWidth, int totalLength)
        {
            this._totalWidth = totalWidth;
            this._totalLength = totalLength;
        }

        internal object CalculateRooms(int maxIterations, int roomMinWidth, int roomMinLength)
        {
            return null;
        }
    }
}
