using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class RoomGenerator
    {
        private int _maxIterations;
        private int _roomMinWidth;
        private int _roomMinLength;

        public RoomGenerator(int maxIterations, int roomMinWidth, int roomMinLength)
        {
            this._maxIterations = maxIterations;
            this._roomMinWidth = roomMinWidth;
            this._roomMinLength = roomMinLength;
        }

        public List<RoomNode> GenerateRoomsBySpaces(List<Node> roomSpaces)
        {
            List<RoomNode> listToReturn = new List<RoomNode>();

            float bottomLeftModifier = 0.1f;
            float topRightModifier = 0.9f;

            int bottomLeftOffset = 1;
            int topRightOffset = 1;

            foreach (Node spaceNode in roomSpaces)
            {
                Vector2Int newBottomLeftAreaCorner = StructureHelper.RandomizeBottomLeftAreaCorner(spaceNode.BottomLeftAreaCorner, spaceNode.TopRightAreaCorner, bottomLeftModifier, bottomLeftOffset);
                Vector2Int newTopRightAreaCorner = StructureHelper.RandomizeTopRightAreaCorner(spaceNode.BottomLeftAreaCorner, spaceNode.TopRightAreaCorner, topRightModifier, topRightOffset);
                
                // Assigning new corners
                spaceNode.BottomLeftAreaCorner = newBottomLeftAreaCorner;
                spaceNode.TopRightAreaCorner = newTopRightAreaCorner;
                spaceNode.BottomRightAreaCorner = new Vector2Int(newTopRightAreaCorner.x, newBottomLeftAreaCorner.y);
                spaceNode.TopLeftAreaCorner = new Vector2Int(newBottomLeftAreaCorner.x, newTopRightAreaCorner.y);
                
                listToReturn.Add((RoomNode)spaceNode);
            }

            return listToReturn;
        }
    }
}
