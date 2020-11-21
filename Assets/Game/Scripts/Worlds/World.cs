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
        [SerializeField] private List<Character> _enemies;

        private readonly Dictionary<HexTile, WorldTile> _tiles = new Dictionary<HexTile, WorldTile>();
        private readonly Dictionary<HexTile, Character> _characters = new Dictionary<HexTile, Character>();

        private void Awake()
        {
            GenerateMap();
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
                        if (worldTile is { })
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
            return _characters.TryGetValue(hexTile, out var value)
                ? value
                : null;
        }

        public IEnumerable<Character> GetCharactersWithinRange(int q, int r, int range)
        {
            var worldTiles = GetTilesWithinRange(q, r, range);
            foreach (var worldTile in worldTiles)
            {
                var character = GetCharacterAt(worldTile.Hex);
                if (character is { })
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
            var characters = new[] { _player }.Concat(_enemies);
            foreach (var character in characters)
            {
                var q = character.Q;
                var r = character.R;

                while (true)
                {
                    var tile = GetTileAt(q, r);
                    if (tile is null)
                    {
                        q = random.Next(_radius * 2) - _radius - 1;
                        r = random.Next(_radius * 2) - _radius - 1;
                        continue;
                    }

                    tile.occupyingObject = character.gameObject;
                    character.lastTile = tile;
                    character.transform.position = tile.transform.position + stepOffset;
                    character.Q = q;
                    character.R = r;
                    break;
                }
            }
        }
    }
}
