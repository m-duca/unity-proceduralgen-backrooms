using System.Collections.Generic;

namespace Backrooms
{
    public class BackroomsGenerator
    {
        private int _totalWidth;
        private int _totalLength;

        private List<RoomNode> _totalNodes = new();

        public BackroomsGenerator(int totalWidth, int totalLength)
        {
            this._totalWidth = totalWidth;
            this._totalLength = totalLength;
        }

        public List<Node> CalculateRooms(int maxIterations, int roomMinWidth, int roomMinLength)
        {
            BinarySpacePartitioner bsp = new BinarySpacePartitioner(_totalWidth, _totalLength);
            _totalNodes = bsp.PrepareNodesCollection(maxIterations, roomMinWidth, roomMinLength);
            List<Node> roomSpaces = StructureHelper.TraverseGraph(bsp.RootNode);
            return new List<Node>( _totalNodes);
        }
    }
}
