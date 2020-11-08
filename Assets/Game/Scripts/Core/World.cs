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
        private float zOffset = .76f;

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
                    HexTile hex = new HexTile(column, row);

                    GameObject hexTile = Instantiate(hexPrefab, hex.Position, Quaternion.identity, this.transform);

                    hexTile.name = hex.ToString();

                    WorldTile tile = hexTile.GetComponent<WorldTile>();
                    _tiles.Add(tile);

                    tile.X = column;
                    tile.Z = row;
                }
            }
        }

        private void GenerateWorld()
        {
            DestroyWorld();
            for (var column = 0; column < _width; column++)
            {
                for (var row = 0; row < _depth; row++)
                {
                    float xPosition = column * xOffset;
                    if (row % 2 == 1)
                    { xPosition += xOffset / 2; }

                    var position = new Vector3(xPosition, 0, row * zOffset);

                    WorldTile tileObject = Instantiate(_tilePrefab, position, Quaternion.identity, this.transform);
                    tileObject.name = $"{column},{row}";
                    _tiles.Add(tileObject);

                    var tile = tileObject.GetComponent<WorldTile>();
                    tile.X = column;
                    tile.Z = row;
                }
            }
        }

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
                WorldTile tile = t.GetComponent<WorldTile>();
                return tile.X == x && tile.Z == z;
            });
        }
    }
}
