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
        [SerializeField] private GameObject _tilePrefab;

        private readonly List<GameObject> _tiles = new List<GameObject>();

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
                    var tileObject = Instantiate(_tilePrefab, position, Quaternion.identity, this.transform);
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
            foreach (var tile in _tiles)
            {
                Destroy(tile);
            }
        }

        public GameObject GetTileAt(int x, int z)
        {
            return _tiles.SingleOrDefault(t =>
            {
                var tile = t.GetComponent<WorldTile>();
                return tile.X == x && tile.Z == z;
            });
        }
    }
}
