using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public static class StructureHelper
    {
        public static List<Node> TraverseGraph(RoomNode parentNode)
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
    }
}
