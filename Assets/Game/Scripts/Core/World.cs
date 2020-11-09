using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelite.Core
{
    public class World : MonoBehaviour
    {
        [Header("World Size")]
        [SerializeField] private int _width = 16;
        [SerializeField] private int _depth = 9;


        [Header("Prefabs")]
        [SerializeField] private WorldTile _tilePrefab;
        [SerializeField] private GameObject hexPrefab;

        readonly private float xOffset = .8660254f;

        public float XOffset { get { return xOffset; } }

        private readonly List<WorldTile> _tiles = new List<WorldTile>();
        private readonly List<HexTile> hexTiles = new List<HexTile>();

        private void Awake()
        {
            //GenerateWorld();
            GenerateMap();
        }

        public void GenerateMap()
        {
            for (var column = 0; column < _width; column++)
            {
                for (var row = 0; row < _depth; row++)
                {
                    var hex = new HexTile(column, row);

                    var hexTile = Instantiate(hexPrefab, hex.Position, Quaternion.identity, this.transform);


                    var tile = hexTile.GetComponent<WorldTile>();
                    SetTileName(hex, tile);

                    _tiles.Add(tile);

                    tile.X = column;
                    tile.Z = row;
                }
            }
        }

        //Currently Unused
        private void DestroyWorld()
        {
            foreach (WorldTile tile in _tiles)
            {
                Destroy(tile);
            }
        }

        public WorldTile GetTileAt(int x, int z)
        {
            return _tiles.SingleOrDefault(t =>
            {
                var tile = t.GetComponent<WorldTile>();
                return tile.X == x && tile.Z == z;
            });
        }

        private static void SetTileName(HexTile hex, WorldTile tile)
        {
            tile.name = hex.ToString();
            tile.xCoordinate.text = $"{hex.Q}";
            tile.yCoordinate.text = $"{hex.R}";
            tile.zCoordinate.text = $"{hex.S}";
        }
    }
}
