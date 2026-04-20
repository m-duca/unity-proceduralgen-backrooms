using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class PillarCreator
    {
        private HashSet<Vector2Int> _spawnedPillars = new HashSet<Vector2Int>();

        public void CreatePillars(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, GameObject[] pillarPrefabs,
                    Transform parent, int spawnChance, int offset, float minDistanceBetweenPillars)
        {
            for (int x = bottomLeftAreaCorner.x + offset; x < topRightAreaCorner.x - offset; x++)
            {
                for (int y = bottomLeftAreaCorner.y + offset; y < topRightAreaCorner.y - offset; y++)
                {
                    Vector2Int currentPos = new Vector2Int(x, y);

                    if (Random.Range(0, 100) > spawnChance)
                        continue;

                    if (IsNearOtherPillar(currentPos, minDistanceBetweenPillars))
                        continue;

                    Vector3 worldPos = new Vector3(x, 0, y);

                    GameObject.Instantiate(pillarPrefabs[Random.Range(0, pillarPrefabs.Length)], worldPos, Quaternion.identity, parent);

                    _spawnedPillars.Add(currentPos);
                }
            }
        }

        private bool IsNearOtherPillar(Vector2Int pos, float minDistanceBetweenPillars)
        {
            foreach (var pillar in _spawnedPillars)
            {
                if (Vector2Int.Distance(pos, pillar) < minDistanceBetweenPillars)
                    return true;
            }

            return false;
        }
    }
}
