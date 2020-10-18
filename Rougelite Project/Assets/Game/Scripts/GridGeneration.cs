using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;

    private void Start()
    {
        for (var z = 0; z < 10; z++)
        {
            for (var x = 0; x < 5; x++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity, this.transform);
                tile.name = $"{x},{z}";
            }
        }
    }
}
