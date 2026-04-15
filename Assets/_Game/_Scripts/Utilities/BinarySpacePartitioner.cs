using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms
{
    public class BinarySpacePartitioner
    {
        private RoomNode _rootNode;

        // Getters
        public RoomNode RootNode { get { return _rootNode; } }

        public BinarySpacePartitioner(int totalWidth, int totalLength)
        {
            Vector2Int bottomLeftAreaCorner = new Vector2Int(0, 0);
            Vector2Int topRightAreaCorner = new Vector2Int(totalWidth, totalLength);

            this._rootNode = new RoomNode(bottomLeftAreaCorner, topRightAreaCorner, null, 0);
        }

        public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomMinWidth, int roomMinLength)
        {
            Queue<RoomNode> graphQueue = new Queue<RoomNode>();
            List<RoomNode> listToReturn = new List<RoomNode>();

            graphQueue.Enqueue(this._rootNode);
            listToReturn.Add(this._rootNode);

            int curIteration = 0;
            while (curIteration < maxIterations && graphQueue.Count > 0)
            {
                curIteration++;
                RoomNode curNode = graphQueue.Dequeue();

                if (curNode.Width >= roomMinWidth * 2 || curNode.Length >= roomMinLength * 2)
                    SplitSpace(curNode, listToReturn, roomMinWidth, roomMinLength, graphQueue);
            }

            return listToReturn;
        }

        private void SplitSpace(RoomNode curNode, List<RoomNode> listToReturn, int roomMinWidth, int roomMinLength, Queue<RoomNode> graphQueue)
        {
            Line line = GetLineDividingSpace
            (
                curNode.BottomLeftAreaCorner,
                curNode.TopRightAreaCorner,
                roomMinWidth,
                roomMinLength
            );

            RoomNode node1;
            RoomNode node2;

            if (line.Orientation == OrientationType.Horizontal)
            {
                node1 = new RoomNode
                (
                    curNode.BottomLeftAreaCorner,
                    new Vector2Int(curNode.TopRightAreaCorner.x, line.Coordinates.y),
                    curNode,
                    curNode.TreeLayerIndex + 1
                );

                node2 = new RoomNode
                (
                    new Vector2Int(curNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                    curNode.TopRightAreaCorner,
                    curNode,
                    curNode.TreeLayerIndex + 1
                );
            }
            else // Vertical
            {
                node1 = new RoomNode
                (
                    curNode.BottomLeftAreaCorner,
                    new Vector2Int(line.Coordinates.x, curNode.TopRightAreaCorner.y),
                    curNode,
                    curNode.TreeLayerIndex + 1
                );

                node2 = new RoomNode
                (
                    new Vector2Int(line.Coordinates.x, curNode.BottomLeftAreaCorner.y),
                    curNode.TopRightAreaCorner,
                    curNode,
                    curNode.TreeLayerIndex + 1
                );
            }

            AddNewNodes(listToReturn, graphQueue, node1);
            AddNewNodes(listToReturn, graphQueue, node2);
        }

        private void AddNewNodes(List<RoomNode> listToReturn, Queue<RoomNode> graphQueue, RoomNode node)
        {
            listToReturn.Add(node);
            graphQueue.Enqueue(node);
        }

        private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomMinWidth, int roomMinLength)
        {
            OrientationType orientation;

            bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= roomMinWidth * 2;
            bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= roomMinLength * 2;

            if (widthStatus && lengthStatus)
                orientation = (OrientationType)Random.Range(0, 2);
            else if (widthStatus)
                orientation = OrientationType.Vertical;
            else
                orientation = OrientationType.Horizontal;

            return new Line(orientation, GetCoordinatesByOrientation(orientation, bottomLeftAreaCorner, topRightAreaCorner, roomMinWidth, roomMinLength));
        }

        private Vector2Int GetCoordinatesByOrientation(OrientationType orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomMinWidth, int roomMinLength)
        {
            Vector2Int coordinates;

            if (orientation == OrientationType.Horizontal)
                coordinates = new Vector2Int(0, Random.Range((bottomLeftAreaCorner.y + roomMinLength), (topRightAreaCorner.y - roomMinLength)));
            else
                coordinates = new Vector2Int(Random.Range((bottomLeftAreaCorner.x + roomMinWidth), (topRightAreaCorner.x - roomMinWidth)), 0);

            return coordinates;
        }
    }
}
