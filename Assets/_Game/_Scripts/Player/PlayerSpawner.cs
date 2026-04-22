using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public class PlayerSpawner
    {
        public void SpawnPlayer(List<Node> roomsList, GameObject playerPrefab)
        {
            if (roomsList == null || roomsList.Count == 0)
                return;

            Node spawnRoom = roomsList[Random.Range(0, roomsList.Count)];

            Vector2 center = (spawnRoom.BottomLeftAreaCorner + spawnRoom.TopRightAreaCorner) / 2;

            Vector3 spawnPosition = new Vector3(center.x, 1f, center.y);

            Object.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
