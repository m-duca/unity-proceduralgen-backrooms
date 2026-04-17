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

        public List<Node> CalculateSpace(int maxIterations, int roomMinWidth, int roomMinLength, float roomBottomLeftModifier, float roomTopRightModifier, int roomOffset, int corridorWidth)
        {
            BinarySpacePartitioner bsp = new BinarySpacePartitioner(_totalWidth, _totalLength);
            _totalNodes = bsp.PrepareNodesCollection(maxIterations, roomMinWidth, roomMinLength);
            
            List<Node> roomSpaces = StructureHelper.TraverseGraph(bsp.RootNode);
            RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomMinWidth, roomMinLength);
            List<RoomNode> roomNodes  = roomGenerator.GenerateRoomsBySpaces(roomSpaces, roomBottomLeftModifier, roomTopRightModifier, roomOffset);

            CorridorGenerator corridorGenerator = new CorridorGenerator();
            List<Node> corridorsNode = corridorGenerator.CreateCorridors(_totalNodes, corridorWidth);

            return new List<Node>( roomNodes);
        }
    }
}
