using UnityEngine;

namespace Backrooms
{
    public class RoomNode : Node
    {
        // Getters
        public int Width { get { return TopRightAreaCorner.x - BottomLeftAreaCorner.x; } }
        public int Length { get { return TopRightAreaCorner.y - BottomLeftAreaCorner.y; } }

        public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, 
                            Node parentNode, int treeLayerIndex) : base(parentNode)
        {
            this.BottomLeftAreaCorner = bottomLeftAreaCorner;
            this.TopRightAreaCorner = topRightAreaCorner;
            this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
            this.TopLeftAreaCorner = new Vector2Int(BottomLeftAreaCorner.x, TopRightAreaCorner.y);
            this.TreeLayerIndex = treeLayerIndex;
        }
    }
}
