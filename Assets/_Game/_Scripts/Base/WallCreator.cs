using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class WallCreator
    {
        private List<Vector3Int> _possibleDoorHorizontalPosition = new List<Vector3Int>();
        private List<Vector3Int> _possibleDoorVerticalPosition = new List<Vector3Int>();

        private List<Vector3Int> _possibleWallHorizontalPosition = new List<Vector3Int>();
        private List<Vector3Int> _possibleWallVerticalPosition = new List<Vector3Int>();

        public void CalculateWallPositions(Vector2 bottomLeftAreaCorner, Vector2 topRightAreaCorner)
        {
            Vector3 bottomLeftPoint = new Vector3(bottomLeftAreaCorner.x, 0, bottomLeftAreaCorner.y);
            Vector3 topRightPoint = new Vector3(topRightAreaCorner.x, 0, topRightAreaCorner.y);
            Vector3 bottomRightPoint = new Vector3(topRightAreaCorner.x, 0, bottomLeftAreaCorner.y);
            Vector3 topLeftPoint = new Vector3(bottomLeftAreaCorner.x, 0, topRightAreaCorner.y);

            for (int row = (int)bottomLeftPoint.x; row < (int)bottomRightPoint.x; row++)
            {
                Vector3 wallPosition = new Vector3(row, 0, bottomLeftPoint.z);
                AddWallPositionToList(wallPosition, _possibleWallHorizontalPosition, _possibleDoorHorizontalPosition);
            }

            for (int row = (int)topLeftPoint.x; row < (int)topRightPoint.x; row++)
            {
                Vector3 wallPosition = new Vector3(row, 0, topRightPoint.z);
                AddWallPositionToList(wallPosition, _possibleWallHorizontalPosition, _possibleDoorHorizontalPosition);
            }

            for (int col = (int)bottomLeftPoint.z; col < (int)topLeftPoint.z; col++)
            {
                Vector3 wallPosition = new Vector3(bottomLeftPoint.x, 0, col);
                AddWallPositionToList(wallPosition, _possibleWallVerticalPosition, _possibleDoorVerticalPosition);
            }

            for (int col = (int)bottomRightPoint.z; col < (int)topRightPoint.z; col++)
            {
                Vector3 wallPosition = new Vector3(bottomRightPoint.x, 0, col);
                AddWallPositionToList(wallPosition, _possibleWallVerticalPosition, _possibleDoorVerticalPosition);
            }
        }

        public void InstantiateWalls(GameObject wallHorizontalPrefab, GameObject wallVerticalPrefab, Transform parent)
        {
            foreach (Vector3Int wallPosition in _possibleWallHorizontalPosition)
                GameObject.Instantiate(wallHorizontalPrefab, wallPosition, Quaternion.identity, parent);

            foreach (Vector3Int wallPosition in _possibleWallVerticalPosition)
                GameObject.Instantiate(wallVerticalPrefab, wallPosition, Quaternion.identity, parent);
        }

        private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
        {
            Vector3Int point = Vector3Int.CeilToInt(wallPosition);

            if (wallList.Contains(point))
            {
                doorList.Add(point);
                wallList.Remove(point);
            }
            else
            {
                wallList.Add(point);
            }
        }
    }
}
