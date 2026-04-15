using System.Collections.Generic;

namespace Backrooms
{
    public class SpaceGenerator
    {
        private int _totalWidth;
        private int _totalLength;
        private List<RoomNode> _totalNodes = new();

        public SpaceGenerator(int totalWidth, int totalLength)
        {
            this._totalWidth = totalWidth;
            this._totalLength = totalLength;
        }

        public List<Node> CalculateRooms(int maxIterations, int roomMinWidth, int roomMinLength, float roomBottomLeftModifier, float roomTopRightModifier, int roomOffset)
        {
            BinarySpacePartitioner bsp = new BinarySpacePartitioner(_totalWidth, _totalLength);
            _totalNodes = bsp.PrepareNodesCollection(maxIterations, roomMinWidth, roomMinLength);
            
            List<Node> roomSpaces = StructureHelper.TraverseGraph(bsp.RootNode);
            RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomMinWidth, roomMinLength);
            List<RoomNode> roomNodes  = roomGenerator.GenerateRoomsBySpaces(roomSpaces, roomBottomLeftModifier, roomTopRightModifier, roomOffset);

            return new List<Node>( roomNodes);
        }
    }
}
