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

        public List<RoomNode> GenerateRoomsBySpaces(List<Node> roomSpaces, float roomBottomLeftModifier, float roomTopRightModifier, int roomOffset)
        {
            List<RoomNode> listToReturn = new List<RoomNode>();

            foreach (Node spaceNode in roomSpaces)
            {
                Vector2Int newBottomLeftAreaCorner = StructureHelper.RandomizeBottomLeftAreaCorner(spaceNode.BottomLeftAreaCorner, spaceNode.TopRightAreaCorner, roomBottomLeftModifier, roomOffset);
                Vector2Int newTopRightAreaCorner = StructureHelper.RandomizeTopRightAreaCorner(spaceNode.BottomLeftAreaCorner, spaceNode.TopRightAreaCorner, roomTopRightModifier, roomOffset);
                
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
