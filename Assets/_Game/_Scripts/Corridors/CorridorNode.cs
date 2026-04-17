using UnityEngine;

namespace Backrooms
{
    public class CorridorNode : Node
    {
        private Node _node1;
        private Node _node2;
        private int _corridorWidth;

        public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
        {
            this._node1 = node1;
            this._node2 = node2;
            this._corridorWidth = corridorWidth;

            GenerateCorridor();
        }

        private void GenerateCorridor()
        {
            RelativePositionType relativePositionNode2 = CheckRelativePositionAtNode2();
            
            switch(relativePositionNode2)
            {
                case RelativePositionType.Up:
                    ProcessRoomOnVertical(this._node1, this._node2);
                    break;

                case RelativePositionType.Down:
                    ProcessRoomOnVertical(this._node2, this._node1);
                    break;

                case RelativePositionType.Right:
                    ProcessRoomOnHorizontal(this._node1, this._node2);
                    break;

                case RelativePositionType.Left:
                    ProcessRoomOnHorizontal(this._node2, this._node1);
                    break;
            }
        }

        private void ProcessRoomOnHorizontal(Node node1, Node node2)
        {
            
        }

        private void ProcessRoomOnVertical(Node node1, Node node2)
        {
            
        }

        private RelativePositionType CheckRelativePositionAtNode2()
        {
            Vector2 middlePointNode1 = ((Vector2)_node1.TopRightAreaCorner + _node1.BottomLeftAreaCorner) / 2;
            Vector2 middlePointNode2 = ((Vector2)_node2.TopRightAreaCorner + _node2.BottomLeftAreaCorner) / 2;

            float relativeAngle = CalculateAngleBetweenNodes(middlePointNode1, middlePointNode2);

            RelativePositionType relativePosition;

            if (relativeAngle < 45 && relativeAngle >= 0 || relativeAngle > -45 && relativeAngle < 0)
                relativePosition = RelativePositionType.Right;
            else if (relativeAngle > 45 && relativeAngle < 135)
                relativePosition = RelativePositionType.Up;
            else if (relativeAngle > -135 && relativeAngle < -45)
                relativePosition = RelativePositionType.Down;
            else
                relativePosition = RelativePositionType.Left;
            
            return relativePosition;
        }

        private float CalculateAngleBetweenNodes(Vector2 middlePointNode1, Vector2 middlePointNode2)
        {
            return Mathf.Atan2(middlePointNode2.y - middlePointNode1.y, middlePointNode2.x - middlePointNode1.x) * Mathf.Rad2Deg;
        }
    }
}
