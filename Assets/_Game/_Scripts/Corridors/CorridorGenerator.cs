using System;
using System.Collections.Generic;
using System.Linq;

namespace Backrooms
{
    public class CorridorGenerator
    {
        public List<Node> CreateCorridors(List<RoomNode> totalNodes, int corridorWidth)
        {
            List<Node> listToReturn = new List<Node>();
            Queue<RoomNode> structuresToCheckQueue = new Queue<RoomNode>(totalNodes.OrderByDescending( node => node.TreeLayerIndex).ToList());
        
            while(structuresToCheckQueue.Count > 0)
            {
                Node node = structuresToCheckQueue.Dequeue();

                if (node.ChildrenNodes.Count == 0) continue;

                CorridorNode corridorNode = new CorridorNode(node.ChildrenNodes[0], node.ChildrenNodes[1], corridorWidth);
                listToReturn.Add(corridorNode);
            }

            return listToReturn;
        }
    }
}
