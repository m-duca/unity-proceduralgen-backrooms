using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms
{
    public class CorridorNode : Node
    {
        private Node _node1;
        private Node _node2;
        private int _corridorWidth;
        private int _modifierDistanceFromWall = 1;

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
            Node leftNode = null;
            List<Node> leftNodeChildren = StructureHelper.TraverseGraph(_node1);

            Node rightNode = null;
            List<Node> rightNodeChildren = StructureHelper.TraverseGraph(_node2);

            List<Node> sortedLeftNodes = leftNodeChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
            if (sortedLeftNodes.Count == 1)
                leftNode = sortedLeftNodes[0];
            else
            {
                int maxX = sortedLeftNodes[0].TopRightAreaCorner.x;
                sortedLeftNodes = sortedLeftNodes.Where(children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();
                int index = Random.Range(0, sortedLeftNodes.Count);

                leftNode = sortedLeftNodes[index];
            }

            List<Node> possibleNeighboursInRightNode = rightNodeChildren.Where(
                child => GetValidYForNeighbourHorizontal(
                    leftNode.TopRightAreaCorner, 
                    leftNode.BottomRightAreaCorner, 
                    child.TopLeftAreaCorner, 
                    child.BottomLeftAreaCorner) != -1
                ).ToList();
            
            if (possibleNeighboursInRightNode.Count == 0)
                rightNode = node2;
            else
                rightNode = possibleNeighboursInRightNode[0];

            int y = GetValidYForNeighbourHorizontal(leftNode.TopLeftAreaCorner, leftNode.BottomRightAreaCorner,
            rightNode.TopLeftAreaCorner, rightNode.BottomLeftAreaCorner);
            
            while (y == -1 && sortedLeftNodes.Count > 1)
            {
                sortedLeftNodes = sortedLeftNodes.Where(child => child.TopLeftAreaCorner.y != leftNode.TopLeftAreaCorner.y).ToList();

                leftNode = sortedLeftNodes[0];

                y = GetValidYForNeighbourHorizontal(leftNode.TopLeftAreaCorner, leftNode.BottomRightAreaCorner,
            rightNode.TopLeftAreaCorner, rightNode.BottomLeftAreaCorner);
            }

            this.BottomLeftAreaCorner = new Vector2Int(leftNode.BottomRightAreaCorner.x, y);
            this.TopRightAreaCorner = new Vector2Int(rightNode.TopLeftAreaCorner.x, y + this._corridorWidth);
        }

        private int GetValidYForNeighbourHorizontal(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
        {
            if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
            {
                return StructureHelper.CalculateMiddlePoint
                (
                    leftNodeDown + new Vector2Int(0, this._modifierDistanceFromWall), 
                    leftNodeUp - new Vector2Int(0,  this._modifierDistanceFromWall + this._corridorWidth)
                ).y;
            }

            if(rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
            {
                return StructureHelper.CalculateMiddlePoint
                (
                    rightNodeDown + new Vector2Int(0, this._modifierDistanceFromWall),
                    rightNodeUp - new Vector2Int(0, this._modifierDistanceFromWall + this._corridorWidth)
                ).y;
            }

            if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeDown.y)
            {
                return StructureHelper.CalculateMiddlePoint
                (
                    rightNodeDown + new Vector2Int(0, this._modifierDistanceFromWall),
                    leftNodeUp - new Vector2Int(0, this._modifierDistanceFromWall)
                ).y;
            }

            if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
            {
                return StructureHelper.CalculateMiddlePoint
                (
                    leftNodeDown + new Vector2Int(0, this._modifierDistanceFromWall),
                    rightNodeUp - new Vector2Int(0, this._modifierDistanceFromWall + this._corridorWidth)
                ).y;
            }

            return -1;
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
