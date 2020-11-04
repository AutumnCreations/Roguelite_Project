using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Roguelite.Core
{
    public class World : MonoBehaviour
    {
        [Header("World size")]
        [SerializeField] private int _width = 16;
        [SerializeField] private int _depth = 9;

        [Header("Prefabs")]
        [SerializeField] private WorldTile _tilePrefab;

        private readonly List<WorldTile> _tiles = new List<WorldTile>();

        private void Awake()
        {
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            DestroyWorld();

            for (var z = 0; z < _depth; z++)
            {
                for (var x = 0; x < _width; x++)
                {
                    var position = new Vector3(x - _width / 2f - 0.5f, 0, z - _depth / 2f - 0.5f);
                    WorldTile tileObject = Instantiate(_tilePrefab, position, Quaternion.identity, this.transform);
                    tileObject.name = $"{x},{z}";
                    _tiles.Add(tileObject);

                    //var tile = tileObject.AddComponent<WorldTile>();
                    WorldTile tile = tileObject.GetComponent<WorldTile>();
                    tile.X = x;
                    tile.Z = z;
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

            //foreach (WorldTile tile in _tiles)
            //{
            //    if (tile.X == x && tile.Z == z)
            //    {
            //        return tile;
            //    }
            //}

            //return null;

        }


        //First attempt at displaying Spell range

        //public List<WorldTile> GetSurroundingTiles(WorldTile targetTile, int range)
        //{
        //    List<WorldTile> tiles = new List<WorldTile>();

        //    for (int i = 0; i <= range; i++)
        //    {
        //        tiles.Add(GetTileAt(targetTile.X + i, targetTile.Z));
        //        tiles.Add(GetTileAt(targetTile.X, targetTile.Z + i));
        //        tiles.Add(GetTileAt(targetTile.X + i, targetTile.Z + i));
        //        tiles.Add(GetTileAt(targetTile.X + i, targetTile.Z - i));
        //        tiles.Add(GetTileAt(targetTile.X - i, targetTile.Z + i));
        //        tiles.Add(GetTileAt(targetTile.X - i, targetTile.Z));
        //        tiles.Add(GetTileAt(targetTile.X, targetTile.Z - i));
        //        tiles.Add(GetTileAt(targetTile.X - i, targetTile.Z - i));

        //    }
        //    return tiles;
        //}
    }
}
