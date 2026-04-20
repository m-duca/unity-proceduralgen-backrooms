using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class PillarCreator
    {
        private HashSet<Vector2Int> _spawnedPillars = new HashSet<Vector2Int>();

        public void CreatePillars(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, GameObject pillarPrefab,
                    Transform parent, int spawnChance, int offset)
        {
            for (int x = bottomLeftAreaCorner.x + offset; x < topRightAreaCorner.x - offset; x++)
            {
                for (int y = bottomLeftAreaCorner.y + offset; y < topRightAreaCorner.y - offset; y++)
                {
                    Vector2Int currentPos = new Vector2Int(x, y);

                    if (Random.Range(0, 100) > spawnChance)
                        continue;

                    if (IsNearOtherPillar(currentPos))
                        continue;

                    Vector3 worldPos = new Vector3(x, 0, y);

                    GameObject.Instantiate(pillarPrefab, worldPos, Quaternion.identity, parent);

                    _spawnedPillars.Add(currentPos);
                }
            }
        }

        private bool IsNearOtherPillar(Vector2Int pos)
        {
            return _spawnedPillars.Contains(pos + Vector2Int.up) ||
                   _spawnedPillars.Contains(pos + Vector2Int.down) ||
                   _spawnedPillars.Contains(pos + Vector2Int.left) ||
                   _spawnedPillars.Contains(pos + Vector2Int.right);
        }
    }
}
