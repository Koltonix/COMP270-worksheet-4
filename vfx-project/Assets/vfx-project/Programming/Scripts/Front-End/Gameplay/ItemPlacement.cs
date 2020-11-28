//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
using UnityEngine;

namespace VFX.Gameplay
{
    /// <summary>
    /// Initialises and holds all of the tile information which can be used for
    /// the gameplay logic as shown here where I have made it hold a gameobject
    /// that I have randomly spawned.
    /// 
    /// You could also used this tile system for really any game or mechanic
    /// such as A* pathfinding as it is in a grid format.
    /// </summary>
    public class ItemPlacement : MonoBehaviour
    {
        [Header("Input")]
        private RaycastHit hit;

        [Header("Data")]
        private Tile[,] tiles = null;
        private Vector2 tileSize = Vector2.one;

        [Header("Spawn Object")]
        private GameObject parent = null;
        [SerializeField]
        private GameObject[] prefabs;
        [SerializeField]
        private float minScale = 0.25f;
        [SerializeField]
        private float maxScale = 1.0f;

        /// <summary>
        /// Spawns a random prefab from the list and then assigns it to the
        /// Tile that it was placed on. It also destroys it and removes it
        /// if it is already there.
        /// </summary>
        public void SpawnRandomPrefab()
        {
            if (hit.collider != null && tiles != null)
            {
                Tile tile = PositionToTile(hit.point);
                if (tile == null) return;

                // Didn't want it to create unless it spawns a tree
                if (!parent)
                    parent = new GameObject("Spawned Objects");

                if (!tile.IsOccupied())
                {
                    GameObject tree = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);

                    tree.transform.position = tile.position;
                    tree.transform.localScale *= Random.Range(minScale, maxScale);
                    tree.transform.SetParent(parent.transform);

                    tile.SetHeldObject(tree);
                }

                else
                { 
                    GameObject go = tile.RemoveHeldObject();
                    Destroy(go);
                }
            }
        }

        /// <summary>
        /// Converts the world space position to the grid space to get the Tile.
        /// </summary>
        /// <param name="position">Position closest to the tile.</param>
        /// <returns>The tile closest to the position provided.</returns>
        public Tile PositionToTile(Vector3 position)
        {
            Vector3 totalSize = GetUpperBound() * 2;
            Vector3 minBound = GetLowerBound();

            float xPercent = (position.x - minBound.x) / totalSize.x;
            float yPercent = (position.z - minBound.z) / totalSize.z;

            int xIndex = Mathf.RoundToInt(xPercent * totalSize.x - (tileSize.x * .5f));
            int yIndex = Mathf.RoundToInt((yPercent * totalSize.z) - (tileSize.y * .5f));

            // If it is out of range of the array
            // I prefer this over clamping it since if you don't click within
            // the bounds then I don't want you to be able to access the tile
            if (xIndex < 0 || yIndex < 0 || xIndex >= tiles.GetLength(0) || yIndex >= tiles.GetLength(1))
                return null;

            return tiles[xIndex, yIndex];
        }

        /// <summary>Gets the smallest bound of the whole grid.</summary>
        /// <returns>The lower bound of the whole grid.</returns>
        private Vector3 GetLowerBound()
        {
            Vector3 minTilePos = tiles[0, 0].position;
            minTilePos -= new Vector3(tileSize.x, 0.0f, tileSize.y) * .5f;

            return minTilePos;
        }

        /// <summary>Gets the largest bound of the whole grid.</summary>
        /// <returns>The upper bound of the whole grid.</returns>
        private Vector3 GetUpperBound()
        {
            Vector3 maxTilePos = tiles[tiles.GetLength(0) - 1, tiles.GetLength(1) - 1].position;
            maxTilePos += new Vector3(tileSize.x, 0.0f, tileSize.y) * .5f;

            return maxTilePos;
        }

        #region Event Listeners
        // All of these are used to assign variables through the event system
        // to ensure that no dependencies exist in this class
        public void OnMouseHover(RaycastHit hit) => this.hit = hit;
        public void SetAllTilePositions(Vector3[,] tilePositions) => AssignPositionsToTiles(tilePositions);
        public void SetTileSize(Vector2 tileSize) => this.tileSize = tileSize;
        #endregion

        /// <summary>Creates a 2D array of tiles from the positions provided.</summary>
        /// <param name="positions">A 2D array of positions in the world.</param>
        private void AssignPositionsToTiles(Vector3[,] positions)
        {
            int width = positions.GetLength(0);
            int height = positions.GetLength(1);

            tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = new Tile(positions[x, y]);
                }
            }
        }
    }
}