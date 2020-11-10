﻿using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.Extensions;
using UnityEngine;

namespace Roguelite.Core
{
    public class World : MonoBehaviour
    {
        [Header("World Size")]
        [SerializeField] private int _radius = 16;

        [Header("Prefabs")]
        [SerializeField] private WorldTile _tilePrefab;
        [SerializeField] private GameObject hexPrefab;

        private readonly float xOffset = .8660254f;
        private float zOffset = .76f;

        public float XOffset { get { return xOffset; } }

        private readonly List<WorldTile> _tiles = new List<WorldTile>();

        private void Awake()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            DestroyWorld();

            for (var x = -_radius; x <= _radius; x++)
            {
                for (var y = -_radius; y <= _radius; y++)
                {
                    for (var z = -_radius; z <= _radius; z++)
                    {
                        if (x + y + z != 0)
                        {
                            continue;
                        }

                        var hex = new HexTile(x, z);

                        var hexTile = Instantiate(hexPrefab, hex.GetWorldPosition(), Quaternion.identity, this.transform);
                        var tile = hexTile.GetComponent<WorldTile>();
                        tile.Hex = hex;

                        _tiles.Add(tile);
                    }
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

        public WorldTile GetTileAt(int q, int r)
        {
            return _tiles.SingleOrDefault(t =>
            {
                var tile = t.GetComponent<WorldTile>();
                return tile.Hex.Q == q && tile.Hex.R == r;
            });
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
