using System.Collections.Generic;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    [Header("World size")]
    [SerializeField] private int _width = 16;
    [SerializeField] private int _depth = 9;

    [Header("Prefabs")]
    [SerializeField] private GameObject _tilePrefab;

    private List<GameObject> _tiles;

    private void Start()
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
                var position = new Vector3(x - _width / 2f, 0, z - _depth / 2f);
                var tile = Instantiate(_tilePrefab, position, Quaternion.identity, this.transform);
                tile.name = $"{x},{z}";

                _tiles.Add(tile);
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
}
