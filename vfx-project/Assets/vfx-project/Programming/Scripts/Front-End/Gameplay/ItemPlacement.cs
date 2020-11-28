//////////////////////////////////////////////////
// Christopher Robertson 2020.
// https://github.com/Koltonix
// Copyright (c) 2020. All rights reserved.
//////////////////////////////////////////////////
using UnityEngine;

namespace VFX.Gameplay
{
    public class ItemPlacement : MonoBehaviour
    {
        [Header("Input")]
        private RaycastHit hit;

        [Header("Data")]
        private Tile[,] tiles = null;
        private Vector2 tileSize = Vector2.one;

        [Header("Spawn Object")]
        [SerializeField]
        private GameObject[] treePrefabs;
        [SerializeField]
        private float minScale = 0.25f;
        [SerializeField]
        private float maxScale = 1.0f;

        public void OnClick()
        {
            if (hit.collider != null && tiles != null)
            {
                Tile tile = PositionToTile(hit.point);
                if (!tile.IsOccupied())
                {
                    GameObject tree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)]);
                    tree.transform.position = tile.position;
                    tree.transform.localScale *= Random.Range(minScale, maxScale);
                    tile.SetHeldObject(tree);
                }

                else if (tile.IsOccupied())
                { 
                    GameObject go = tile.RemoveHeldObject();
                    Destroy(go);
                }
            }
        }

        public Tile PositionToTile(Vector3 position)
        {
            Vector3 totalSize = GetUpperBound() * 2;
            Vector3 minBound = GetLowerBound();

            float xPercent = (position.x - minBound.x) / totalSize.x;
            float yPercent = (position.z - minBound.z) / totalSize.z;

            int xIndex = Mathf.RoundToInt(xPercent * totalSize.x - (tileSize.x * .5f));
            int yIndex = Mathf.RoundToInt((yPercent * totalSize.z) - (tileSize.y * .5f));

            return tiles[xIndex, yIndex];
        }

        private Vector3 GetLowerBound()
        {
            Vector3 minTilePos = tiles[0, 0].position;
            minTilePos -= new Vector3(tileSize.x, 0.0f, tileSize.y) * .5f;

            return minTilePos;
        }

        private Vector3 GetUpperBound()
        {
            Vector3 maxTilePos = tiles[tiles.GetLength(0) - 1, tiles.GetLength(1) - 1].position;
            maxTilePos += new Vector3(tileSize.x, 0.0f, tileSize.y) * .5f;

            return maxTilePos;
        }

        #region Event Listeners
        public void OnMouseHover(RaycastHit hit) => this.hit = hit;
        public void SetAllTilePositions(Vector3[,] tilePositions) => AssignPositionsToTiles(tilePositions);
        public void SetTileSize(Vector2 tileSize) => this.tileSize = tileSize;

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
        #endregion
    }
}