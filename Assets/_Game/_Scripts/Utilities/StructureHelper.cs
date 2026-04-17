using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Backrooms
{
    public static class StructureHelper
    {
        public static List<Node> TraverseGraph(Node parentNode)
        {
            Queue<Node> nodesQueue = new Queue<Node>();
            List<Node> listToReturn = new List<Node>();

            if (parentNode.ChildrenNodes.Count == 0)
                return new List<Node>() { parentNode };

            foreach(Node childNode in parentNode.ChildrenNodes)
                nodesQueue.Enqueue(childNode);

            while(nodesQueue.Count > 0)
            {
                Node curNode = nodesQueue.Dequeue();

                if (curNode.ChildrenNodes.Count == 0)
                {
                    listToReturn.Add(curNode);
                }
                else
                {
                    foreach(Node childNode in curNode.ChildrenNodes)
                        nodesQueue.Enqueue(childNode);
                }
            }

            return listToReturn;
        }

        public static Vector2Int RandomizeBottomLeftAreaCorner(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
        {
            int minX = boundaryLeftPoint.x + offset;
            int maxX = boundaryRightPoint.x - offset;

            int minY = boundaryLeftPoint.y + offset;
            int maxY = boundaryRightPoint.y - offset;

            return new Vector2Int
            (
                Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)), 
                Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier))
            );
        }

        public static Vector2Int RandomizeTopRightAreaCorner(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
        {
            int minX = boundaryLeftPoint.x + offset;
            int maxX = boundaryRightPoint.x - offset;

            int minY = boundaryLeftPoint.y + offset;
            int maxY = boundaryRightPoint.y - offset;

            return new Vector2Int
            (
                Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX), 
                Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY)
            );
        }

        public static Vector2Int CalculateMiddlePoint(Vector2Int point1, Vector2Int point2)
        {
            Vector2 sum = point1 + point2;
            Vector2 middlePoint = sum / 2;
            
            return new Vector2Int((int)middlePoint.x, (int)middlePoint.y);
        }
    }
}
