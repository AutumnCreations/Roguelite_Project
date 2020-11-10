using System.Collections.Generic;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Core
{
    public class World : MonoBehaviour
    {
        [Header("World Size")]
        [SerializeField] private int _radius = 16;

        [Header("Prefabs")]
        [SerializeField] private WorldTile _tilePrefab;
        [SerializeField] private GameObject hexPrefab;

        private readonly Dictionary<HexTile, WorldTile> _tiles = new Dictionary<HexTile, WorldTile>();

        private void Awake()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            var random = new System.Random();

            for (var x = -_radius; x <= _radius; x++)
            {
                for (var y = -_radius; y <= _radius; y++)
                {
                    for (var z = -_radius; z <= _radius; z++)
                    {
                        if (x + y + z != 0 || random.Next(10) == 0)
                        {
                            continue;
                        }

                        var hex = new HexTile(x, z);

                        var hexTile = Instantiate(hexPrefab, hex.GetWorldPosition(), Quaternion.identity, this.transform);
                        var tile = hexTile.GetComponent<WorldTile>();
                        tile.Hex = hex;

                        _tiles.Add(hex, tile);
                    }
                }
            }
        }

        public WorldTile GetTileAt(int q, int r)
        {
            return _tiles.TryGetValue(new HexTile(q, r), out var value)
                ? value
                : null;
        }

        public IEnumerable<WorldTile> GetTilesWithinRange(int q, int r, int range)
        {
            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    for (var z = -range; z <= range; z++)
                    {
                        if (x + y + z != 0)
                        {
                            continue;
                        }

                        var worldTile = GetTileAt(q + x, r + y);
                        if (worldTile is { })
                        {
                            yield return worldTile;
                        }
                    }
                }
            }
        }
    }
}
