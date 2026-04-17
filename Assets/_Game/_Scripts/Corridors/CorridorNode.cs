using System;
using UnityEngine;

namespace Backrooms
{
    public class CorridorNode : Node
    {
        private Node _structure1;
        private Node _structure2;
        private int _corridorWidth;

        public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
        {
            this._structure1 = node1;
            this._structure2 = node2;
            this._corridorWidth = corridorWidth;

            GenerateCorridor();
        }

        private void GenerateCorridor()
        {
            RelativePositionType relativePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
            
            switch(relativePositionOfStructure2)
            {
                case RelativePositionType.Up:
                    ProcessRoomOnVertical(this._structure1, this._structure2);
                    break;

                case RelativePositionType.Down:
                    ProcessRoomOnVertical(this._structure2, this._structure1);
                    break;

                case RelativePositionType.Right:
                    ProcessRoomOnHorizontal(this._structure1, this._structure2);
                    break;

                case RelativePositionType.Left:
                    ProcessRoomOnHorizontal(this._structure2, this._structure1);
                    break;
            }
        }

        private void ProcessRoomOnHorizontal(Node structure1, Node structure2)
        {
            
        }

        private void ProcessRoomOnVertical(Node structure1, Node structure2)
        {
            
        }

        private RelativePositionType CheckPositionStructure2AgainstStructure1()
        {
            Vector2 middlePointStructure1 = ((Vector2)_structure1.TopRightAreaCorner + _structure1.BottomLeftAreaCorner) / 2;
            Vector2 middlePointStructure2 = ((Vector2)_structure2.TopRightAreaCorner + _structure2.BottomLeftAreaCorner) / 2;

            float relativeAngle = CalculateAngleBetweenStructures(middlePointStructure1, middlePointStructure2);

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

        private float CalculateAngleBetweenStructures(Vector2 middlePointStructure1, Vector2 middlePointStructure2)
        {
            return Mathf.Atan2(middlePointStructure2.y - middlePointStructure1.y, middlePointStructure2.x - middlePointStructure1.x) * Mathf.Rad2Deg;
        }
    }
}
