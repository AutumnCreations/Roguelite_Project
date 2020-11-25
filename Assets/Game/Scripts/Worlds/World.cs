using System.Collections.Generic;
using System.Linq;
using Scripts.Characters;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Worlds
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Vector3 stepOffset = new Vector3(0, .05f);

        [Header("World Size")]
        [SerializeField] private int _radius = 16;

        [Header("Prefabs")]
        [SerializeField] private GameObject hexPrefab;

        [Header("Characters")]
        [SerializeField] private Character _player;
        [SerializeField] private EnemyContainer _enemies;

        private readonly Dictionary<HexTile, WorldTile> _tiles = new Dictionary<HexTile, WorldTile>();

        private void Awake()
        {
            GenerateMap();
        }

        private void Start()
        {
            PlaceCharacters();
        }

        public WorldTile GetTileAt(int q, int r)
        {
            var hexTile = new HexTile(q, r);
            return GetTileAt(hexTile);
        }

        private WorldTile GetTileAt(HexTile hexTile)
        {
            return _tiles.TryGetValue(hexTile, out var value)
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
                        if (worldTile != null)
                        {
                            yield return worldTile;
                        }
                    }
                }
            }
        }

        public Character GetCharacterAt(int q, int r)
        {
            var hexTile = new HexTile(q, r);
            return GetCharacterAt(hexTile);
        }

        private Character GetCharacterAt(HexTile hexTile)
        {
            var worldTile = GetTileAt(hexTile);
            if (worldTile == null || worldTile.occupyingObject == null)
            {
                return null;
            }

            return worldTile.occupyingObject.GetComponent<Character>();
        }

        public IEnumerable<Character> GetCharactersWithinRange(int q, int r, int range)
        {
            var worldTiles = GetTilesWithinRange(q, r, range);
            foreach (var worldTile in worldTiles)
            {
                var character = GetCharacterAt(worldTile.Hex);
                if (character != null)
                {
                    yield return character;
                }
            }
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

        private void PlaceCharacters()
        {
            var random = new System.Random();
            var characters = new[] { _player }.Concat(_enemies.Characters.Select(e => e.Character));
            List<WorldTile> worldTiles = new List<WorldTile>();

            foreach (var worldTile in _tiles.Values)
            {
                worldTiles.Add(worldTile);
            }

            foreach (var character in characters)
            {
                var index = random.Next(worldTiles.Count);
                var tile = worldTiles[index];
                character.Stats.Q = tile.Hex.Q;
                character.Stats.R = tile.Hex.R;

                character.CurrentTile = tile;
                character.transform.position = tile.transform.position + stepOffset;

                worldTiles.RemoveAt(index);
            }

        }
    }
}
